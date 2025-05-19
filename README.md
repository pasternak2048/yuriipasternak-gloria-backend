# GLORIA 🏡 — Go. Live. Own. Rent. Inform. Act.

**GLORIA** is a modular real estate management platform built with **.NET 8**, designed with principles of clean architecture, scalability, and service separation.

This repository contains the **backend system** of GLORIA — a collection of microservices communicating via REST and RabbitMQ, managed behind a central YARP API Gateway.

It includes:
- Identity and session management (JWT)
- Real estate catalog and offer publishing
- Document metadata and file storage
- Event-driven notifications and subscriptions

> GLORIA is designed to scale.


## 🚀 Features

- 🔐 **Authentication & Authorization**
  - Access & Refresh JWT tokens
  - Session tracking with IP & device info
  - Multi-session logout and token invalidation
- 🏢 **Real Estate Management**
  - Catalog of properties (CRUD)
  - Publishable offers with filters and enums
- 📂 **Document Handling**
  - Upload and store files with metadata
  - Local or cloud storage ready
- 🔔 **Event-Driven Architecture**
  - RabbitMQ-powered pub/sub between services
  - Subscriptions + notifications when offers are created
- 🌐 **API Gateway**
  - YARP-powered HTTP routing to all services
  - CORS, HTTPS
- ⚙️ **Generic Infrastructure**
  - Generic CRUD controllers and repositories
  - Auto-caching with invalidation
  - MongoDB seeding, DI extensions, distributed cache
- 🧪 **Ready for NuGet**
  - Clean `Contracts` and `BuildingBlocks` projects
  - No frontend dependencies



## 📦 Project Structure

```text
src/
├── ApiGateways/
│   └── YarpApiGatewayDesktop/
│
├── Services/
│   ├── Advert/
│   ├── Catalog/
│   ├── DocumentMetadata/
│   ├── DocumentStorage/
│   ├── IdentityProvider/
│   ├── Notification/
│   └── Subscription/
│
├── Contracts/               # Shared DTOs, Filters, Enums, Events
├── BuildingBlocks/          # Infrastructure: DI, Mongo, Redis, Middleware
├── docker-compose.yml       # Local development setup
└── GLORIA.Backend.sln       # Unified solution
```
Each service is self-contained, fully testable, and independently deployable.
Shared code is cleanly separated into reusable packages (Contracts, BuildingBlocks) — ready to be published as NuGet packages.



## 🧱 Architecture & Tech Stack

GLORIA follows a layered architecture with clean separation between concerns and responsibilities. It is built around a **microservices-first** approach with strong internal consistency and clear external contracts.

### 🧠 Architecture Principles

- **Clean Architecture**
  - Contracts, infrastructure, and logic are strictly isolated
- **Microservices**
  - Each domain has its own API, database, and message integration
- **Event-Driven Communication**
  - Services publish/subscribe via RabbitMQ
- **Generic Infrastructure**
  - Reusable patterns for CRUD, caching, Mongo, DI, exception handling

### 🔧 Technologies Used

| Layer              | Stack                                            |
|-------------------|--------------------------------------------------|
| API & Logic        | ASP.NET Core 8, C#, Minimal APIs                |
| Persistence        | MongoDB, Redis                                  |
| Messaging          | RabbitMQ, Pub/Sub with typed Events             |
| API Gateway        | YARP (Microsoft Reverse Proxy)                  |
| Infrastructure     | Docker, docker-compose                          |
| Shared Libraries   | Contracts (DTOs, Enums, Events) + BuildingBlocks (infra) |
| Security           | JWT (Access + Refresh), SHA512 signature validation |



## ▶️ How to Run (Local Development)

> The entire backend stack can be launched with a single command using Docker.

### 🐳 Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker + Docker Compose](https://www.docker.com/products/docker-desktop)

### 🚀 Run Everything

```bash
docker compose up --build
```

### 📍 Services started locally

| Service              | URL                                |
|----------------------|------------------------------------|
| API Gateway          | https://localhost:6061             |
| Identity Provider    | https://localhost:6062             |
| Catalog              | https://localhost:6063             |
| Advert               | https://localhost:6064             |
| Document Metadata    | https://localhost:6069             |
| Document Storage     | https://localhost:6070             |
| Notification         | https://localhost:6075             |
| Subscription         | https://localhost:6076             |
| RabbitMQ Management  | http://localhost:15672 (guest/guest) |

> All API traffic is routed through the **YARP API Gateway** at [https://localhost:6061](https://localhost:6061).



## 💡 Why this project exists

I built GLORIA as a way to challenge myself and grow as a backend engineer.

I had never worked with microservices before — and most of my past pet projects felt too simplistic, too improvised.  
This time, I decided to treat my learning like a real-world system: with clean architecture, infrastructure, composition, and scale in mind.

This project helps me:
- stay in shape technically
- learn enterprise-level practices
- and explore real challenges — caching, message buses, auth, clean separation, deployment, and reuse.

GLORIA is not a toy. It’s my personal journey toward mastery.
If you’re a developer or employer looking to understand how I think, code and grow — this project is me.

## 📄 License

This project is licensed under the [MIT License](LICENSE).
