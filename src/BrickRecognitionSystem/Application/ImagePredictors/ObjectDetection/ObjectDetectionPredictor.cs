using System.Drawing;
using System.Drawing.Imaging;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ImageManipulators;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection.BoundingBoxes;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection.DataModels;
using BrickManager.BrickRecognitionSystem.Application.Options;
using Microsoft.Extensions.Options;
using Microsoft.ML.Data;

namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection;

public interface IObjectDetectionPredictor
{
    ImageDataPrediction Predict(string fileName);
}

public class ObjectDetectionPredictor(IObjectDetectionModelScorer objectDetectionModelScorer,
    IImageConverter imageConverter) : IObjectDetectionPredictor
{
    
    public ImageDataPrediction Predict(string fileName)
    {
        var imageAsBase64 = LoadImageAsBase64(fileName);

        var image = imageConverter.ConvertToImageFromBase64(imageAsBase64);
        
        var annotatedImage = (Bitmap)image;

        // Current trained model expects images of 512x512 resolution. 
        // Model scorer is capable of resizing the model by itself but currently has troubles with keeping correct aspect ratio.
        // We use this method to resize the image to a slightly bigger version with correct aspect ratio
        // and later we allow the model to correct the size to a slightly smaller one.
        // At the moment is it impossible to skip that part in the model itself as it will throw an error hence we do it twice.
        // TODO: To remove resizing from here completely and expect the image to already arrive resized by now 
        int imageInputHeight = 513;
        int imageInputWidth = 513;
        Bitmap resizedImage = ImageEditor.ResizeImageWithAspectRatio(image, imageInputWidth, imageInputHeight);

        // Now place the resized picture (with aspect ratio) in top left of black canvas 321x321
        // The output we will get will be a 1:1 ratio image, with any "empty" pixels black.
        Bitmap bitmapOnnx = ImageEditor.RepositionImageOnExactSizeCanvas(resizedImage, imageInputWidth, imageInputHeight);
        
        Dictionary<string, IEnumerable<float[]>> probabilities = objectDetectionModelScorer.Score(bitmapOnnx);
        
        // Get the return values from the scoring.
        var detectionBoxes = probabilities.Where(x => x.Key == "detection_boxes").First().Value.ToList();
        var detectionClasses = probabilities.Where(x => x.Key == "detection_classes").First().Value.ToList();
        var detectionScores = probabilities.Where(x => x.Key == "detection_scores").First().Value.ToList();

        var formattedDetectionBoxes = FormatDetectionBoxes(detectionBoxes);

        var boundingBoxes = GetBoundingBoxes(detectionBoxes, detectionScores.ToList(), formattedDetectionBoxes, detectionClasses.ToList(), annotatedImage, image);

        foreach (var boundingBox in boundingBoxes)
        {
            ImageEditor.DrawBoundingBox(boundingBox, ref bitmapOnnx);
        }
        
        SavePredictedImage(bitmapOnnx);
        
        // TODO: This will return a dictionary of cropped images with singular lego object to pass onto further analysis 
        return new ImageDataPrediction(boundingBoxes, annotatedImage);
    }

    /// <summary>
    /// Returns class labels Dictionary with translation between ID and label that this model outputs.
    /// </summary>
    /// <returns>Dictionary of expected labels</returns>
    private static Dictionary<int, string> GetModelLabels()
        => new() { { 1, "Lego" } };

    /// <summary>
    /// TODO: This method is here only for testing purposes. To be removed when BrickRecognitionSystem is synced with BrickInventorySystem  
    /// </summary>
    private string LoadImageAsBase64(string fileName)
    {
        byte[] imageArray = File.ReadAllBytes(fileName);
        return Convert.ToBase64String(imageArray);
    }

    /// <summary>
    /// Formats retrieved detection boxes coordinates from a single array of values into a easily readable collection of coordinates grouped by 4. 
    /// </summary>
    /// <param name="detectionBoxes">Un</param>
    /// <returns></returns>
    private static IEnumerable<float[]> FormatDetectionBoxes(ICollection<float[]> detectionBoxes)
    {
        // The detectionBoxes list is in a Python numpy representation.
        // There are 400 entries in a singular array, while the actual return we should be having 100 entries with 4 values each.
        // To achieve this we chunk these 400 records to 100 records by 4 each
        // so with an array like [1,2,3,4,5,6,7,8] it will be converted to [1,2,3,4],[5,6,7,8] etc.
        const int rowSize = 4;
        var detectionBoxesArray = detectionBoxes.First();
        return detectionBoxesArray.Chunk(rowSize);
    }
    
    
    private static List<BoundingBox> GetBoundingBoxes(List<float[]> detectionBoxes,
        IReadOnlyList<float[]> detectionScores,
        IEnumerable<float[]> formattedDetectionBoxes,
        IReadOnlyList<float[]> detectionClasses, 
        Image annotatedImage,
        Image inputImageResized)
    {
        var boundingBoxesList = new List<BoundingBox>();
        var modelLabels = GetModelLabels();
        
        for (int imageCount = 0; imageCount < detectionBoxes.Count; imageCount++)
        {
            // TODO: Make this variable available to setup from appsettings
            var minScoreThreshold = 0.4F;

            int index = -1;
            var detectionScoresForImage = detectionScores[imageCount].Where(u => u >= minScoreThreshold);
            foreach (var score in detectionScoresForImage)
            {
                index++;

                var bbox = formattedDetectionBoxes.ElementAt(index);
                var detectedObjectClassId = detectionClasses[imageCount][index];

                // Figure out the ratio between the original inputImage and the resized
                // image, as the object coordinates are based on the resized image
                // and we want the coordinates based on the original image.
                double ratioX = (double)annotatedImage.Width / inputImageResized.Width;
                double ratioY = (double)annotatedImage.Height / inputImageResized.Height;

                // Add bounding boxes to list.
                var bb = new BoundingBox
                {
                    Id = index,
                    Label = modelLabels.GetValueOrDefault((int)detectedObjectClassId) ?? "unknown",
                    XStart = (int)(bbox[1] * 512 * ratioX),
                    XEnd = (int)(bbox[3] * 512 * ratioX),
                    YStart = (int)(bbox[0] * 512 * ratioY),
                    YEnd = (int)(bbox[2] * 512 * ratioY),
                    Score = score
                };
                boundingBoxesList.Add(bb);
            }
        }

        return boundingBoxesList;
    }
    
    [Obsolete("To be removed. Used only for testing purposes.")]
    private static void SavePredictedImage(Image annotatedImage)
    {
        var myImageCodecInfo = GetEncoderInfo("image/jpeg");
        var myEncoderParameters = new EncoderParameters(1);
        var myEncoderParameter = new EncoderParameter(Encoder.Quality, 75L);
        myEncoderParameters.Param[0] = myEncoderParameter;
        
        annotatedImage.Save("src/BrickRecognitionSystem/Application/ImagePredictors/ObjectDetection/Data/prediction.jpg");
    }
    
    private static ImageCodecInfo GetEncoderInfo(String mimeType)
    {
        int j;
        ImageCodecInfo[] encoders;
        encoders = ImageCodecInfo.GetImageEncoders();
        for(j = 0; j < encoders.Length; ++j)
        {
            if(encoders[j].MimeType == mimeType)
                return encoders[j];
        }
        return null;
    }
}