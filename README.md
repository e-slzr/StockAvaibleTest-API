# Stock Available Test API

REST API developed in C# (.NET 8) for Tegra's inventory management. Allows management of products, boxes, categories, and transactions between boxes/products.

## Features

- ✅ REST API in C# (.NET 8)
- ✅ MS SQL Server connection
- ✅ Swagger documentation
- ✅ Repository and Unit of Work pattern
- ✅ DTOs and AutoMapper
- ✅ FluentValidation validations
- ✅ Error and exception handling
- ✅ Ready to be consumed by PHP frontend

## Prerequisites

- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 or later (recommended)

## Installation

1. Clone the repository
```bash
git clone [https://github.com/e-slzr/StockAvaibleTest-API.git]
```

2. Update the connection string in `appsettings.json`
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=your_database;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
}
```

3. Run migrations
```bash
dotnet ef database update
```

4. Run the project
```bash
dotnet run
```

## API Documentation

### 1. Base Structure
```
BASE URL: /api
Swagger UI: /swagger
```

### 2. Main Endpoints

#### Categories
```
GET    /api/categories          - Get all categories
GET    /api/categories/{id}     - Get category by ID
POST   /api/categories          - Create new category
PUT    /api/categories/{id}     - Update category
DELETE /api/categories/{id}     - Delete category
```

#### Products
```
GET    /api/products           - Get all products
GET    /api/products/{id}      - Get product by ID
POST   /api/products           - Create new product
PUT    /api/products/{id}      - Update product
DELETE /api/products/{id}      - Delete product
```

#### Boxes
```
GET    /api/boxes              - Get all boxes
GET    /api/boxes/{id}         - Get box by ID
POST   /api/boxes              - Create new box
PUT    /api/boxes/{id}         - Update box
DELETE /api/boxes/{id}         - Delete box
```

#### Transactions
```
GET    /api/transactions           - Get all transactions
GET    /api/transactions/{id}      - Get transaction by ID
POST   /api/transactions          - Create new transaction
```

### 3. Common Validations Implemented in the API
- Required fields cannot be null or empty
- Codes must be unique
- IDs must exist in the database
- Quantities must be greater than 0
- Sufficient stock for OUT transactions

### 4. Database
- SQL Server
- Relationships:
  - Category -> Products (1:N)
  - Box -> BoxProductTransactions (1:N)
  - Product -> BoxProductTransactions (1:N)

## Project Structure
StockAvaibleTest-API/
├── Controllers/            # REST Controllers
├── Services/               # Business Logic
├── Repositories/           # Data Access
├── Models/                 # Domain Entities
├── DTOs/                   # Data Transfer Objects
├── Interfaces/             # Interfaces
├── Common/                 # Shared Utilities
├── Validators/             # FluentValidation Validators
└── Data/                   # EF Core Context and Configuration