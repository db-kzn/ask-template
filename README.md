# ASK — Aspire Starter Kit

**Base template for startups using .NET 9, Aspire, Blazor WebAssembly, Clean Architecture, and Multitenancy.**

> 🔥 **Production-ready, modular, testable, and scalable**

---

## 🏗️ Project Structure

```
src/
├── Aspire/
│   ├── ASK.AppHost/                     # Aspire orchestration
│   └── ASK.ServiceDefaults/             # Shared service defaults (logging, OpenTelemetry)
├── Server/
│   ├── ASK.Server.Api/                  # API + Blazor WASM host (Minimal APIs, OpenAPI)
│   ├── ASK.Server.Application/          # Shared application logic (CQRS, Mediator, VSA)
│   ├── ASK.Server.Domain/               # Core domain models (Entities, Value Objects, Interfaces)
│   ├── ASK.Server.Infrastructure/       # Shared infrastructure (EF Core, Identity, Multitenancy)
│   ├── Migrations/                      # Database migrations
│   │   ├── ASK.Server.Migrations.PostgreSQL/  # PostgreSQL-specific migrations
│   │   └── ASK.Server.Migrations.SQLite/      # SQLite-specific migrations
│   └── Modules/                         # Optional feature modules (VSA)
│       ├── ASK.Server.Modules.Catalog/  # Example: Product management
│       └── ASK.Server.Modules.Todo/     # Example: Task management
├── Clients/
│   └── ASK.Clients.BlazorWebApp/        # Blazor WebAssembly client (MudBlazor, i18n)
└── Shared/
    └── ASK.Shared/                      # Shared DTOs, constants, Result<T>, Pagination

tests/
├── ASK.Tests.Shared/                    # Shared test fixtures, mocks, helpers
├── ASK.Tests.Server.Application/        # Unit/Integration tests for Application layer
├── ASK.Tests.Server.Infrastructure/     # Integration tests for DB, Identity, Seed
├── ASK.Tests.Server.Api/                # API integration tests (auth, endpoints)
└── ASK.Tests.Clients.BlazorWebApp/      # Blazor component tests (bUnit)
```

---

## ✅ Features

### 🌐 Core
- ✅ .NET 9 + Aspire 9.4.0 orchestration
- ✅ Blazor WebAssembly + MudBlazor
- ✅ Clean Architecture + Vertical Slice Architecture (VSA)
- ✅ Minimal APIs + OpenAPI (Swagger)
- ✅ Multitenancy with schema isolation (via `Finbuckle.MultiTenant`)
- ✅ JWT + Refresh Token with `tenant_id` claim
- ✅ RBAC & ABAC authorization (roles, policies, permissions)
- ✅ BackgroundService for async tasks (email, cleanup)

### 🔐 Security
- ✅ ASP.NET Core Identity with `ApplicationUser`
- ✅ RefreshToken storage with revocation
- ✅ Tenant isolation via PostgreSQL schema
- ✅ Claims-based auth: `tenant_id`, `permissions`

### 🧱 Domain & Data
- ✅ `Entity`, `AuditableEntity`, `ValueObject` from Clean Architecture
- ✅ EF Core 9 + PostgreSQL + SQLite
- ✅ Redis via Aspire (caching)
- ✅ Manual mapping (no AutoMapper — MIT-safe)

### 🧪 Testing
- ✅ TDD / Test-Along Development approach
- ✅ Unit & Integration tests for all layers
- ✅ `xUnit`, `Moq`, `FluentAssertions`, `bUnit`
- ✅ `WebApplicationFactory<T>` for API tests
- ✅ `Testcontainers.PostgreSql` for real DB tests

### 🌍 i18n & UX
- ✅ i18n (en, ru) via `IStringLocalizer`
- ✅ User preferences (theme, language)
- ✅ Responsive Blazor UI

### 🚀 DevOps
- ✅ Aspire Dashboard
- ✅ Health Checks
- ✅ OpenTelemetry
- ✅ Ready for Docker/Kubernetes

---

## 🚀 Getting Started

1. Clone the repo
2. Run PostgreSQL (Docker):
   ```bash
   docker run -d --name postgres-ask -e POSTGRES_PASSWORD=postgres -e POSTGRES_USER=postgres -e POSTGRES_DB=ask -p 5432:5432 postgres:16
   ```
3. Run migrations:
   ```bash
   dotnet ef database update --project src/Server/Migrations/ASK.Server.Migrations.PostgreSQL --startup-project src/Aspire/ASK.AppHost
   ```
4. Start the app:
   ```bash
   dotnet run --project src/Aspire/ASK.AppHost
   ```
5. Open Swagger: `https://localhost:XXXX/swagger`

---

## 🧩 Sample Endpoints

### Auth
- `POST /api/auth/login` — returns JWT + RefreshToken
- `POST /api/auth/refresh` — renew tokens

### Sample Request
```json
{
  "email": "admin@ask.local",
  "password": "P@ssw0rd!",
  "tenantId": "main",
  "ipAddress": "127.0.0.1"
}
```

---

## 📦 License

MIT — free for startups, individuals, and non-profits (< $5M revenue).
