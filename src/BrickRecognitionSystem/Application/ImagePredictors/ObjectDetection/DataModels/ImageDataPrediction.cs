using System.Drawing;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection.BoundingBoxes;
using Microsoft.ML.Data;

namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection.DataModels;

public class ImageDataPrediction(List<BoundingBox> boundingBoxList, Bitmap? annotatedImage)
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