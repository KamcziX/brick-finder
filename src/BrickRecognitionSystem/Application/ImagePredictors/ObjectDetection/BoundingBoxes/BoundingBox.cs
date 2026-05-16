namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection.BoundingBoxes;

/// <summary>
/// Represents a detected object in an image, defined by its bounding box coordinates, label, and confidence score.
/// </summary>
public sealed class BoundingBox
{
    /// <summary>Index of this detection within the model output.</summary>
    public int Id { get; set; }

    /// <summary>Class label assigned to the detected object (e.g. "Lego").</summary>
    public string Label { get; set; } = "";

    /// <summary>X coordinate of the left edge of the bounding box, in pixels.</summary>
    public int XStart { get; set; }

    /// <summary>Y coordinate of the top edge of the bounding box, in pixels.</summary>
    public int YStart { get; set; }

    /// <summary>X coordinate of the right edge of the bounding box, in pixels.</summary>
    public int XEnd { get; set; }

    /// <summary>Y coordinate of the bottom edge of the bounding box, in pixels.</summary>
    public int YEnd { get; set; }

    /// <summary>Confidence score for this detection, in the range [0, 1].</summary>
    public float Score { get; set; }
}
