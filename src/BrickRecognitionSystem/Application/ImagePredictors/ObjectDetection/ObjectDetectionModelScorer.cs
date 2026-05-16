using System.Drawing;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.Base;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection.DataModels;
using BrickManager.BrickRecognitionSystem.Application.Options;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection;

/// <summary>
/// Scores an image through the ONNX object detection model and returns raw detection output arrays.
/// </summary>
public interface IObjectDetectionModelScorer
{
    /// <summary>
    /// Runs the ONNX detection model on the provided bitmap and returns the raw output column values.
    /// </summary>
    /// <param name="bitmapOnnx">The pre-processed input bitmap, sized to match the model's expected dimensions.</param>
    /// <returns>A dictionary keyed by output column name containing the model's raw float array outputs.</returns>
    Dictionary<string, IEnumerable<float[]>> Score(Bitmap bitmapOnnx);
}

/// <summary>
/// Object detection predictor class that contains the pipeline to convert the input image to what the Onnx model expects,
/// runs the detection logic on the model and then returns the results.
/// </summary>
public sealed class ObjectDetectionModelScorer : BaseMl, IObjectDetectionModelScorer
{
    private readonly string _modelFilePath;
    private static EstimatorChain<Microsoft.ML.Transforms.Onnx.OnnxTransformer>? _pipeline;
    private static TransformerChain<Microsoft.ML.Transforms.Onnx.OnnxTransformer>? _model = null;

    public ObjectDetectionModelScorer(IOptions<ObjectDetectionOptions> options)
    {
        _modelFilePath = options.Value.ModelFilePath;
    }

    public Dictionary<string, IEnumerable<float[]>> Score(Bitmap bitmapOnnx)
    {
        var model = LoadModel();

        List<ImageDataInput> mlImages =
        [
            new ImageDataInput
            {
                Bitmap = bitmapOnnx
            }
        ];

        var data = MlContext.Data.LoadFromEnumerable(mlImages);
        return PredictDataUsingModel(data, model);
    }

    /// <summary>
    /// Load the model and return the ML transformer.
    /// </summary>
    /// <returns></returns>
    private ITransformer LoadModel()
    {
        if (_model != null)
            return _model;

        SetPipeline();
        var MLImages = new List<ImageDataInput>();
        var imageDataView = MlContext.Data.LoadFromEnumerable(MLImages);

        return _model = _pipeline!.Fit(imageDataView);
    }

    /// <summary>
    /// Set the ML.net pipeline which will transform the input image to the format
    /// that the ONNX model expects.
    /// </summary>
    private void SetPipeline()
    {
        if (_pipeline != null)
        {
            return;
        }

        // Define the pipeline. This pipeline transforms the input image to what the Onnx model expects.
        _pipeline = MlContext.Transforms.ResizeImages(
                outputColumnName: "input_tensor",
                imageWidth: 512,
                imageHeight: 512,
                inputColumnName: nameof(ImageDataInput.Bitmap),
                resizing: Microsoft.ML.Transforms.Image.ImageResizingEstimator.ResizingKind.IsoPad,
                cropAnchor: Microsoft.ML.Transforms.Image.ImageResizingEstimator.Anchor.Left)
            .Append(MlContext.Transforms.ExtractPixels(
                outputColumnName: "input_tensor",
                inputColumnName: null,
                colorsToExtract: Microsoft.ML.Transforms.Image.ImagePixelExtractingEstimator.ColorBits.Rgb,
                orderOfExtraction: Microsoft.ML.Transforms.Image.ImagePixelExtractingEstimator.ColorsOrder.ARGB,
                interleavePixelColors: true,
                offsetImage: 1,
                scaleImage: 1,
                outputAsFloatArray: false))
            .Append(MlContext.Transforms.ApplyOnnxModel(
                shapeDictionary: new Dictionary<string, int[]>()
                {
                    { "input_tensor", new[] { 1, 512, 512, 3 } }
                },
                modelFile: _modelFilePath,
                outputColumnNames: new[]
                {
                    ObjectDetectionConstants.OutputColumnDetectionAnchorIndices,
                    ObjectDetectionConstants.OutputColumnDetectionBoxes,
                    ObjectDetectionConstants.OutputColumnDetectionClasses,
                    ObjectDetectionConstants.OutputColumnDetectionScores
                },
                inputColumnNames: new[]
                {
                    ObjectDetectionConstants.InputColumn
                },
                gpuDeviceId: null,
                fallbackToCpu: true));
    }

    /// <summary>
    /// Transform method which does the actual prediction of the objects by using the model
    /// and returns the ONNX model output results.
    /// </summary>
    /// <param name="testData"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    private Dictionary<string, IEnumerable<float[]>> PredictDataUsingModel(IDataView testData, ITransformer model)
    {
        var scoredData = model.Transform(testData);

        var returns = new Dictionary<string, IEnumerable<float[]>>
        {
            {
                ObjectDetectionConstants.OutputColumnDetectionBoxes,
                scoredData.GetColumn<float[]>(ObjectDetectionConstants.OutputColumnDetectionBoxes)
            },
            {
                ObjectDetectionConstants.OutputColumnDetectionClasses,
                scoredData.GetColumn<float[]>(ObjectDetectionConstants.OutputColumnDetectionClasses)
            },
            {
                ObjectDetectionConstants.OutputColumnDetectionScores,
                scoredData.GetColumn<float[]>(ObjectDetectionConstants.OutputColumnDetectionScores)
            }
        };

        return returns;
    }
}
