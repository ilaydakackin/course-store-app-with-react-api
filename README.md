# E-Commerce API

A comprehensive ASP.NET Core Web API for e-commerce operations with user authentication, shopping cart functionality, order management, and payment integration.

## Features

- **User Authentication & Authorization**
  - JWT token-based authentication
  - Role-based authorization (Admin, Customer)
  - User registration and login
  - ASP.NET Core Identity integration

- **Product Management**
  - View products
  - Product details with stock management
  - Product seeding with sample data

- **Shopping Cart**
  - Add/remove items from cart
  - Cart persistence for both authenticated and anonymous users
  - Cookie-based cart for anonymous users
  - Cart merging on user login

- **Order Management**
  - Create orders from cart
  - Order history tracking
  - Order status management (Pending, Approved, PaymentFailed, Completed)

- **Payment Integration**
  - Iyzipay payment gateway integration
  - Secure payment processing
  - Payment validation

- **Global Exception Handling**
  - Custom exception middleware
  - Structured error responses

## Technology Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT Bearer tokens with ASP.NET Core Identity
- **Payment Gateway**: Iyzipay
- **API Documentation**: OpenAPI (Swagger) with Scalar
- **Architecture**: RESTful API with DTO pattern

## Project Structure

```
API/
├── Controllers/           # API Controllers
│   ├── AccountController.cs      # Authentication endpoints
│   ├── CartController.cs         # Shopping cart operations
│   ├── OrdersController.cs       # Order management
│   ├── ProductsController.cs     # Product operations
│   └── ...
├── Data/                 # Database context and seeding
│   ├── DataContext.cs
│   └── SeedDatabase.cs
├── DTO/                  # Data Transfer Objects
├── Entity/               # Entity models
├── Extensions/           # Extension methods
└── Services/            # Business services
    └── TokenService.cs
```

## Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

## Installation & Setup

1. **Clone the repository**
   ```bash
   git clone [repository-url]
   cd [project-directory]
   ```

2. **Configure Connection String**
   Update the connection string in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "SQLServerConnection": "your-connection-string-here"
     }
   }
   ```

3. **Configure JWT Settings**
   Add JWT configuration to `appsettings.json`:
   ```json
   {
     "JWTSecurity": {
       "SecretKey": "your-super-secret-key-here-minimum-32-characters"
     }
   }
   ```

4. **Configure Payment Settings**
   Add Iyzipay configuration to `appsettings.json`:
   ```json
   {
     "PaymentApi": {
       "ApiKey": "your-iyzipay-api-key",
       "SecretKey": "your-iyzipay-secret-key"
     }
   }
   ```

5. **Install Dependencies**
   ```bash
   dotnet restore
   ```

6. **Update Database**
   ```bash
   dotnet ef database update
   ```

7. **Run the Application**
   ```bash
   dotnet run
   ```

## API Endpoints

### Authentication
- `POST /api/account/register` - User registration
- `POST /api/account/login` - User login
- `GET /api/account/getuser` - Get current user info

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID

### Shopping Cart
- `GET /api/cart` - Get current cart
- `POST /api/cart` - Add item to cart
- `DELETE /api/cart` - Remove item from cart

### Orders
- `GET /api/orders` - Get user orders
- `GET /api/orders/{id}` - Get specific order
- `POST /api/orders` - Create new order

## Sample Users

The application seeds with two default users:

**Admin User:**
- Username: `ilaydakackin`
- Email: `kackinilayda@gmail.com`
- Password: `Ghjnj_m40`
- Roles: Admin, Customer

**Customer User:**
- Username: `kackinmuharrem`
- Email: `kackinmuharrem@gmail.com`
- Password: `Kfdfv_l20`
- Roles: Customer

## Authentication

The API uses JWT Bearer token authentication. After logging in, include the token in the Authorization header:

```
Authorization: Bearer [your-jwt-token]
```

## Password Requirements

- Minimum 6 characters
- At least one digit
- At least one lowercase letter
- At least one uppercase letter
- At least one non-alphanumeric character

## Database Schema

### Key Entities
- **AppUser**: User accounts (extends IdentityUser)
- **AppRole**: User roles (extends IdentityRole)
- **Product**: Product information
- **Cart**: Shopping cart
- **CartItem**: Items in shopping cart
- **Order**: Customer orders
- **OrderItem**: Items in orders

## CORS Configuration

The API is configured to allow requests from `http://localhost:5173` (typically React/Vue.js development server). Update CORS settings in `Program.cs` for production deployment.

## API Documentation

When running in development mode, API documentation is available at:
- Swagger UI: `/swagger`
- Scalar UI: `/scalar/v1`
- OpenAPI JSON: `/openapi/v1.json`

## Error Handling

The application includes global exception handling middleware that:
- Logs all exceptions
- Returns structured error responses
- Shows stack traces in development mode only
- Returns generic error messages in production

## Development URLs

- HTTP: `http://localhost:5168`
- HTTPS: `https://localhost:7161`

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

[Add your license information here]

## Support

[Add contact information or support details here]