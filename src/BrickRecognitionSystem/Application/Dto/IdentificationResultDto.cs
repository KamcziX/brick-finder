namespace BrickManager.BrickRecognitionSystem.Application.Dto;

/// <summary>
/// Data transfer object used to store a final result of identification lego bricks from picture process. 
/// </summary>
/// <param name="IdentifiedBricks">A <see cref="Dictionary{TKey,TValue}"/> of all identified bricks and their respective quantities</param>
public record IdentificationResultDto(IDictionary<string, int> IdentifiedBricks);