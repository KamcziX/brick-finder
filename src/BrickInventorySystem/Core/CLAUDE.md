# BrickInventorySystem.Core — Domain Layer Rules

## Overview

This is the innermost layer of BrickInventorySystem. It has no dependencies on Infrastructure, EF Core, MediatR, or any external framework. All domain logic lives here.

---

## Domain Model Rules

- Entities must extend `Entity<TIdentifier>` (`SeedWork/Entity.cs`)
- Aggregates must extend `AggregateRoot<TIdentifier>` (`SeedWork/AggregateRoot.cs`)
- Value objects must extend `ValueObject<T>` (`SeedWork/ValueObject.cs`)
- Domain events must extend `DomainEvent` and implement `IDomainEvent` (`SeedWork/`)
- Raise domain events inside aggregates via `AddDomainEvent()` — never publish events directly
- All domain-level errors must use `DomainException(code, message)` (`SeedWork/DomainException.cs`)

---

## What Claude Should Not Do

- Do not reference Infrastructure, EF Core, MediatR, or any persistence concern from Core
- Do not publish domain events directly — raise them via `AddDomainEvent()` only; `UnitOfWork` handles publishing
