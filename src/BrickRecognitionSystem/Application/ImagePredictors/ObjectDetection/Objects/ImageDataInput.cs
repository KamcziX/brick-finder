using Microsoft.ML.Data;

namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection.Objects;

public class ImageDataInput
{
    [LoadColumn(0)]
    public string ImagePath;

    [LoadColumn(1)]
    public string Label;
}