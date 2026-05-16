# BrickRecognitionSystem — Context-Specific Rules

## Overview

BrickRecognitionSystem is an ML-based bounded context responsible for identifying LEGO bricks in images. It uses ONNX object detection to locate bricks and TensorFlow classification to identify them.

It currently has two layers:

- **Application** — commands, DTOs, ML predictors, image processing utilities, options
- **Api** — ASP.NET Core entry point, endpoint registration, configuration wiring

No Infrastructure layer exists yet. Each layer has its own CLAUDE.md with rules specific to that project.

---

## Layer Responsibilities

| Layer | Owns | Must Not Contain |
|---|---|---|
| Application | Commands, DTOs, ML predictors, image processing, options | HTTP concerns, endpoint logic |
| Api | Endpoints, Startup, Program, configuration wiring | Business logic, ML calls |
