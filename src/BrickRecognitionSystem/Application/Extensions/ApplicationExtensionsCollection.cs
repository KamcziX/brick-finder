using System.Reflection;
using BrickManager.BrickRecognitionSystem.Application.Commands;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ImageManipulators;
using BrickManager.BrickRecognitionSystem.Application.ImagePredictors.ObjectDetection;
using Hellang.Middleware.ProblemDetails.Mvc;
using MediatR.Extensions.FluentValidation.AspNetCore;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace BrickManager.BrickRecognitionSystem.Application.Extensions;

public static class ApplicationExtensionsCollection
{
    public static void AddApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(c=>
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