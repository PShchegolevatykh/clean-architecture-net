# .NET Clean Architecture & Vertical Slice Guidelines

This document provides architectural rules and coding standards for AI agents working on this project.

## 1. Core Architecture: Vertical Slice & Clean Architecture

### Project Structure
The project is organized into four layers:
- `Domain`: Enterprise logic (Entities, Value Objects, Domain Exceptions). No dependencies.
- `Application`: Application logic (Features, CQRS Handlers, Behaviors, Interfaces). Depends on `Domain`.
- `Infrastructure`: External concerns (EF Core, Logging, File System). Depends on `Application` and `Domain`.
- `Web`: API Layer (Controllers, Middleware). Depends on `Application`.

### Feature-Based Folders (Vertical Slices)
All business logic is grouped by feature in `src/CleanArchitecture.Application/Features`.
**Structure:** `Features/{FeatureName}/{Commands|Queries}/{ActionName}/{ActionName}.cs`

**Example:**
```
/Features/Flashcards
  /Commands
    /CreateFlashcard
      CreateFlashcard.cs (Command, Validator, Handler)
    /DeleteFlashcard
      DeleteFlashcard.cs
  /Queries
    /GetFlashcard
      GetFlashcard.cs (Query, DTO, Handler)
```
- **Rule:** Keep the Request (Command/Query), Validator, DTOs, and Handler in the SAME file for each action. This is the preferred structure for AI-assisted development as it provides the full context of a vertical slice in one place.
- **Rule:** In larger, complex enterprise scenarios where a single file exceeds ~500 lines or contains too many distinct responsibilities, you MAY split it into a folder (e.g., `Features/Flashcards/Commands/CreateFlashcard/{Command,Validator,Handler,Dto}.cs`), but this should be the exception, not the rule.
- **Rule:** Use `record` for Commands, Queries, and DTOs. Prefer primary constructor syntax for records.

---

## 2. Domain Layer Rules

- **Pure Domain:** No dependencies on EF Core, Data Annotations, or Web frameworks.
- **Behavior-Driven:** Encapsulate business rules within Entities. Avoid anemic models.
- **Base Class:** All entities must inherit from `BaseEntity` (provides `Id` as `Guid.CreateVersion7()`, `CreatedAt`, and `UpdatedAt`).
- **Strong Typing:** Use Value Objects for complex types. They must be immutable.
- **Validation:** Enforce invariants in constructors or methods. Throw `DomainException` or specific variants if rules are violated.

---

## 3. Application Layer (CQRS via MediatR)

- **Handlers:** One handler per request. Inject `IApplicationDbContext` for data access.
- **Validation:** Every Command MUST have a `FluentValidation` validator.
- **Purity:** Logic in Handlers should focus on orchestration. Complex rules belong in the Domain.
- **DTOs:** Return DTOs from Queries. Return minimal data (e.g., ID) from Commands.

---

## 4. Infrastructure & Persistence

- **ORM:** Entity Framework Core (PostgreSQL in production, SQLite for local dev).
- **Configurations:** Use `IEntityTypeConfiguration<T>` in `Infrastructure/Persistence/Configurations`. No attributes in Domain.
- **Migrations:** Managed in the Infrastructure layer.
- **DbContext:** `ApplicationDbContext` implements `IApplicationDbContext`.

---

## 5. CancellationToken Usage (MANDATORY)

Cancellation must be respected across the entire pipeline.
- **API:** Pass `HttpContext.RequestAborted` to MediatR: `mediator.Send(request, HttpContext.RequestAborted)`.
  - *Current project note:* Some controllers currently omit this; newly generated code MUST include it.
- **Application:** Every `Handle` method MUST use the provided `cancellationToken`.
- **Infrastructure:** All async EF Core calls (e.g., `ToListAsync`, `FirstOrDefaultAsync`, `SaveChangesAsync`) MUST receive the `cancellationToken`.
- **Rule:** Never use `CancellationToken.None` unless there is a strictly justified reason.

---

## 6. API Layer (Web)

- **Controllers:** Thin controllers. Only call MediatR. No business logic.
- **Async:** All endpoints must be `async`.
- **Errors:** Handled via `ExceptionHandlingMiddleware`. Map Domain/Validation exceptions to `ProblemDetails`.

---

## 7. Testing

- **Framework:** xUnit.
- **Unit Tests:** For Domain logic and Value Objects.
- **Integration Tests:** For Feature Handlers using `IApplicationDbContext` (use In-Memory or real DB).
- **Naming:** `MethodName_Should_ExpectedBehavior_When_Condition`.

---

## 8. Coding Standards

- **Nullable:** Enable nullable reference types.
- **Implicit Usings:** Enabled.
- **C# 14:** Use modern C# features (Primary Constructors, File-Scoped Namespaces, etc.).
- **Consistency:** Match the style of existing files in the directory you are editing.
