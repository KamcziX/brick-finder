# BrickManager — Solution-Wide Rules

## Solution Overview

Single Visual Studio solution (`BrickManager.sln`) containing two bounded contexts and one shared infrastructure project:

- **BrickInventorySystem** — manages brick inventory (Core, Application, Infrastructure, Api)
- **BrickRecognitionSystem** — image-based brick recognition via ML (Application, Infrastructure, Api)
- **Infrastructure** (root) — shared cross-cutting concerns (persistence, external integrations)

Target framework: .NET 8.0, C#, nullable reference types enabled, implicit usings enabled.

Each bounded context has its own CLAUDE.md with layer-specific rules. This file covers rules that apply everywhere.

---

## Git Workflow

- Branch naming: `feature/<short-description>`, `fix/<short-description>`, `chore/<short-description>`
- All changes merged to `main` via pull request — never commit directly to `main`
- One logical concern per branch; keep branches short-lived
- Commit messages: imperative mood, present tense (`feat: Add recognition endpoint`, `fix: Correct null check in aggregate`)

---

## Architecture Rules

- **No business logic in Infrastructure or Api layers.** Infrastructure persists and integrates; Api receives and routes. All logic belongs in Application or Core.
- Each bounded context is self-contained. Do not reference one system's internals from another — communicate through shared contracts or events only.
- The shared root `Infrastructure` project may be referenced by both systems but must contain no business logic itself.

---

## C# Conventions

- **Async all the way down.** Never call `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()` on a `Task`. Every I/O path must be `async`/`await`.
- **No raw SQL — EF Core only.** All database access goes through EF Core. No ADO.NET, no Dapper, no string-interpolated queries.
- Use `CancellationToken` parameters on all async public methods that perform I/O.
- Prefer records for immutable data transfer objects (DTOs, value objects).
- Do not use `static` classes or the singleton pattern manually — use the DI container.

---

## Commenting Rules

- Add XML doc comments (`///`) to all public types, methods, and properties — describe intent, not implementation.
- Do not comment private methods unless they cannot be simplified further. If a private method is hard to understand, simplify it first; only add a comment if simplification is not possible.
- Do not add comments that restate what the code already says (`// increment counter`).

---

## What Claude Should Not Do

- Do not add features, abstractions, or refactors beyond what the current task requires.
- Do not introduce backwards-compatibility shims for code that is simply being changed.
- Do not skip or work around failing tests — fix the root cause.
- Do not generate migrations automatically unless explicitly asked.
