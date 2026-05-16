namespace BrickManager.BrickRecognitionSystem.Application.Options;

/// <summary>Options for the ONNX object detection model.</summary>
public class ObjectDetectionOptions
{
    /// <summary>Path to the ONNX model file, relative to the application working directory.</summary>
    public string ModelFilePath { get; set; }
}
