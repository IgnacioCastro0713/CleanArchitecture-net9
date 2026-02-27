# Clean Architecture Template (.NET 9)

A modern, production-ready solution template built with .NET 9, following the principles of Clean Architecture and Domain-Driven Design (DDD). This template is designed to provide a solid foundation for building scalable, maintainable, and robust enterprise applications.

## 🌟 Features

- **.NET 9 & Minimal APIs:** Leveraging the latest performance improvements and simplicity of modern .NET.
- **Clean Architecture & DDD:** Strict separation of concerns (Domain, Application, Infrastructure, Presentation) and rich domain models.
- **CQRS Pattern:** Command Query Responsibility Segregation with custom abstractions and decorators for cross-cutting concerns.
- **Result Pattern:** Avoids throwing exceptions for control flow by using a robust `Result` pattern for success and failure handling.
- **Cross-Cutting Concerns via Decorators:**
  - **Validation:** Automatic validation using `FluentValidation` before command execution.
  - **Logging:** Automatic structured logging for queries and commands using `Serilog`.
- **Domain Events:** Built-in mechanism to raise and dispatch domain events using Entity Framework Core interceptors.
- **Entity Framework Core:** SQL Server integration, complete with configurations, strongly typed IDs, and migrations.
- **Authentication & Authorization:** JWT token generation and a custom permission-based authorization system.
- **.NET Aspire Integration:** Seamless local orchestration and distributed tracing using Aspire (`AppHost` & `ServiceDefaults`) with OpenTelemetry.
- **Docker Ready:** Dockerfile and `docker-compose` included for easy containerization.
- **Architecture Tests:** Automated tests using `NetArchTest.Rules` to enforce architectural boundaries and prevent dependency regressions.

## 🏗️ Project Structure

The solution is divided into several projects, enforcing Clean Architecture constraints:

*   **`Domain`**: Contains enterprise-wide logic and types (Entities, Enums, Domain Errors, Domain Events). It has no external dependencies.
*   **`SharedKernel`**: Common DDD abstractions shared across the system (`Entity`, `AuditableEntity`, `Error`, `Result`, `IDomainEvent`).
*   **`Application`**: Business use cases. Contains interfaces (`ICommand`, `IQuery`), CQRS handlers, Validators, and abstractions for external services (e.g., `IApplicationDbContext`, `ITokenProvider`).
*   **`Infrastructure`**: Implementation of external concerns. Contains the EF Core `DbContext`, Database Interceptors, JWT Authentication, Permission Authorization, and Domain Event dispatchers.
*   **`Web.Api`**: The presentation layer. Registers dependency injection, configures Minimal API endpoints, Global Exception Handling, and Swagger UI.
*   **`Aspire.AppHost` & `Aspire.ServiceDefaults`**: .NET Aspire projects for cloud-native orchestration, managing dependencies like Azure SQL Server and setting up OpenTelemetry.
*   **`ArchitectureTests`**: Test suite ensuring that dependency rules between the layers are strictly followed.

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (or a Docker runtime to run Azure SQL Edge via Aspire)
- Docker Desktop / Podman (Optional, required for Aspire container orchestration)

### Run Locally (Standard)

1. Set `Web.Api` as the startup project.
2. Update the `Database` connection string in `appsettings.Development.json` inside the `Web.Api` project.
3. Run the EF Core migrations to create the database:
   ```bash
   dotnet ef database update --project src/Infrastructure --startup-project src/Web.Api
   ```
4. Run the project:
   ```bash
   dotnet run --project src/Web.Api
   ```

### Run Locally (with .NET Aspire)

This template includes **.NET Aspire** for streamlined local development, orchestration, and monitoring.

1. Set `Aspire.AppHost` as your startup project.
2. Run the application via Visual Studio, Rider, or CLI:
   ```bash
   dotnet run --project src/Aspire.AppHost
   ```
   *Aspire will automatically provision an Azure SQL Edge container, inject the connection string into the `Web.Api` project, and start the Aspire Dashboard for telemetry (Logs, Traces, Metrics).*

## 🧪 Testing

The solution includes an `ArchitectureTests` project to maintain architectural integrity. Run the tests via:

```bash
dotnet test
```

## 📜 Logging & Observability

- **Serilog:** Configured to push structured logs to the console and external providers. Can be easily configured to send logs to a Seq instance.
- **OpenTelemetry:** Pre-configured in `Aspire.ServiceDefaults` to capture metrics and distributed traces, viewable locally via the .NET Aspire Dashboard.


