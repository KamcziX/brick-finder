# BrickInventorySystem.Application — Use-Case Layer Rules

## Overview

This layer owns commands, queries, DTOs, and strongly-typed options. It must not reference EF Core or perform direct database access.

---

## DTO Rules

- Domain DTOs (from Core) may be used freely within the Application layer
- Never return a domain DTO from a command/query handler — map to a `*ResponseDto` record before returning (e.g. `IdentificationResultResponseDto`)
- Response DTOs live in `Application/Dto/` and are named `*ResponseDto`

---

## Options Rules

- Options classes live in `Application/Options/`
- The class name must match the corresponding appSettings section name exactly (e.g. `ApiOptions` → `"ApiOptions"` section)
