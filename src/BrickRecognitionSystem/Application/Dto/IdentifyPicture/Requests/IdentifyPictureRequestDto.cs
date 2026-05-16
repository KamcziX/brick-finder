namespace BrickManager.BrickRecognitionSystem.Application.Dto.IdentifyPicture.Requests;

/// <summary>Request body for the identify-picture endpoint.</summary>
/// <param name="PictureName">Name of the picture file to identify.</param>
public sealed record IdentifyPictureRequestDto(string PictureName);
