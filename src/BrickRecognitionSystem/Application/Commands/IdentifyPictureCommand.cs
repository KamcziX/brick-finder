using BrickManager.BrickRecognitionSystem.Application.Dto;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection;
using MediatR;

namespace BrickManager.BrickRecognitionSystem.Application.Commands;

/// <summary>
/// Command used in picture identification process
/// </summary>
/// <param name="Picture"></param>
public record IdentifyPictureCommand(string PictureName) : IRequest<IdentificationResultDto>;

/// <inheritdoc cref="IdentifyPictureCommand"/>
public class IdentifyPictureCommandHandler(IObjectDetectionPredictor objectDetectionPredictor) 
    : IRequestHandler<IdentifyPictureCommand, IdentificationResultDto>
{
    public async Task<IdentificationResultDto> Handle(IdentifyPictureCommand request, 
        CancellationToken cancellationToken)
    {
        var x = objectDetectionPredictor.Predict(request.PictureName);
        return new IdentificationResultDto(new Dictionary<string, int>());
    }
}
