using System.Drawing;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;

namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectRecognition.DataModels;

public class ImageDataInput
{
    [LoadColumn(0)]
    [ImageType(512, 512)]
    public Bitmap Bitmap { get; set; }
    
    [LoadColumn(1)]
    public string Label;
}