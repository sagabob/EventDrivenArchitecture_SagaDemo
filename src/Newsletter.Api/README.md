# Newsletter.Api

## Overview
A small .NET 10 Web API using FastEndpoints to handle newsletter subscriptions and MassTransit to publish messaging events. It includes EF Core for persistence and Swagger/OpenAPI for interactive documentation.

## Requirements
- .NET 10 SDK
- A PostgreSQL-compatible database (connection string in `appsettings.*.json`)
- A RabbitMQ broker (AMQP/AMQPS) for MassTransit
- Optional: Visual Studio 2026 for development and debugging

## Configuration
Configuration is stored in `appsettings.Development.json` (development only); the relevant keys:
- `ConnectionStrings:Database` — EF Core connection string (Postgres)
- `ConnectionStrings:RabbitMq` — RabbitMQ URI (e.g. `amqps://user:pass@host:5671/vhost`)

Make sure the connection strings are correct and secrets are handled securely in production.

## Database & Migrations
EF Core DbContext is `NewsletterDbContext`. To apply migrations:
- From the command line: `dotnet ef database update --project src/Newsletter.Api`
- Or from Visual Studio __Package Manager Console__: `Update-Database -Project Newsletter.Api`

## MassTransit (RabbitMQ)
MassTransit reads the RabbitMQ URI from `ConnectionStrings:RabbitMq`. The project supports both `amqp://` and `amqps://` schemes. If using `amqps`, ensure any required TLS certificates are trusted on the host running the service.

## Running the API
- Start the API from Visual Studio (__F5__ to debug or __Ctrl+F5__ to run without debugging).
- The development launch profiles use:
  - HTTP: `http://localhost:5264`
  - HTTPS: `https://localhost:7027`
- Swagger UI is available at the application root when running in Development (e.g. `https://localhost:7027/`).

## Endpoints
- POST `/newsletter/subscribe` — subscribe with a JSON body `{ "Email": "you@example.com" }`. Returns a tracking id.
- GET `/newsletter/info` — returns basic newsletter information.

