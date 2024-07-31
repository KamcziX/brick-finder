namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectRecognition.DataModels;

public class ImageDataPredictionItem : TrainerImageDataInputItem
{
    public float[] Score;

    public string PredictedLabelValue;
}