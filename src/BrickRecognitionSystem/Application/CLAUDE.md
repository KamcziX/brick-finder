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

## DTO Rules

- Domain DTOs (from Core) may be used freely within the Application layer
- Never return a domain DTO from a command/query handler — map to a `*ResponseDto` before returning
- DTOs are organized under `Application/Dto/<TaskName>/` where `<TaskName>` matches the command or use case (e.g. `IdentifyPicture`):
  - `Dto/<TaskName>/Requests/` — request DTOs named `*RequestDto` (e.g. `Dto/IdentifyPicture/Requests/IdentifyPictureRequestDto.cs`)
  - `Dto/<TaskName>/Responses/` — response DTOs named `*ResponseDto` (e.g. `Dto/IdentifyPicture/Responses/IdentificationResultResponseDto.cs`)
- All DTOs must be `sealed record` unless explicitly designed for inheritance
- Commands and Queries must be `sealed record`
- Command handlers and Query handlers must be `internal sealed class`

---

## Options Rules

- Options classes live in `Application/Options/`
- The class name must match the appSettings section name exactly (e.g. `ApiOptions` → `"ApiOptions"` section)

---

## ML Model Versioning

- Model file names must include a version suffix: `lego-detection-v1.onnx`, `lego-detection-v2.onnx`, etc. — never overwrite an existing model file
- Model file paths must be configurable via options (not hardcoded in code). Add an options class in `Application/Options/` and read the path from appSettings — do not use the static string in `ObjectDetectionModelScorer` for new models
- Before replacing the active model, validate it manually against a fixed set of test images and confirm it meets or exceeds the previous model's detection accuracy
- When a model is updated, add an entry to `ImagePredictors/ObjectDetection/Data/MODELS.md` with: model filename, date, and a short description of what changed (e.g. retrained on new dataset, architecture change)

---

## What Claude Should Not Do

- Do not reload the ONNX or TensorFlow model per request — use the static cached instances
- Do not hardcode ONNX column names — use constants from `ObjectDetectionConstants.cs`
- Do not add new ML or image-processing services without registering them in `AddApplicationServices()`
- Do not overwrite an existing model file — always use a versioned filename and update appSettings to point to the new one
