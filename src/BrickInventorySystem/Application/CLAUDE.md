# BrickInventorySystem.Application — Use-Case Layer Rules

## Overview

This layer owns commands, queries, DTOs, and strongly-typed options. It must not reference EF Core or perform direct database access.

---

## Options Rules

- Options classes live in `Application/Options/`
- The class name must match the corresponding appSettings section name exactly (e.g. `ApiOptions` → `"ApiOptions"` section)
