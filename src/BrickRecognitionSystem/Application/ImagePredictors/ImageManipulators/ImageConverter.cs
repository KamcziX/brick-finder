using System.Drawing;
using Microsoft.ML.Data;

namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ImageManipulators;

public interface IImageConverter
{
    Image ConvertToImageFromBase64(string imageToConvert);
}

public class ImageConverter : IImageConverter
{
    public Image ConvertToImageFromBase64(string imageToConvert)
    {
        return Bitmap.FromStream(new MemoryStream(Convert.FromBase64String(imageToConvert)));
    }
}