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
        var ratioX = (double)width / image.Width;
        var ratioY = (double)height / image.Height;
        
        // Use whichever multiplier is smaller
        var ratio = ratioX < ratioY ? ratioX : ratioY;

        // Now calculate the new height and width
        var newHeight = Convert.ToInt32(image.Height * ratio);
        var newWidth = Convert.ToInt32(image.Width * ratio);

        var destRect = new Rectangle(0, 0, newWidth, newHeight);
        var destImage = new Bitmap(newWidth, newHeight);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using var graphics = Graphics.FromImage(destImage);
        graphics.CompositingMode = CompositingMode.SourceCopy;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        using var wrapMode = new ImageAttributes();
        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);

        return destImage;
    }
    
    /// <summary>
    /// Repositions and fills the image that has been resized to the exact specified size so the predicted bounding boxes do not seem offset.
    /// The input image will be placed in the top left corner of the canvas, all of the "empty" pixels will be set to black.
    /// </summary>
    /// <param name="image">Image to reposition</param>
    /// <returns>Repositioned image</returns>
    public static Bitmap RepositionImageOnExactSizeCanvas(Bitmap inputImageResized, int width, int height)
    {
        var bitmapOnnx = new Bitmap(width, height);
        bitmapOnnx.SetResolution(72, 72);
        
        using var graph = Graphics.FromImage(bitmapOnnx);
        var ImageSize = new Rectangle(0, 0, width, height);
        graph.FillRectangle(Brushes.Black, ImageSize);
        graph.DrawImageUnscaled(inputImageResized, 0, 0);

        return bitmapOnnx;
    }
    
    /// <summary>
    /// Draw a bounding box on an image.
    /// </summary>
    public static void DrawBoundingBox(BoundingBox bbox, ref Bitmap annotatedImage)
    {
        // annotatedImage.
        using var thumbnailGraphic = Graphics.FromImage(annotatedImage);
        
        thumbnailGraphic.CompositingQuality = CompositingQuality.HighQuality;
        thumbnailGraphic.SmoothingMode = SmoothingMode.HighQuality;
        thumbnailGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

        // Draw label string on image
        var drawFont = new Font("Arial", 36, FontStyle.Bold);
        var size = thumbnailGraphic.MeasureString(bbox.Label ?? "unknown", drawFont);
        var fontBrush = new SolidBrush(Color.Red);
        var atPoint = new Point(bbox.XStart, bbox.YStart - (int)size.Height - 1);
        thumbnailGraphic.DrawString(bbox.Label ?? "unknown", drawFont, fontBrush, atPoint);

        // Draw bounding box on image
        var pen = new Pen(Color.GreenYellow, 6f);
        thumbnailGraphic.DrawRectangle(pen, bbox.XStart, bbox.YStart, bbox.XEnd - bbox.XStart, bbox.YEnd - bbox.YStart);
    }
    
    /// <summary>
    /// Draw a bounding box on an image.
    /// </summary>
    public static Bitmap CutUsingBoundingBox(BoundingBox bbox, Bitmap annotatedImage)
    {
        const int imageWidth = 128;
        const int imageHeight = 128;
        
        var centerPoint = new Point(bbox.XEnd/2 + bbox.XStart/2, bbox.YEnd/2 + bbox.YStart/2);

        var startingX = centerPoint.X - imageHeight / 2;
        var startingY = centerPoint.Y - imageWidth / 2;
        
        if (startingX < 0)
            startingX = 0;
        
        if (startingY < 0)
            startingY = 0;
        
        var startingPoint = new Point(startingX,  startingY);
        var fixedIageSize = new Size(imageWidth, imageHeight);
        
        var croppedImage = annotatedImage.Clone(new Rectangle(startingPoint, fixedIageSize), annotatedImage.PixelFormat);
        return croppedImage;
    }

}