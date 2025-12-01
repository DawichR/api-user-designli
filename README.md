# User Management App - Designli Technical Challenge

A complete .NET 8 Web API with Razor Pages frontend for user authentication and management using JWT tokens.

## 📋 Table of Contents
- [Overview](#overview)
- [Requirements Compliance](#requirements-compliance)
- [Features](#features)
- [Architecture](#architecture)
- [Technologies](#technologies)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [API Endpoints](#api-endpoints)
- [Testing](#testing)
- [Configuration](#configuration)
- [What Could Be Improved](#what-could-be-improved)

---

## 🎯 Overview

This project is a technical challenge solution that implements a user management system with JWT authentication. It features a clean architecture approach with CQRS pattern, in-memory data storage, and a modern UI with Razor Pages.

## ✅ Requirements Compliance

### Challenge Requirements
All requirements have been successfully implemented:

1. **✓ .NET 8 Project** - Built using .NET 8 SDK
2. **✓ Userapp Class** - Simple DTO class with **only** username and password fields as required
3. **✓ In-Memory Data** - 3+ test users seeded on application startup
4. **✓ Login Endpoint** - POST endpoint that validates credentials and returns JWT token
5. **✓ Protected GET Endpoint** - Returns list of users, protected with JWT authentication
6. **✓ Login Razor View** - Modern, responsive login page with Designli branding
7. **✓ Users List Page** - Protected page displaying users, only accessible with valid JWT

### Architecture Compliance
The project strictly follows the challenge requirements with a clean separation:
- **`Userapp` (DTO)** - Simple class with only `username` and `password` for login requests
- **`User` (Entity)** - Complete user entity with all fields (Id, Username, Password, Email, Name, LastName) for data storage
- This separation ensures compliance with requirement #2 while maintaining clean architecture principles

### Additional Implemented Features
- **Clean Architecture** - Separation of concerns with distinct layers
- **CQRS Pattern** - Commands and Queries using MediatR
- **Dependency Injection** - Following .NET best practices
- **JWT Token Provider** - Custom implementation with configurable claims
- **Comprehensive Unit Tests** - Controllers and Infrastructure tested
- **Modern UI** - Responsive design with Designli brand colors and assets
- **Docker Support** - Containerization with docker-compose
- **Swagger Documentation** - Interactive API documentation
- **Exception Handling** - Global middleware for error management

---

## 🚀 Features

### Backend
- **JWT Authentication** - Secure token-based authentication
- **In-Memory Database** - Fast, simple data storage for users
- **Clean Architecture** - Domain, Application, Infrastructure, and API layers
- **CQRS Pattern** - Separated read and write operations
- **MediatR Pipeline** - Centralized request handling
- **Global Exception Handling** - Consistent error responses
- **Swagger/OpenAPI** - Interactive API documentation

### Frontend
- **Razor Pages** - Server-side rendered pages
- **Modern UI** - Responsive design with gradient backgrounds
- **Designli Branding** - Company logo, colors (#f87565 orange, #fff white)
- **JWT Decoding** - Client-side token parsing for user info
- **Protected Routes** - Automatic redirect if not authenticated
- **External Assets** - Separated CSS and JavaScript files

---

## 🏗️ Architecture

The project follows **Clean Architecture** principles:

```
┌─────────────────────────────────────┐
│         UmaDesignli.Api             │  ← Controllers, Middleware, Razor Pages
├─────────────────────────────────────┤
│     UmaDesignli.Application         │  ← Commands, Queries, Handlers, Interfaces
├─────────────────────────────────────┤
│     UmaDesignli.Infrastructure      │  ← Token Provider, Repositories, Persistence
├─────────────────────────────────────┤
│       UmaDesignli.Domain            │  ← Entities, Business Logic
├─────────────────────────────────────┤
│      UmaDesignli.UnitTest           │  ← Unit Tests
└─────────────────────────────────────┘
```

### Userapp (DTO) vs User (Entity) Separation

To strictly comply with challenge requirement #2: *"Create a simple class named 'userapp' with username and password as fields"*, the project implements a clean separation:

**`Userapp` (Data Transfer Object)**
- **Purpose:** Login request DTO
- **Fields:** Only `username` and `password` (as required)
- **Usage:** Receives credentials from the login endpoint
- **Location:** `UmaDesignli.Domain/Entities/Userapp.cs`

```csharp
public sealed class Userapp
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
```

**`User` (Entity)**
- **Purpose:** Complete user entity for data storage
- **Fields:** Id, Username, Password, Email, Name, LastName
- **Usage:** Internal repository storage and JWT token generation
- **Location:** `UmaDesignli.Domain/Entities/User.cs`

```csharp
public sealed class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
}
```

**Flow:**
1. Client sends `Userapp` (username + password) to `/api/access/user/login`
2. Controller converts to `LoginCommand`
3. Handler queries `User` repository
4. TokenProvider generates JWT from `User` entity
5. Response returns token to client

This separation ensures:
- ✅ Exact compliance with challenge requirement #2
- ✅ Secure password handling (not exposed in responses)
- ✅ Clean architecture principles (DTO vs Entity separation)
- ✅ Single Responsibility Principle

### Design Patterns Used
- **CQRS** - Command Query Responsibility Segregation
- **Repository Pattern** - Data access abstraction
- **Dependency Injection** - IoC container for loose coupling
- **Mediator Pattern** - Decoupled request handling with MediatR
- **Options Pattern** - Strongly-typed configuration

---

## 💻 Technologies

- **.NET 8** - Latest LTS framework
- **ASP.NET Core** - Web API and Razor Pages
- **MediatR** - CQRS implementation
- **JWT (JSON Web Tokens)** - Authentication
- **xUnit** - Unit testing framework
- **Moq** - Mocking framework for tests
- **Swagger/OpenAPI** - API documentation
- **Docker** - Containerization

---

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Docker (optional)
- Git

### Running Locally

1. **Clone the repository**
```bash
git clone https://github.com/DawichR/api-user-designli.git
cd api-user-designli
```

2. **Configure JWT Settings**
Edit `UmaDesignli.Api/appsettings.json`:
```json
{
  "Jwt": {
    "Secret": "your-secret-key-at-least-32-characters-long",
    "Issuer": "UmaDesignliApi",
    "Audience": "UmaDesignliClient",
    "ExpirationInMinutes": 60
  }
}
```

3. **Run the application**
```bash
dotnet run --project UmaDesignli.Api
```

4. **Access the application**
- Login Page: `https://localhost:5003/login`
- Swagger UI: `https://localhost:5003/swagger`

### Running with Docker

The Docker build process includes:
1. ✅ Dependency restoration
2. ✅ **Automatic unit test execution** (build fails if tests fail)
3. ✅ Application compilation
4. ✅ Container creation

**Build and run:**
```bash
# Build and start (tests run automatically during build)
docker-compose up --build

# View logs
docker-compose logs -f

# Stop
docker-compose down
```

**What happens during Docker build:**
- All unit tests are executed automatically
- If any test fails, the build stops
- Build output shows detailed test results
- Container only starts if all tests pass

**Access the application:**
- Login Page: `http://localhost:5000/login`
- Swagger UI: `http://localhost:5000/swagger`
- Health Check: `http://localhost:5000/health`

### Test Users

Three users are seeded automatically on startup:

| Username     | Password      | Name            | Email                       |
|-------------|---------------|-----------------|-----------------------------||
| jperez      | password@123  | Juan Pérez      | juan.perez@designli.co      |
| mgarcia     | password@123  | María García    | maria.garcia@designli.co    |
| crodriguez  | password@123  | Carlos Rodríguez| carlos.rodriguez@designli.co|

---

## 📁 Project Structure

```
api-user-designli/
├── UmaDesignli.Api/                # Web API & Razor Pages
│   ├── Controllers/
│   │   ├── Access/                 # Authentication endpoints
│   │   └── Users/                  # User management endpoints
│   ├── Pages/                      # Razor Pages
│   │   ├── Login.cshtml            # Login page
│   │   └── Users.cshtml            # Users list page
│   ├── wwwroot/
│   │   ├── css/                    # Stylesheets
│   │   │   ├── login.css
│   │   │   └── users.css
│   │   └── js/                     # JavaScript files
│   │       ├── login.js
│   │       └── users.js
│   ├── Middleware/                 # Custom middleware
│   └── Extensions.cs               # Service configuration
│
├── UmaDesignli.Application/        # Business logic
│   ├── Commands/                   # CQRS Commands
│   │   └── Access/
│   │       ├── LoginCommand.cs
│   │       └── LoginCommandHandler.cs
│   ├── Queries/                    # CQRS Queries
│   │   └── Access/
│   │       ├── GetAllUsersQuery.cs
│   │       └── GetAllUsersQueryHandler.cs
│   ├── Interfaces/                 # Abstractions
│   │   ├── ITokenProvider.cs
│   │   └── Repositories/
│   └── Behaviors/                  # MediatR behaviors
│
├── UmaDesignli.Infrastructure/     # External concerns
│   ├── Token/
│   │   └── TokenProvider.cs        # JWT token generation
│   └── Persistence/
│       ├── Repositories/           # Data access
│       └── Seeds/                  # Data seeding
│
├── UmaDesignli.Domain/             # Core domain
│   └── Entities/
│       ├── User.cs                 # User entity (complete user data)
│       └── Userapp.cs              # Userapp DTO (login: username + password only)
│
└── UmaDesignli.UnitTest/           # Tests
    ├── Controllers/                # Controller tests
    │   ├── AccessControllerTests.cs
    │   └── UsersControllerTests.cs
    └── Infrastructure/             # Infrastructure tests
        └── TokenProviderTests.cs
```

---

## 🔌 API Endpoints

### Authentication

#### Login
```http
POST /api/access/user/login
Content-Type: application/json

{
  "username": "jperez",
  "password": "password@123"
}
```
**Note:** The request body uses the `Userapp` DTO class (only username and password fields as per challenge requirements).

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "jperez"
}
```

### Users (Protected)

#### Get All Users
```http
GET /api/users
Authorization: Bearer {token}
```

**Response:**
```json
[
  {
    "id": 1,
    "username": "jperez",
    "email": "juan.perez@example.com",
    "name": "Juan",
    "lastName": "Pérez"
  },
  ...
]
```

Also available at: `GET /api/access/users` (alternative endpoint)

---

## 🧪 Testing

### Run Unit Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true

# Run specific test class
dotnet test --filter "FullyQualifiedName~AccessControllerTests"
```

### Test Coverage

The project includes comprehensive unit tests for:

1. **AccessController** (3 test cases)
   - Valid login returns token
   - Mediator is called correctly
   - Different credentials are handled

2. **UsersController** (4 test cases)
   - Returns list of users
   - Calls mediator correctly
   - Handles empty list
   - Returns correct data structure

3. **TokenProvider** (7 test cases)
   - Generates valid JWT
   - Contains correct claims
   - Has proper issuer/audience
   - Has expiration time
   - Different users get different tokens
   - Full name formatting

### Test Files
- `UmaDesignli.UnitTest/Controllers/AccessControllerTests.cs`
- `UmaDesignli.UnitTest/Controllers/UsersControllerTests.cs`
- `UmaDesignli.UnitTest/Infrastructure/TokenProviderTests.cs`

---

## ⚙️ Configuration

### JWT Configuration (`appsettings.json`)

```json
{
  "Jwt": {
    "Secret": "minimum-32-characters-secret-key",
    "Issuer": "UmaDesignliApi",
    "Audience": "UmaDesignliClient",
    "ExpirationInMinutes": 60
  }
}
```

### Port Configuration (`launchSettings.json`)

- **HTTPS**: `https://localhost:5003`
- **HTTP**: `http://localhost:5002`
- **Docker**: `http://localhost:5000`

---

## 🔧 What Could Be Improved

### Security Enhancements
1. **Password Hashing** - Currently passwords are stored in plain text
   - Implement password hashing
   - Add salt for additional security

2. **Refresh Tokens** - Add refresh token functionality
   - Extend user sessions securely
   - Implement token revocation

3. **Rate Limiting** - Prevent brute force attacks
   - Add rate limiting middleware
   - Implement account lockout after failed attempts

### Data Persistence
4. **Real Database** - Replace in-memory storage
   - Use SQL Server, PostgreSQL, or MongoDB
   - Add Entity Framework Core migrations
   - Implement data validation

5. **Data Validation** - Enhanced validation
   - FluentValidation for all commands
   - Model validation attributes
   - Custom validation rules

### Architecture & Code Quality
6. **Exception Handling** - More granular error handling
   - Custom exception types
   - Detailed error responses
   - Logging with Serilog

7. **Integration Tests** - Add integration tests
   - Test complete request/response cycle
   - Test database interactions
   - Test authentication flow

8. **API Versioning** - Support multiple API versions
   - Use Microsoft.AspNetCore.Mvc.Versioning
   - Maintain backward compatibility

### Features
9. **User Management** - CRUD operations
   - Create new users
   - Update user information
   - Delete users
   - Role-based access control (RBAC)

10. **Email Verification** - User registration flow
    - Email confirmation
    - Password reset
    - Two-factor authentication (2FA)

### DevOps & Monitoring
11. **Health Checks** - Comprehensive health monitoring
    - Database health
    - External service checks
    - Custom health checks

12. **Logging & Monitoring** - Production-ready logging
    - Structured logging with Serilog
    - Application Insights integration
    - Performance monitoring

13. **CI/CD Pipeline** - Automated deployment
    - GitHub Actions or Azure DevOps
    - Automated testing
    - Container registry publishing

### UI/UX Improvements
14. **Remember Me** - Persistent login
    - Local storage for longer sessions
    - Cookie-based authentication option

15. **Loading States** - Better user feedback
    - Loading spinners
    - Progress indicators
    - Toast notifications for actions

16. **Accessibility** - WCAG compliance
    - ARIA labels
    - Keyboard navigation
    - Screen reader support

---

## 👥 Test Users Reference

```csharp
// Seeded users in UserSeeding.cs
Username: jperez      | Password: password@123 | Name: Juan Pérez
Username: mgarcia     | Password: password@123 | Name: María García
Username: crodriguez  | Password: password@123 | Name: Carlos Rodríguez
```

---

## 📄 License

This project is part of a technical challenge for Designli.

---

## 🤝 Author and Contributors

Dawich Rodriguez.

---
