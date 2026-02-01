**Talabat E-Commerce Web API**
A robust E-commerce Web API built with .NET 8, implementing Onion Architecture and modern design patterns to ensure scalability and maintainability.

🏗 Architecture & Design Patterns
The solution is structured into four layers to follow the Separation of Concerns principle:

Talabat.APIs: The entry point handling HTTP requests, DTOs, and Swagger documentation.

Talabat.Core: The heart of the system containing Domain Entities, Interfaces, and Specifications.

Talabat.Repository: Data access layer implementing EF Core, Generic Repository, and Unit of Work.

Talabat.Service: Business logic layer handling Stripe Payments and JWT Authentication.

🚀 Key Features
Specification Pattern: For clean and reusable query logic (filtering, sorting, and pagination).

Caching: High-performance data retrieval using Redis.

Payments: Secure checkout flow integrated with Stripe SDK.

Security: Identity management and API protection via JWT Tokens.

Database: Automated schema migrations and data seeding on application startup.

🛠 Tech Stack
Framework: ASP.NET Core 8

ORM: Entity Framework Core

Database: SQL Server & Redis

Tools: AutoMapper, Swagger UI, Stripe API

▶️ Getting Started
Clone the repository.

Update Connection Strings and Stripe Keys in appsettings.json.

Run the project; the database will auto-migrate and seed initial data.


ؤGJC
ؤ
