---
name: create-branch
description: Creates a new git branch following the repo naming convention (feature/, fix/, chore/). Use this whenever the user wants to start work on a new branch, create a branch, or begin a new feature/fix/chore. Accepts an optional type and description as arguments.
---

Create a new git branch following the project's naming convention.

## Arguments
$ARGUMENTS

If arguments were provided, parse them as: `<type> <short-description>` (e.g. `feature add-inventory-endpoint`).
If no arguments were provided, ask the user for the branch type and description before proceeding.

## Branch naming convention
- `feature/<short-description>` — new functionality
- `fix/<short-description>` — bug fix
- `chore/<short-description>` — maintenance, tooling, docs, config

The short description should be lowercase, hyphen-separated, and concise (2-4 words).

## Steps

1. Confirm the intended branch name with the user before creating it (e.g. "Creating branch `feature/add-inventory-endpoint` from `main` — does that look right?"). Wait for confirmation.

2. Fetch the latest state of the remote and create the branch from `origin/main`:
   ```
   git fetch origin
   git checkout -b <branch-name> origin/main
   ```

3. Push the branch to the remote and set the upstream tracking:
   ```
   git push -u origin <branch-name>
   ```

4. Confirm success by telling the user the branch name and that it's been pushed and tracked.

If anything fails (e.g. the branch already exists, or there's no remote), report the error clearly and suggest a fix.
