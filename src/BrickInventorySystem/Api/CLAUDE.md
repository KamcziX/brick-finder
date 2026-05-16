# BrickInventorySystem.Api — Hosting Layer Rules

## Overview

This layer owns the ASP.NET Core entry point, endpoint registration, and configuration wiring. It must not contain business logic.

---

## Configuration Rules

- Bind config sections using `ConfigurationExtensions.BindFromSection<T>()` (`ConfigurationExtensions.cs`)
- Both `appSettings.Development.json` and `appSettings.Production.json` must define the same sections — keep them in sync

---

## What Claude Should Not Do

- Do not put business logic in `Startup.cs` or `Program.cs`
