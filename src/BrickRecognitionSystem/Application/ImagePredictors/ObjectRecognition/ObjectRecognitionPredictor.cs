using System.Drawing;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.Base;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectRecognition.DataModels;
using Microsoft.ML;
using Serilog;

namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectRecognition;

public interface IObjectRecognitionPredictor
{
    
}

public class ObjectRecognitionPredictor : BaseMl
{
    private static readonly string ML_NET_MODEL = Path.Combine(Environment.CurrentDirectory, "chapter12.mdl");

    private ITransformer _model;

    public ImageDataPredictionItem Predict(string filePath) => 
        Predict(new TrainerImageDataInputItem 
            {
                ImagePath = filePath 
            }
        );
    
    public ImageDataPredictionItem Predict(TrainerImageDataInputItem image)
    {
        if (!File.Exists(ML_NET_MODEL))
        {
            Log.Error("Image prediction model not found.");

            throw new NullReferenceException("Image prediction model not found.");
        }
        
        _model = MlContext.Model.Load(ML_NET_MODEL, out var x);
        var predictor = MlContext.Model.CreatePredictionEngine<TrainerImageDataInputItem, ImageDataPredictionItem>(_model);

        return predictor.Predict(image);
    }
    
    public ImageDataPredictionItem Predict2(TrainerImageDataInputItem images)
    {
        if (!File.Exists(ML_NET_MODEL))
        {
            Log.Error("Image prediction model not found.");

            throw new NullReferenceException("Image prediction model not found.");
        }
        
        _model = MlContext.Model.Load(ML_NET_MODEL, out var x);
        var predictor = MlContext.Model.CreatePredictionEngine<TrainerImageDataInputItem, ImageDataPredictionItem>(_model);

        return predictor.Predict(images);
    }
}