using Microsoft.ML.Data;

namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection.Objects;

public class ImageDataPrediction
{
    [ColumnName("grid")]
    public float[] PredictedLabels;
}