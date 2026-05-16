# BrickRecognitionSystem.Application — Application Layer Rules

## Overview

This layer owns all business logic: MediatR commands, ML predictors, image processing utilities, DTOs, and options. It must not contain HTTP concerns or reference the Api layer.

---

## Commands & MediatR

- Commands are records implementing `IRequest<TResult>` (see `Commands/IdentifyPictureCommand.cs`)
- Handlers implement `IRequestHandler<TCommand, TResult>`
- New handlers are auto-discovered via assembly scan from `IdentifyPictureCommand` — no manual registration needed
- Register new services (predictors, converters) explicitly in `ApplicationExtensionsCollection.AddApplicationServices()` (`Extensions/ApplicationExtensionsCollection.cs`)

---

## ML Rules

- All ML classes must extend `BaseMl` (`ImagePredictors/Base/BaseMl.cs`) — it provides the shared `MLContext` with fixed seed `2137`
- ONNX model pipeline column names are defined in `ObjectDetectionConstants.cs` — do not hardcode them elsewhere
- The ONNX model and pipeline are cached as static fields in `ObjectDetectionModelScorer` — do not reload the model per request
- Do not load ML models in constructors — use lazy static initialization as per `ObjectDetectionModelScorer.LoadModel()`

---

## Image Processing Rules

- Always convert Base64 input to image via `IImageConverter.ConvertToImageFromBase64()` (`ImageManipulators/ImageConverter.cs`)
- Use `ImageEditor` static methods (`ImageManipulators/ImageEditor.cs`) for all image manipulation: resizing, canvas repositioning, bounding box drawing, and cropping
- Bounding box crops are always 128×128 px centered on the box center point — do not change without updating `ImageEditor.CutUsingBoundingBox()`
- Detection score threshold is currently hardcoded at `0.4F` in `ObjectDetectionPredictor` — do not add a second threshold elsewhere until this is made configurable via options

---

## Options Rules

- Options classes live in `Application/Options/`
- The class name must match the appSettings section name exactly (e.g. `ApiOptions` → `"ApiOptions"` section)

---

## What Claude Should Not Do

- Do not reload the ONNX or TensorFlow model per request — use the static cached instances
- Do not hardcode ONNX column names — use constants from `ObjectDetectionConstants.cs`
- Do not add new ML or image-processing services without registering them in `AddApplicationServices()`
