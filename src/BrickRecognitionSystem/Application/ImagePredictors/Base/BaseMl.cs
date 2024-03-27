using Microsoft.ML;

namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.Base;

public class BaseMl
{
    protected MLContext MlContext;

    public BaseMl()
    {
        MlContext = new MLContext(2137);
    }
}