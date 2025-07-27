# IdempotentApi

## Description

This project provides a REST API for order management, with support for idempotency via a custom attribute. Currently, the API exposes endpoints for creating and retrieving orders. Idempotency logic is implemented using custom filters and attributes.

## Current status

- **Orders API**: Exposes endpoints for order management.
- **Idempotency**: Implemented via custom attribute and filter.
- **Domain**: Basic models and logic defined in the main project.
- **Architecture**: Monolithic structure, with partial separation between controllers, attributes, and domain.

## To be added

### 1. Clean Architecture

- **Project separation**:
  - **Domain**: Entities and domain logic.
  - **Application**: Use cases and application services.
  - **Infrastructure**: Repository implementations, data access, external services.
  - **Api**: Controllers and web configuration.
- **Logic separation**: Move business logic out of controllers, delegating to application services.

### 2. Persistence and caching

- **Entity Framework Core**: Implement repositories and data access using EF Core.
- **SQL Database**: Configure a relational database (e.g., SQL Server or PostgreSQL) for persisting orders and idempotency keys.
- **Redis**: Integrate Redis for efficient idempotency key management and response caching.

### 3. Future improvements

- **Automated tests**: Add unit and integration tests.
- **Swagger documentation**: Improve endpoint documentation.
- **Error handling**: Centralize exception and error message management.
- **Front End Client**: A client responsible sending request and generating an idempotent-key for each one.


