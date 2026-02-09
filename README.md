# FoodStore Server - ASP.NET Core 8.0 Backend API

## 👨‍💻 Contributors

- Mohammad Hossein Jafari => Authentication, Authorization, External Service, User Services
- Parsa Maghsodian => Validation Behavior, Middleware, Error Message, Medaitor/CQRS, Global Exception Handler, Soft Delete, Logging, Serilog, Category Services   
- Arshia Mokhtari => Repository, Unit of Work, Food Services

---

## 📋 Project Overview

**FoodStore** is a comprehensive food ordering and e-commerce platform backend built with **ASP.NET Core 8.0** following **Clean Architecture** principles. The system provides secure user authentication, food catalog management, order processing, and role-based access control with JWT authentication and email verification.

### Key Purpose
This project demonstrates enterprise-level backend development with industry best practices, modern design patterns, and secure authentication mechanisms suitable for production environments.

---


## ✨ Core Features

### 🔐 Authentication & Authorization

- **User Registration** with secure password hashing
- **Email Verification** - Resend API integration for confirmation emails
- **JWT Bearer Token Authentication** - Secure token-based access with 10-minute expiration
- **Refresh Token Mechanism** - Extended session support without re-authentication
- **Role-Based Access Control (RBAC)** - Admin, User, Vendor roles
- **Automatic Role Initialization** - Roles created at application startup
- **Secure Password Requirements**
  - Minimum 8 characters
  - At least one uppercase letter
  - At least one lowercase letter
  - At least one number
  - At least one special character

### 👤 User Management

- User registration with profile information (FirstName, LastName, PhoneNumber)
- Email validation with custom regex patterns
- User profile management
- Login with JWT token generation
- Token refresh functionality
- User account deletion
- Email address verification workflows
- Role assignment and management

### 🍕 Food/Product Management

- **Create Food Items** with:
  - Name (3-50 characters)
  - Description (max 300 characters)
  - Price (Money value object with currency support)
  - Category assignment
  - Food image (max 5MB)
  - Availability status
  - Featured flag
- **Retrieve Food Items**:
  - Find single food by ID
  - Get all foods with category information
- **Category Management**:
  - Food categorization system
  - Organized product catalog

### 📦 Order Management

- **Order Creation** - Create orders with multiple items
- **Order Tracking** - Complete status lifecycle:
  - Pending → Processing → Shipped → Delivered or Cancelled
- **Payment Status Management** - Track payment states:
  - Pending → Paid or Failed
- **Order Details**:
  - Customer information
  - Item listings with quantities
  - Total, discount, and final amounts
  - Delivery tracking timestamps
  - Shipping address management
  - Order notes/special instructions
- **Soft Delete Support** - Archived orders remain in database

### 📧 Email Services

- **Email Verification** - Resend API integration
- **Confirmation Emails** - Automated registration confirmation
- **Transactional Emails** - Order notifications (future)
- **Email Validation** - RFC-compliant email format checking

### 🔍 Data Integrity & Validation

- **FluentValidation** - Comprehensive request validation
- **Domain Validation** - Business rules enforced at entity level
- **Value Objects** - Encapsulated validation logic:
  - Email validation
  - Money calculations with currency support
  - Password strength requirements
  - Phone number formats
  - Quantity management
- **ErrorOr Pattern** - Type-safe error handling
- **Automatic Validation Pipeline** - MediatR pipeline behavior

### 📊 Soft Delete Implementation

- Non-destructive data deletion
- Automatic query filtering
- SoftDeleteInterceptor pattern
- Compliance with data retention requirements
- Maintains audit trails

---

## 🏗️ Architecture & Design Patterns

### Clean Architecture Layers

```
┌─────────────────────────────────────┐
│      Presentation Layer             │
│  (Controllers, Route Handlers)      │
├─────────────────────────────────────┤
│      Application Layer              │
│  (MediatR, Services, Behaviors)     │
├─────────────────────────────────────┤
│      Domain Layer                   │
│  (Entities, Value Objects, Rules)   │
├─────────────────────────────────────┤
│      Infrastructure Layer           │
│  (EF Core, Repositories, Database)  │
└─────────────────────────────────────┘
```

### Design Patterns Implemented

1. **CQRS (Command Query Responsibility Segregation)**
   - MediatR for command/query separation
   - Request handling through handlers
   - Pipeline behaviors for cross-cutting concerns

2. **Repository Pattern**
   - Generic repository for data access
   - Decouples business logic from data access
   - Repositories: CategoryRepository, OrderRepository, ProductRepository, UserRepository

3. **Unit of Work Pattern**
   - Coordinates multiple repositories
   - Ensures transaction consistency

4. **Value Objects Pattern**
   - Email, Money, Password, PhoneNumber, Quantity
   - Encapsulates validation and behavior
   - Immutable design

5. **Pipeline Behavior Pattern** (MediatR)
   - LoggingPipelineBehavior - Request/response logging
   - ValidationBehavior - Automatic validation
   - Extensible for cross-cutting concerns

6. **Dependency Injection**
   - Managed through Microsoft.Extensions.DependencyInjection
   - Service lifetime management (Scoped, Transient, Singleton)

7. **Error Handling Pattern**
   - ErrorOr<T> for Result types
   - Global exception handler middleware
   - Structured error responses

---

## 🛠️ Technologies & Dependencies

### Framework & Core
- **ASP.NET Core 8.0.22** - Modern web framework
- **.NET 8.0** - Runtime environment
- **C# 12** - Latest language features

### Data Access & ORM
- **Entity Framework Core 8.0.22** - ORM framework
- **SQL Server 2022** - Database engine
- **Migrations** - Database versioning

### Authentication & Security
- **ASP.NET Identity 8.0.22** - User management
- **JWT Bearer Authentication** - Token-based auth
- **System.IdentityModel.Tokens.Jwt 8.15.0** - JWT handling

### Business Logic
- **MediatR 14.0.0** - CQRS mediator
- **FluentValidation 12.1.1** - Request validation
- **ErrorOr 2.0.1** - Error handling
- **Mapster 7.4.0** - Object mapping

### External Services
- **Resend 0.2.1** - Email service API
- **DotNetEnv 3.1.1** - Environment variables

### Logging
- **Serilog 8.0.3** - Structured logging
- **Serilog.AspNetCore 8.0.3**
- **Serilog.Sinks.Console & File**
- **Serilog.Settings.Configuration**

### API & Documentation
- **Swashbuckle/Swagger 6.6.2** - API documentation
- **Microsoft.AspNetCore.OpenApi** - OpenAPI support

---

## 📂 Project Structure

```
FoodStore.Server/
├── Domain/                              # Business Logic Layer
│   ├── Entities/
│   │   ├── Category.cs
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── Product.cs
│   │   └── User.cs
│   ├── Enums/
│   │   ├── OrderStatus.cs
│   │   └── UserRole.cs
│   └── Valueobjects/
│       ├── Email.cs
│       ├── Money.cs
│       ├── Password.cs
│       ├── PhoneNumber.cs
│       ├── Quantity.cs
│       └── ValueObject.cs
│
├── Application/                         # Application Logic Layer
│   ├── Behaviors/
│   │   ├── LoggingPipelineBehavior.cs
│   │   └── ValidationBehavior.cs
│   ├── Common/
│   │   └── Interfaces/
│   │       ├── ICategoryRepository.cs
│   │       ├── IEmailService.cs
│   │       ├── IOrderRepository.cs
│   │       ├── IProductRepository.cs
│   │       ├── IRepository.cs
│   │       ├── IUnitOfWork.cs
│   │       └── IUserRepository.cs
│   ├── Foods/
│   │   ├── Commands/
│   │   │   └── CreateFood.cs
│   │   ├── Queries/
│   │   │   ├── FindFood.cs
│   │   │   └── GetAllFoods.cs
│   │   └── Errors/
│   │       └── FoodErrors.cs
│   ├── Users/
│   │   ├── Commands/
│   │   │   ├── AddRole.cs
│   │   │   ├── ConfirmEmail.cs
│   │   │   ├── DeleteUser.cs
│   │   │   ├── LoginUser.cs
│   │   │   ├── LoginUserWithRefreshToken.cs
│   │   │   ├── LogoutUser.cs
│   │   │   ├── RegisterUser.cs
│   │   │   └── RevokeRefreshToken.cs
│   │   ├── Queries/
│   │   │   ├── GetAllUsers.cs
│   │   │   ├── GetUserByEmail.cs
│   │   │   ├── GetUserById.cs
│   │   │   └── UpdateUser.cs
│   │   └── Errors/
│   │       └── UserErrors.cs
│   ├── Middlewares/
│   │   └── GlobalExceptionHandler.cs
│   └── Services/
│       ├── FoodService.cs
│       ├── IFoodService.cs
│       ├── IUserService.cs
│       ├── TokenProvider.cs
│       └── UserService.cs
│
├── Infrastructure/                      # Data Access Layer
│   ├── FoodStoreDbContext.cs
│   ├── DataModels/
│   │   ├── Customer.cs
│   │   ├── Food.cs
│   │   ├── FoodCategory.cs
│   │   ├── Order.cs
│   │   └── OrderItem.cs
│   ├── FluentApi/
│   │   ├── CustomerConfiguration.cs
│   │   ├── FoodCategoryConfiguration.cs
│   │   ├── FoodConfiguration.cs
│   │   ├── OrderConfiguration.cs
│   │   └── OrderItemConfiguration.cs
│   ├── Interceptor/
│   │   └── SoftDeleteInterceptor.cs
│   ├── Interfaces/
│   │   └── ISoftDeletable.cs
│   ├── Repositories/
│   │   ├── CategoryRepository.cs
│   │   ├── OrderRepository.cs
│   │   ├── ProductRepository.cs
│   │   ├── Repository.cs
│   │   └── UserRepository.cs
│   ├── ResendEmail/
│   │   └── ResendEmailService.cs
│   └── UnitOfWork/
│       └── UnitOfWork.cs
│
├── Identity/                            # Authentication Layer
│   ├── UserDbContext.cs
│   ├── DataModels/
│   │   ├── ApplicationUser.cs
│   │   └── RefreshToken.cs
│   └── FluentApi/
│       ├── ApplicationUserConfiguration.cs
│       └── RefreshTokenConfiguration.cs
│
├── Presentation/                        # API Layer
│   ├── Controllers/
│   │   ├── FoodController.cs
│   │   └── UserController.cs
│   └── Properties/
│       └── launchSettings.json
│
├── Shared/
│   └── JwtConfiguration.cs
│
├── Migrations/
│   └── [Database migration files]
│
├── Program.cs                           # Application entry point
├── appsettings.json                     # Configuration
├── FoodStore.Server.csproj              # Project file
└── FoodStore.Server.http                # API test file
```

---

## 🗄️ Database Schema

### Two Separate Databases

#### **FoodStoreDbContext** (Business Data)
```
Foods
├── Id (PK)
├── FoodCategoryId (FK)
├── Name
├── Description
├── Price (Money - Owned Type)
├── IsAvailable
└── FoodImage

FoodCategories
├── Id (PK)
├── Name
└── Foods (Navigation)

Customers
├── Id (PK)
├── ApplicationUserId (FK to AspNetUsers)
├── FirstName
├── LastName
├── Email (Owned Type)
├── PhoneNumber (Owned Type)
├── Address
└── Orders (Navigation)

Orders
├── Id (PK)
├── OrderNumber
├── CustomerId (FK)
├── TotalAmount
├── DiscountAmount
├── FinalAmount
├── Status (Enum)
├── PaymentStatus
├── PaymentMethod
├── ShippingAddress
├── Notes
├── CreatedAt
├── DeliveredAt
└── OrderItems (Navigation)

OrderItems
├── Id (PK)
├── OrderId (FK)
├── FoodId (FK)
├── Quantity (Owned Type)
└── Price (Money - Owned Type)
```

#### **UserDbContext** (Identity Data)
```
AspNetUsers
├── Id (PK)
├── UserName
├── Email
├── PasswordHash
├── FirstName
├── LastName
├── PhoneNumber
├── IsEmailConfirmed
├── IsActive
├── CreatedAt
└── RefreshTokens (Navigation)

RefreshTokens
├── Id (PK)
├── UserId (FK)
├── Token
├── ExpiryDate
├── IsRevoked
└── CreatedAt

AspNetRoles
├── Id (PK)
└── Name

AspNetUserRoles
├── UserId (FK)
└── RoleId (FK)
```

### Owned Types (Value Objects)
- **Email** - Validated email address
- **Money** - Amount + Currency
- **PhoneNumber** - Formatted phone number
- **Quantity** - Item quantity
- **Password** - Encrypted password

---

## 🔐 Authentication Flow

### Registration Flow
```
1. User submits registration request
   ↓
2. Email validation (Value Object)
   ↓
3. Password validation (min 8 chars, complex rules)
   ↓
4. User created in AspNetUsers table
   ↓
5. Confirmation email sent via Resend API
   ↓
6. Email token generated and stored
   ↓
7. User account awaits email verification
```

### Login Flow
```
1. User submits email + password
   ↓
2. FluentValidation validates format
   ↓
3. UserManager finds user by email
   ↓
4. Password verification against hash
   ↓
5. JWT Access Token generated (10 min expiration)
   ↓
6. Refresh Token generated (long-lived)
   ↓
7. Tokens returned to client
```

### Token Refresh Flow
```
1. Client sends expired JWT + Refresh Token
   ↓
2. Validate Refresh Token in database
   ↓
3. Check token hasn't been revoked
   ↓
4. Generate new JWT Access Token
   ↓
5. Return new token to client
```

### JWT Token Structure
```
Header:
{
  "alg": "HS256",
  "typ": "JWT"
}

Payload:
{
  "sub": "user-id",
  "name": "username",
  "role": "User",
  "exp": 1707474000,
  "iss": "SecureApi",
  "aud": "SecureApiUser"
}

Signature: HMAC-SHA256(Header + Payload, Secret)
```

---

## 📡 API Endpoints

### User Controller

#### 1. Register User
```http
POST /api/user/register
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "phoneNumber": "+1234567890",
  "userName": "johndoe",
  "password": "SecurePass123!"
}

Response (201 Created):
{
  "userId": "guid",
  "userName": "johndoe",
  "email": "john@example.com"
}
```

#### 2. Login User
```http
POST /api/user/login
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "SecurePass123!"
}

Response (200 OK):
{
  "accessToken": "eyJhbGc...",
  "refreshToken": "eyJhbGc..."
}
```

#### 3. Login with Refresh Token
```http
POST /api/user/login-with-refresh-token
Content-Type: application/json

{
  "refreshToken": "eyJhbGc..."
}

Response (200 OK):
{
  "accessToken": "eyJhbGc...",
  "refreshToken": "eyJhbGc..."
}
```

#### 4. Logout User
```http
POST /api/user/logout
Authorization: Bearer {token}

Response (200 OK):
{
  "message": "Logged out successfully"
}
```

#### 5. Confirm Email
```http
POST /api/user/confirm-email
Content-Type: application/json

{
  "email": "john@example.com",
  "confirmationToken": "token"
}

Response (200 OK):
{
  "message": "Email confirmed successfully"
}
```

#### 6. Get Secured Data
```http
GET /api/user
Authorization: Bearer {token}

Response (200 OK):
{
  "message": "This Secured Data is available only for Authenticated Users."
}
```

### Food Controller

#### 1. Get All Foods
```http
GET /api/food

Response (200 OK):
[
  {
    "id": 1,
    "name": "Margherita Pizza",
    "description": "Classic pizza",
    "price": 12.99,
    "currency": "USD",
    "categoryId": 1,
    "categoryName": "Pizzas",
    "isAvailable": true,
    "foodImage": null
  }
]
```

---

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK or higher
- SQL Server 2019 or higher
- Visual Studio 2022 or VS Code
- Node.js 18+ (for frontend)

### Installation & Setup

1. **Clone the repository**
```bash
git clone https://github.com/yourusername/foodstore.git
cd FoodStore
```

2. **Configure Database Connection**
   - Open `appsettings.json`
   - Update connection strings:
```json
{
  "ConnectionStrings": {
    "FoodStoreConnection": "Server=.;Database=FoodStore;Trusted_Connection=true;",
    "IdentityConnection": "Server=.;Database=FoodStoreIdentity;Trusted_Connection=true;"
  }
}
```

3. **Set Environment Variables**
   - Create `.env` file in root:
```
Resend__ApiKey=your_resend_api_key
JWT__SecretKey=your_jwt_secret_key_min_32_chars
```

4. **Apply Database Migrations**
```bash
cd FoodStore.Server

# Create/Update FoodStore database
dotnet ef database update -c FoodStoreDbContext

# Create/Update Identity database
dotnet ef database update -c UserDbContext
```

5. **Build the Project**
```bash
dotnet build
```

6. **Run the Application**
```bash
dotnet run
```

7. **Access the API**
   - Swagger UI: `https://localhost:5001/swagger`
   - API Base URL: `https://localhost:5001/api`

---

## 🧪 Testing the API

### Using Swagger UI
1. Navigate to `https://localhost:5001/swagger`
2. Authorize with Bearer token (click Authorize button)
3. Paste JWT token from login response
4. Execute endpoints directly from UI

### Using cURL

**Register a user:**
```bash
curl -X POST https://localhost:5001/api/user/register \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "phoneNumber": "+1234567890",
    "userName": "johndoe",
    "password": "SecurePass123!"
  }'
```

**Login:**
```bash
curl -X POST https://localhost:5001/api/user/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john@example.com",
    "password": "SecurePass123!"
  }'
```

**Access protected endpoint:**
```bash
curl -X GET https://localhost:5001/api/user \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

## 📝 Configuration Files

### appsettings.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "JWTConfiguration": {
    "SecretKey": "7c1e33d780725e19dfa38a8d2dfdc9bd",
    "Issuer": "SecureApi",
    "Audience": "SecureApiUser",
    "ExpirationInMinutes": 10
  },
  "Resend": {
    "ApiKey": ""
  }
}
```

### launchSettings.json
- Development HTTPS: `https://localhost:7261`
- Development HTTP: `http://localhost:5106`
- SPA Proxy: `https://localhost:51147`

---

## 🔄 Database Migrations

### Create a New Migration
```bash
dotnet ef migrations add MigrationName -c FoodStoreDbContext
```

### Update Database
```bash
dotnet ef database update -c FoodStoreDbContext
```

### Rollback Migration
```bash
dotnet ef migrations remove -c FoodStoreDbContext
```

### View Migrations
```bash
dotnet ef migrations list
```

---

## 📊 Logging & Monitoring

### Serilog Configuration
Logs are written to:
1. **Console** - Real-time output in development
2. **File** - `logs/log-{Date}.txt` - Persistent logging
3. **JSON Format** - Structured logs for parsing

### Log Levels
- **Information** - General application flow
- **Warning** - Potential issues
- **Error** - Application errors (logged to all sinks)

### Request Logging
Every HTTP request/response is logged with:
- Request method and path
- Status code
- Response time
- Exception details (if any)

---

## 🔒 Security Features

### Password Security
- Hashed using ASP.NET Identity (PBKDF2)
- Complex requirements enforced
- Minimum 8 characters required
- Mix of uppercase, lowercase, numbers, special characters

### JWT Token Security
- Symmetric encryption (HMAC-SHA256)
- 10-minute expiration (access token)
- Long-lived refresh tokens
- Token revocation support
- Signed with secret key

### Input Validation
- Email format validation (RFC 5322)
- Phone number format validation
- Money amount validation (no negative)
- String length constraints
- SQL injection prevention (EF Core parameterized queries)

### CORS Policy
- Development: Allows `localhost:5106`
- Production: Configurable per environment

### HTTPS Enforcement
- Redirect HTTP to HTTPS
- Secure cookies
- HSTS headers (in production)

---

## 🎯 API Response Format

### Successful Response (200/201)
```json
{
  "data": {
    "id": 1,
    "name": "Pizza Margherita",
    "price": 12.99
  }
}
```

### Error Response (4xx/5xx)
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "An error occurred while processing your request.",
  "status": 400,
  "errors": {
    "Email": ["Email address format is invalid."],
    "Password": ["Password must be at least 8 characters long."]
  }
}
```

---

## 📈 Performance Considerations

- **Soft Delete Interceptor** - Prevents accidental permanent deletion
- **AsNoTracking()** - Read-only queries for better performance
- **Automatic Query Includes** - Eager loading for related entities
- **Async Operations** - All database operations are async
- **Mapster** - Fast object mapping
- **Entity Validation** - Early error detection

---

## 📚 Development Best Practices Implemented

✅ **Clean Code**
- Single Responsibility Principle
- DRY (Don't Repeat Yourself)
- Meaningful naming conventions

✅ **SOLID Principles**
- Dependency Injection
- Interface segregation
- Abstraction over implementation

✅ **Async/Await**
- Non-blocking operations
- Better resource utilization
- Scalability

✅ **Error Handling**
- Global exception handler
- Structured error responses
- Proper HTTP status codes

✅ **Validation**
- Multi-layer validation
- Business rule enforcement
- User feedback

✅ **Security**
- Password hashing
- JWT tokens
- Email verification
- CORS configuration

✅ **Logging**
- Structured logging
- Request/response tracking
- Exception logging

✅ **Database**
- Migrations for version control
- Soft deletes
- Foreign key constraints
- Value objects for complex types

---


## 🎓 Educational Note

This project demonstrates enterprise-level ASP.NET Core development suitable for:
- Capstone projects
- Academic review
- Portfolio showcase
- Production deployment
- Team collaboration

**Technologies & Patterns Demonstrated:**
- Clean Architecture
- CQRS with MediatR
- Value Objects (Domain-Driven Design)
- Repository Pattern
- Unit of Work Pattern
- JWT Authentication
- Soft Delete Pattern
- Global Exception Handling
- Dependency Injection
- Structured Logging
- FluentValidation
- Entity Framework Core 8.0

---

**Last Updated:** February 2025
**Framework Version:** ASP.NET Core 8.0.22
**Database:** SQL Server
