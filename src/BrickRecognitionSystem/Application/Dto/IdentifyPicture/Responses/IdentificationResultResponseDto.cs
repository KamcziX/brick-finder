namespace BrickManager.BrickRecognitionSystem.Application.Dto.IdentifyPicture.Responses;

/// <summary>
/// Response DTO used to return the final result of the LEGO brick identification process.
/// </summary>
/// <param name="IdentifiedBricks">A <see cref="Dictionary{TKey,TValue}"/> of all identified bricks and their respective quantities.</param>
public sealed record IdentificationResultResponseDto(IDictionary<string, int> IdentifiedBricks);
