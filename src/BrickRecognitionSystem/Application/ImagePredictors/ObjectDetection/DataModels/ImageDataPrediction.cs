using System.Drawing;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection.BoundingBoxes;
using Microsoft.ML.Data;

namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection.DataModels;

public class ImageDataPrediction
{
    public ImageDataPrediction(List<BoundingBox> boundingBoxList, Bitmap? annotatedImage)
    {
        BoundingBoxes = boundingBoxList;
        AnnotatedImage = annotatedImage;
    }
    
    /// <summary>
    /// Collection of bounding boxes (detected objects) metadata.
    /// </summary>
    public List<BoundingBox> BoundingBoxes { get; private set; }

    /// <summary>
    /// Original image with added (drawed on) bounding box annotations (detected objects).
    /// </summary>
    public Bitmap? AnnotatedImage { get; private set; }
}