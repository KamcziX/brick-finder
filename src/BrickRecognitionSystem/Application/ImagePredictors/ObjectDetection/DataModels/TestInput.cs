using Microsoft.ML.Data;

namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection.DataModels;

public class TestInput
{
    [LoadColumn(0)]
    public byte[] input_tensor;
}