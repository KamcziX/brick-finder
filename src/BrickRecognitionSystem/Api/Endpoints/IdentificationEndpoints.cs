using BrickManager.BrickRecognitionSystem.Application.Commands;
using BrickManager.BrickRecognitionSystem.Application.Dto.IdentifyPicture.Requests;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BrickManager.BrickRecognitionSystem.Api.Endpoints;

public static class IdentificationEndpoints
{
    public static void MapIdentificationEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/api/identification/identify-picture", async (ISender sender, IdentifyPictureRequestDto requestDto) =>
            {
                var command = new IdentifyPictureCommand(requestDto.PictureName);
                var result = await sender.Send(command);
                return Results.Ok(result);
            }).WithOpenApi()
            .WithTags("Identification")
            .WithName("IdentifyPicture");
    }
}
