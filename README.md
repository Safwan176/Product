# Product API

ASP.NET Core Web API with JWT authentication, BCrypt password hashing, and product management.

## Tech Stack

- .NET 10
- Entity Framework Core (SQL Server)
- BCrypt.Net-Next
- JWT Bearer Authentication
- Swagger / OpenAPI

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (or update `Program.cs` to use SQLite)

### Configuration

Update `appsettings.json` with your connection string and JWT settings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ProductDb;Trusted_Connection=True;"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key-here",
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "ExpiryMinutes": 60
  }
}
```

### Run

```bash
dotnet run --project Product
```

API runs at `http://localhost:5135`. Swagger UI available at `/swagger`.

## Endpoints

### Auth

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register a new user (password is BCrypt hashed) |
| POST | `/api/auth/login` | Login and receive a JWT token |

#### Register body
```json
{
  "userID": "john",
  "email": "john@example.com",
  "password": "secret",
  "name": "John Doe"
}
```

#### Login body
```json
{
  "userID": "john",
  "password": "secret"
}
```

### Products

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create a product |
| PUT | `/api/products/{id}` | Update a product |
| DELETE | `/api/products/{id}` | Delete a product |
