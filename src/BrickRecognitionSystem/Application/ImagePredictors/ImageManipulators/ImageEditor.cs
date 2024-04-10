using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection.BoundingBoxes;
using Microsoft.ML.Data;

namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ImageManipulators;

public static class ImageEditor
{
    /// <summary>
    /// Resize the image to the specified width and height, keeping aspect ratio intact.
    /// </summary>
    /// <param name="image">Image to resize.</param>
    /// <param name="width">Width to resize to.</param>
    /// <param name="height">Height to resize to.</param>
    /// <returns>Resized image.</returns>
    public static Bitmap ResizeImageWithAspectRatio(Image image, int width, int height)
    {
        // Figure out the ratio
        double ratioX = (double)width / image.Width;
        double ratioY = (double)height / image.Height;
        
        // Use whichever multiplier is smaller
        double ratio = ratioX < ratioY ? ratioX : ratioY;

        // Now calculate the new height and width
        int newHeight = Convert.ToInt32(image.Height * ratio);
        int newWidth = Convert.ToInt32(image.Width * ratio);

        var destRect = new Rectangle(0, 0, newWidth, newHeight);
        var destImage = new Bitmap(newWidth, newHeight);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using (var graphics = Graphics.FromImage(destImage))
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }

        return destImage;
    }
    
    /// <summary>
    /// Repositions and fills the image that has been resized to the exact specified size so the predicted bounding boxes do not seem offset.
    /// The input image will be placed in the top left corner of the canvas, all of the "empty" pixels will be set to black.
    /// </summary>
    /// <param name="image">Image to reposition</param>
    /// <returns>Repositioned image</returns>
    public static Bitmap RepositionImageOnExactSizeCanvas(Bitmap image)
    {
        var resizedImageWidth = image.Width;
        var resizedImageHeight = image.Height;
        
        // MemoryStream ms = new();
        // onnxRepositionedImage.Save(ms, ImageFormat.Jpeg);
        // ms.Seek(0, SeekOrigin.Begin);
        // var mlImage = MLImage.CreateFromStream(ms);
        Bitmap bitmapOnnx = new Bitmap(resizedImageWidth, resizedImageHeight);
        MemoryStream ms = new();
        bitmapOnnx.Save(ms, ImageFormat.Jpeg);
        ms.Seek(0, SeekOrigin.Begin);
        
        bitmapOnnx.SetResolution(72, 72);
        using (Graphics graph = Graphics.FromImage(image))
        {
            Rectangle ImageSize = new Rectangle(0, 0, resizedImageWidth, resizedImageHeight);
            graph.FillRectangle(Brushes.Black, ImageSize);
            graph.DrawImageUnscaled(image, 0, 0);
        }


        
        return bitmapOnnx;
    }
    
    /// <summary>
    /// Repositions and fills the image that has been resized to the exact specified size so the predicted bounding boxes do not seem offset.
    /// The input image will be placed in the top left corner of the canvas, all of the "empty" pixels will be set to black.
    /// </summary>
    /// <param name="image">Image to reposition</param>
    /// <returns>Repositioned image</returns>
    public static Bitmap RepositionImageOnExactSizeCanvas2(Bitmap inputImageResized, int width, int height)
    {
        Bitmap bitmapOnnx = new Bitmap(width, height);
        bitmapOnnx.SetResolution(72, 72);
        using (Graphics graph = Graphics.FromImage(bitmapOnnx))
        {
            Rectangle ImageSize = new Rectangle(0, 0, width, height);
            graph.FillRectangle(Brushes.Black, ImageSize);
            graph.DrawImageUnscaled(inputImageResized, 0, 0);
        }

        return bitmapOnnx;
    }
    
    /// <summary>
    /// Draw a bounding box on an image.
    /// </summary>
    public static void DrawBoundingBox(BoundingBox bbox, ref Bitmap annotatedImage) 
    {
        // annotatedImage.
        using (Graphics thumbnailGraphic = Graphics.FromImage(annotatedImage))
        {
            thumbnailGraphic.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraphic.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw label string on image
            Font drawFont = new Font("Arial", 36, FontStyle.Bold);
            SizeF size = thumbnailGraphic.MeasureString(bbox.Label ?? "unknown", drawFont);
            SolidBrush fontBrush = new SolidBrush(Color.Red);
            Point atPoint = new Point(bbox.XStart, bbox.YStart - (int)size.Height - 1);
            thumbnailGraphic.DrawString(bbox.Label ?? "unknown", drawFont, fontBrush, atPoint);

            // Draw bounding box on image
            Pen pen = new Pen(Color.GreenYellow, 6f);
            thumbnailGraphic.DrawRectangle(pen, bbox.XStart, bbox.YStart, bbox.XEnd - bbox.XStart, bbox.YEnd - bbox.YStart);
        }
    }

}