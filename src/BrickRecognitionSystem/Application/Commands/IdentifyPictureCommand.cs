using BrickManager.BrickRecognitionSystem.Application.Dto.IdentifyPicture.Responses;

using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection;
using MediatR;

namespace BrickManager.BrickRecognitionSystem.Application.Commands;

/// <summary>
/// Command used in picture identification process.
/// </summary>
/// <param name="PictureName">Name of the picture file to identify.</param>
public sealed record IdentifyPictureCommand(string PictureName) : IRequest<IdentificationResultResponseDto>;

/// <inheritdoc cref="IdentifyPictureCommand"/>
internal sealed class IdentifyPictureCommandHandler(IObjectDetectionPredictor objectDetectionPredictor)
    : IRequestHandler<IdentifyPictureCommand, IdentificationResultResponseDto>
{
    public async Task<IdentificationResultResponseDto> Handle(IdentifyPictureCommand request,
        CancellationToken cancellationToken)
    {
        var x = objectDetectionPredictor.Predict(request.PictureName);
        return new IdentificationResultResponseDto(new Dictionary<string, int>());
    }
}
