using BrickManager.BrickRecognitionSystem.Application.Dto;
using MediatR;

namespace BrickManager.BrickRecognitionSystem.Application.Commands;

/// <summary>
/// Command used in picture identification process
/// </summary>
/// <param name="Picture"></param>
public record IdentifyPictureCommand(string Picture) : IRequest<IdentificationResultDto>;

/// <inheritdoc cref="IdentifyPictureCommand"/>
public class IdentifyPictureCommandHandler() 
    : IRequestHandler<IdentifyPictureCommand, IdentificationResultDto>
{
    public async Task<IdentificationResultDto> Handle(IdentifyPictureCommand request, 
        CancellationToken cancellationToken)
    {
        return default;
    }
}
