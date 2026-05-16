# BrickRecognitionSystem.Api — Hosting Layer Rules

## Overview

This layer owns the ASP.NET Core entry point, endpoint registration, and configuration wiring. It must not contain business logic or ML calls.

---

## Endpoint Rules

- Endpoints are registered as static `MapXxxEndpoints(IEndpointRouteBuilder)` extension methods under `Endpoints/`
- Register each endpoint group by calling its map method inside `Startup.Configure()` (`Startup.cs`)
- Dispatch commands via MediatR `ISender.Send()` — do not call Application services directly from endpoints
- Tag each endpoint group with an OpenAPI tag matching the resource name

---

## Configuration Rules

- Bind config sections using `ConfigurationExtensions.BindFromSection<T>()` (`ConfigurationExtensions.cs`)
- Both `appSettings.Development.json` and `appSettings.Production.json` must define the same sections — keep them in sync

---

## What Claude Should Not Do

- Do not put business logic or ML calls in `Startup.cs` or `Program.cs`
- Do not call Application services directly from endpoints — always dispatch through MediatR `ISender`
