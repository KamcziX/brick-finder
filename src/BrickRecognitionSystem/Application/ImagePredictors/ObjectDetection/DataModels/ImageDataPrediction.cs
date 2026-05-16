using System.Drawing;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection.BoundingBoxes;
using Microsoft.ML.Data;

namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection.DataModels;

/// <summary>
/// Result of the object detection pipeline, containing the detected bounding boxes and the original annotated image.
/// </summary>
/// <param name="boundingBoxList">List of bounding boxes for all detected objects.</param>
/// <param name="annotatedImage">The original image, optionally annotated with detected object overlays.</param>
public sealed class ImageDataPrediction(List<BoundingBox> boundingBoxList, Bitmap? annotatedImage)
{
    /// <summary>
    /// Collection of bounding boxes (detected objects) metadata.
    /// </summary>
    public List<BoundingBox> BoundingBoxes { get; private set; } = boundingBoxList;

    /// <summary>
    /// Original image with added (drawed on) bounding box annotations (detected objects).
    /// </summary>
    public Bitmap? AnnotatedImage { get; private set; } = annotatedImage;
}