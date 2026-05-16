# BrickInventorySystem.Application — Use-Case Layer Rules

## Overview

This layer owns commands, queries, DTOs, and strongly-typed options. It must not reference EF Core or perform direct database access.

---

## DTO Rules

- Domain DTOs (from Core) may be used freely within the Application layer
- Never return a domain DTO from a command/query handler — map to a `*ResponseDto` before returning
- DTOs are organized under `Application/Dto/<TaskName>/` where `<TaskName>` matches the command or use case (e.g. `CreateBrick`):
  - `Dto/<TaskName>/Requests/` — request DTOs named `*RequestDto` (e.g. `Dto/CreateBrick/Requests/CreateBrickRequestDto.cs`)
  - `Dto/<TaskName>/Responses/` — response DTOs named `*ResponseDto` (e.g. `Dto/CreateBrick/Responses/CreateBrickResponseDto.cs`)
- All DTOs must be `sealed record` unless explicitly designed for inheritance
- Commands and Queries must be `sealed record`
- Command handlers and Query handlers must be `internal sealed class`

---

## Options Rules

- Options classes live in `Application/Options/`
- The class name must match the corresponding appSettings section name exactly (e.g. `ApiOptions` → `"ApiOptions"` section)
