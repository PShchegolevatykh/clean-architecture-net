## Project Overview

Assume a modern .NET backend using:

* ASP.NET Core
* Clean Architecture
* Domain-Driven Design (DDD)
* CQRS

This file defines **non-negotiable architectural and coding constraints**.

---

## Architecture

Layers:

* Domain
* Application
* Infrastructure
* API

Rules:

* Domain has no dependencies
* Application depends only on Domain
* Infrastructure depends on Application and Domain
* API depends on Application
* No circular dependencies
* Do not violate layer boundaries

---

## Domain

* Encapsulate business logic inside entities
* Avoid anemic domain models
* Use Value Objects for domain concepts
* Enforce invariants inside aggregates
* No framework or infrastructure dependencies
* Domain must remain synchronous and pure

---

## Application

* Use CQRS:

  * Commands mutate state
  * Queries read state
* One handler per request
* Handlers orchestrate, not implement core business logic
* Business rules belong in Domain
* Validate all inputs
* No business logic in API layer

---

## Infrastructure

* Infrastructure contains all external concerns (DB, HTTP, etc.)
* Do not leak ORM or external models outside Infrastructure
* Map persistence models to domain models explicitly
* Configuration must come from environment (no hardcoded secrets)

---

## API Layer

* Controllers must be thin:

  * Accept request
  * Call Application layer
  * Return response
* No business logic or data access
* Use DTOs for input/output
* Return standardized error responses

---

## Security

* Never trust external input; validate all inputs
* Enforce authorization at API/Application boundaries
* Do not expose internal domain or persistence models directly
* Avoid dynamic query construction; use parameterization/ORM
* Do not log sensitive data (tokens, passwords, personal data)
* Do not hardcode secrets; use configuration/environment
* Prefer deny-by-default access control
* Use framework-provided security features (authentication middleware, data protection APIs); avoid custom implementations

---

## Cancellation

* All async methods must accept and propagate CancellationToken
* Never use CancellationToken.None
* Do not create new tokens unless explicitly required
* Allow OperationCanceledException to propagate
* Domain layer must not depend on CancellationToken

---

## Runtime Principles

* Stateless services (no shared in-memory state)
* No reliance on local disk for persistence
* Configuration via environment
* Logs are structured and treated as event streams
* Support graceful shutdown via CancellationToken

---

## Authentication

* Use JWT-based authentication
* Validate issuer and audience
* Do not use HttpContext outside API layer

---

## Testing

* Unit test Domain logic
* Test Application handlers
* Prefer integration tests over excessive mocking

---

## Coding Standards

* Async/await end-to-end (no blocking calls)
* Avoid global/static state
* Use dependency injection
* Prefer explicit, readable code over clever abstractions

---

## Date/Time

* Store timestamps in UTC
* Avoid manual offset calculations
* Use proper timezone handling when required

---

## Performance

* Avoid N+1 queries
* Use pagination for large datasets
* Prefer projection for read models

---

## Error Handling

* Use domain/application-specific exceptions
* Map errors to standard API responses
* Do not expose internal details in production

---

## Logging

* Use structured logging
* Do not log sensitive data
* Log at system boundaries (API, Infrastructure)

---

## Agent Guidelines

When generating code:

* Respect architecture boundaries
* Keep Domain pure
* Place business logic in Domain, orchestration in Application
* Keep API layer thin
* Use async and propagate CancellationToken
* Validate inputs
* Avoid unnecessary abstractions

If uncertain:

* Prefer clarity over cleverness
* Do not assume missing requirements
