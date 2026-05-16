# BrickInventorySystem.Infrastructure — Persistence Layer Rules

## Overview

This layer owns all persistence concerns: EF Core context, MySQL setup, repositories, Unit of Work, and database migrations. No business logic belongs here.

---

## Unit of Work & Persistence Rules

- Always commit via `IUnitOfWork.CommitAsync(CancellationToken)` — never call `DbContext.SaveChangesAsync()` directly
- `UnitOfWork.CommitAsync()` (`MySQL/UnitOfWork.cs`) handles automatically:
  - Setting `UpdatedAt` on all modified entities
  - Collecting and clearing domain events from aggregates
  - Publishing domain events via MediatR in isolated scopes
- Entity type configurations belong in `EntityFrameworkDatabaseContext.OnModelCreating()` (`MySQL/EntityFrameworkDatabaseContext.cs`)
- Migrations are applied automatically on startup via `DatabaseMigrator` — do not apply manually in code
- Generate migrations with EF Core CLI only (`dotnet ef migrations add`); never write migration files by hand

---

## Repository Rules

- Every repository must have a corresponding interface defined in Core
- Register repositories in `InfrastructureExtensionsCollection.AddRepositories()` (`InfrastructureExtensionsCollection.cs`)
- Do not add a repository implementation without its Core interface

---

## What Claude Should Not Do

- Do not call `SaveChangesAsync()` directly — always use `IUnitOfWork`
- Do not publish domain events manually — `UnitOfWork.CommitAsync()` handles this
- Do not add a repository to `AddRepositories()` without a Core interface for it
- Do not generate EF Core migrations automatically unless explicitly asked
