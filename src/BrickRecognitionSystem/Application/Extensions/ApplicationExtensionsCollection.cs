using BrickManager.BrickRecognitionSystem.Application.Commands;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ImageManipulators;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection;
using MediatR.Extensions.FluentValidation.AspNetCore;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace BrickManager.BrickRecognitionSystem.Application.Extensions;

/// <summary>
/// Extension methods for registering BrickRecognitionSystem application services with the DI container.
/// </summary>
public static class ApplicationExtensionsCollection
{
    /// <summary>
    /// Registers MediatR, ML predictors, and image processing services required by the application layer.
    /// </summary>
    /// <param name="serviceCollection">The service collection to register services into.</param>
    public static void AddApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(c =>
        {
            c.NotificationPublisher = new TaskWhenAllPublisher();
            c.Lifetime = ServiceLifetime.Transient;
            c.RegisterServicesFromAssemblyContaining<IdentifyPictureCommand>();
        });

        serviceCollection.AddTransient<IObjectDetectionModelScorer, ObjectDetectionModelScorer>();
        serviceCollection.AddTransient<IObjectDetectionPredictor, ObjectDetectionPredictor>();
        serviceCollection.AddTransient<IImageConverter, ImageConverter>();
    }
}
