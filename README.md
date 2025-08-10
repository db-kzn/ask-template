# ASK — Aspire Starter Kit

Base template for startups using .NET 9, Aspire, Blazor WebAssembly, Clean Architecture, and Multitenancy.

## 🏗️ Project Structure

```
src/
├── Aspire/
│   ├── ASK.AppHost/           # Aspire orchestration
│   └── ASK.ServiceDefaults/   # Shared service defaults
├── Server/
│   ├── ASK.Api/               # API + Blazor WASM host
│   ├── ASK.Application/       # Shared application logic
│   ├── ASK.Domain/            # Core domain models
│   └── ASK.Infrastructure/    # Shared infrastructure
├── Modules/                   # Optional feature modules (VSA)
│   └── (e.g., Catalog, Todo)
├── Clients/
│   └── ASK.BlazorWebApp/      # Blazor WebAssembly client
└── Shared/
    └── ASK.Shared/            # Shared DTOs, enums, constants
```

## ✅ Features

- ✅ Blazor WebAssembly + MudBlazor
- ✅ .NET Aspire 9.4.0 orchestration
- ✅ JWT + Refresh Token with `tenant_id` claim
- ✅ Multitenancy support
- ✅ RBAC & ABAC authorization
- ✅ Audit Trail logging
- ✅ Vertical Slice Architecture (VSA) for modules
- ✅ OpenAPI (Swagger) integration
- ✅ i18n (en, ru)
- ✅ BackgroundService for async tasks (email, audit)
- ✅ PostgreSQL + Redis via Aspire
- ✅ Seed data: roles, users, tenants
- ✅ Modular design (ready for microservices)

## 🚀 Getting Started

1. Clone the repo
2. Run `dotnet run --project src/Aspire/ASK.AppHost`
3. Open `https://localhost:XXXX/swagger`
