# BrickInventorySystem — Context-Specific Rules

## Overview

BrickInventorySystem is a DDD-structured bounded context responsible for managing brick inventory. It follows Clean Architecture across four layers:

- **Core** — domain model: aggregates, entities, value objects, domain events, interfaces
- **Application** — use cases: commands, queries, DTOs, strongly-typed options
- **Infrastructure** — persistence: EF Core context, repositories, Unit of Work, MySQL migrations
- **Api** — hosting: ASP.NET Core entry point, endpoint mapping, configuration wiring

Current state: infrastructure and domain scaffolding is complete. No domain entities or repositories have been implemented yet.

Each layer has its own CLAUDE.md with rules specific to that project.

---

## Layer Responsibilities

| Layer | Owns | Must Not Contain |
|---|---|---|
| Core | Aggregates, entities, value objects, domain events, `IUnitOfWork` | Infrastructure references, EF Core, MediatR calls |
| Application | Commands, queries, DTOs, `Options/` classes | EF Core, direct DB access |
| Infrastructure | `EntityFrameworkDatabaseContext`, repositories, `UnitOfWork`, migrations | Business logic |
| Api | `Program`, `Startup`, `ConfigurationExtensions`, endpoint registration | Business logic, direct DB access |
