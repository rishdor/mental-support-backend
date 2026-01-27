# Backend API – Firebase-authenticated ASP.NET Core

This repository contains a **production-oriented backend API** built for a mobile application delivering short-form mental support content (audio, onboarding flow, lightweight feedback collection).

The purpose of this project was to design, implement, and deploy a **real-world backend service** that handles authentication, persistence, and API behavior with production concerns in mind: configuration management, environment safety, failure handling, and deployment behavior.

The system is intentionally small in scope but **complete end-to-end**.

---

## System capabilities

The API provides:

* Firebase-based user authentication (ID token verification)
* Automatic user resolution and persistence on first request
* Content delivery with audio variants
* Onboarding state tracking
* Post-content survey logging
* Health endpoint for deployment monitoring

The application focuses on reliability, clarity, and correctness rather than feature breadth.

---

## Technology stack

* **ASP.NET Core**
* **Entity Framework Core**
* **PostgreSQL** (production) / InMemory DB (testing)
* **Firebase Admin SDK** (authentication)
* **Swagger** (development only)
* **xUnit** (smoke testing)

---

## Architectural approach

The system follows a **simple but deliberate service architecture**.

### Authentication boundary

Firebase token verification is handled in middleware. Controllers and services operate on resolved user identity and are decoupled from Firebase-specific concerns.

### User resolution layer

A dedicated service resolves or creates application users based on Firebase UID, supporting anonymous users and account transitions.

### Thin controllers

Controllers orchestrate requests. Business logic is implemented in services to keep HTTP concerns separate from application logic.

### Environment-aware behavior

Development, testing, and production environments behave differently by design. The application fails fast in production if required configuration is missing.

---

## Authentication model

Clients send Firebase ID tokens using:

```
Authorization: Bearer <token>
```

The middleware verifies the token and injects identity into the request context. Downstream layers never rely on raw tokens.

Authentication can be explicitly disabled in development and test environments. This is blocked in production.

---

## Persistence & database strategy

* EF Core with explicit model configuration
* Automatic migrations on startup (early-stage deployment convenience)
* Clear separation between relational and in-memory persistence
* Referential integrity enforced through cascade rules where appropriate

The goal is predictable behavior and data consistency without unnecessary abstraction.

---

## Error handling & safety

* Global exception handling
* Structured internal logging
* No silent failures
* Generic client error responses
* Fail-fast startup for missing critical configuration

---

## Testing approach

The project includes smoke tests rather than full test coverage.

Tests verify:

* Application startup behavior
* Endpoint availability
* Middleware wiring
* Environment configuration correctness

This matches the project scope and keeps maintenance overhead proportional.

---

## Running locally

Set required environment variables:

* `ConnectionStrings__DefaultConnection`
* Firebase credentials (`GOOGLE_APPLICATION_CREDENTIALS` or `FIREBASE_CREDENTIALS_PATH`)

Run:

```
dotnet run
```

Swagger UI is available in development mode.

---

## Deployment

The API is deployed to Microsoft Azure:

* Azure App Service (ASP.NET Core)
* Azure PostgreSQL (Flexible Server)

The deployment validates:

* Environment-based configuration
* Secrets handling
* Database connectivity
* Health monitoring via `/health`

---

## Project positioning

This is not a tutorial project. It represents:

* End-to-end backend ownership
* Practical API design
* Production environment awareness
* Real deployment validation

The scope is intentionally constrained, but the engineering decisions are made with real-world operation in mind.

---

## Expected evolution

If usage grows, likely next steps include:

* Stronger data constraints and indexing
* Separation of read/write paths if traffic increases
* More granular validation and error classification
* Moving migrations out of startup
* Pagination, filtering, and access control expansion

These are intentionally deferred until justified by real usage.