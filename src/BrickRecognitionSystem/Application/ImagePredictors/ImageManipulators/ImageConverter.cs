using System.Drawing;
using Microsoft.ML.Data;

namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ImageManipulators;

/// <summary>
/// Converts raw image data into a usable <see cref="Image"/> instance.
/// </summary>
public interface IImageConverter
{
    /// <summary>
    /// Decodes a Base64-encoded image string into an <see cref="Image"/>.
    /// </summary>
    /// <param name="imageToConvert">A Base64-encoded string representation of the image.</param>
    /// <returns>The decoded <see cref="Image"/>.</returns>
    Image ConvertToImageFromBase64(string imageToConvert);
}

public sealed class ImageConverter : IImageConverter
{
    public Image ConvertToImageFromBase64(string imageToConvert)
    {
        return Bitmap.FromStream(new MemoryStream(Convert.FromBase64String(imageToConvert)));
    }
}