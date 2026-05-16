# BrickInventorySystem.Api — Hosting Layer Rules

## Overview

This layer owns the ASP.NET Core entry point, endpoint registration, and configuration wiring. It must not contain business logic.

---

## Request & Response DTO Rules

- If an endpoint accepts a request body that maps to a command, define a `*RequestDto` record in the `Api` layer alongside the endpoint (e.g. `IdentifyPictureRequestDto`)
- Map the `*RequestDto` to the command inside the endpoint handler — do not pass the RequestDto into the Application layer
- Endpoints must return a `*ResponseDto` from the Application layer — never return a domain type or a command/query object directly

---

## Configuration Rules

- Bind config sections using `ConfigurationExtensions.BindFromSection<T>()` (`ConfigurationExtensions.cs`)
- Both `appSettings.Development.json` and `appSettings.Production.json` must define the same sections — keep them in sync

---

## What Claude Should Not Do

- Do not put business logic in `Startup.cs` or `Program.cs`
