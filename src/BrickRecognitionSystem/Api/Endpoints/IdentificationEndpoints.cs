using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace BrickManager.BrickRecognitionSystem.Api.Endpoints;

public static class IdentificationEndpoints
{
    public static void MapIdentificationEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/api/identification/identify-picture", async (ISender sender) =>
        {
            return Results.Ok(42);
            
        }).WithOpenApi()
        .WithTags("Identification")
        .WithName("IdentifyPicture");
    }
}