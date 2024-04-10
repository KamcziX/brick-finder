using System.Drawing;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;

namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection.DataModels;

public class ImageDataInput
{
    [ImageType(512, 512)]
    public Bitmap Bitmap { get; set; }
}