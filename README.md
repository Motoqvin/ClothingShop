<div align="center">

# 🛍️ ClothingStoreApp

### Modern E-Commerce Platform built with ASP.NET Core MVC

A production-ready clothing store built with **ASP.NET Core MVC (.NET 9)** featuring secure authentication, product management, shopping cart, checkout, email notifications, user profiles, logging, and an administrator dashboard.

<p>

<img src="https://img.shields.io/badge/.NET-9-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>

<img src="https://img.shields.io/badge/ASP.NET_Core-MVC-512BD4?style=for-the-badge"/>

<img src="https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white"/>

<img src="https://img.shields.io/badge/Entity_Framework_Core-68217A?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Dapper-00599C?style=for-the-badge"/>

<img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white"/>

<img src="https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white"/>

</p>

**ASP.NET Core • PostgreSQL • Entity Framework Core • Dapper • Identity • Docker**

</div>

---

# ✨ Features

### 👤 Authentication

* User Registration
* Secure Login & Logout
* ASP.NET Core Identity
* Role-based Authorization
* Cookie Authentication
* Administrator Account Seeding

---

### 🛍️ Store

* Browse Products
* Product Details
* Shopping Cart
* Quantity Management
* Session-based Cart
* Checkout

---

### 📦 Orders

* Order History
* Order Details
* Automatic Total Calculation
* Email Confirmation after Purchase

---

### 👤 User Profile

* Update Personal Information
* Upload Avatar
* Replace Existing Avatar
* Delete Avatar
* Persistent Profile Data

---

### 👑 Admin Dashboard

* Dashboard Statistics
* Product CRUD
* User Management
* Role Management
* Order Management
* Order Details

---

### ⚙️ Infrastructure

* Repository Pattern
* Service Layer
* Entity Framework Core
* FluentValidation
* Custom Middleware
* HTTP Request Logging
* Exception Handling Middleware

---

# 🛠 Tech Stack

| Category         | Technology                |
| ---------------- | ------------------------- |
| Backend          | ASP.NET Core MVC (.NET 9) |
| Database         | PostgreSQL                |
| ORM              | Entity Framework Core     |
| Micro ORM        | Dapper                    |
| Authentication   | ASP.NET Core Identity     |
| Validation       | FluentValidation          |
| Frontend         | Bootstrap 5               |
| Email            | SMTP                      |
| Containerization | Docker & Docker Compose   |

---

# 🏗 Architecture

```
Presentation
│
├── Controllers
├── Views
├── ViewModels
└── Validators

        ↓

Application Services

        ↓

Repositories

        ↓

Entity Framework Core

        ↓

PostgreSQL
```

---

# 📁 Project Structure

```
ClothingStoreApp
│
├── src
│   ├── ClothingStoreApp.Core
│   │   ├── Models
│   │   ├── DTOs
│   │   ├── Interfaces
│   │   ├── Exceptions
│   │   ├── Services
│   │   └── Settings
│   │
│   ├── ClothingStoreApp.Infrastructure
│   │   ├── Data
│   │   ├── Repositories
│   │   ├── Services
│   │   └── Migrations
│   │
│   └── ClothingStoreApp.Presentation
│       ├── Controllers
│       ├── Views
│       ├── ViewModels
│       ├── Validators
│       ├── Middlewares
│       └── wwwroot
│
└── tests
    └── ClothingStoreApp.Tests
```

---

# 🚀 Running with Docker

Clone the repository

```bash
git clone https://github.com/Motoqvin/ClothingStoreApp.git

cd ClothingStoreApp
```

Start the application

```bash
docker compose up --build
```

The application automatically:

* Creates the PostgreSQL database
* Applies EF Core migrations
* Seeds Identity roles
* Creates the administrator account

---

## Default Administrator

```
Email:
admin@store.com

Password:
Admin123!
```

> Change these credentials before deploying.

---

# ⚙️ Environment Variables

Example configuration

```env
ConnectionStrings__IdentityDb=Host=db;Port=5432;Database=StoreDB;Username=postgres;Password=postgres

Smtp__Host=smtp.gmail.com
Smtp__Port=587
Smtp__EnableSsl=true
Smtp__Username=your_email@gmail.com
Smtp__Password=your_app_password
Smtp__FromEmail=your_email@gmail.com
Smtp__FromName=Clothing Store
```

---

# 📧 Email

The application supports SMTP email notifications.

When a customer places an order:

* Order confirmation email is sent automatically.
* If SMTP is not configured, checkout still completes successfully.

For Gmail, use an **App Password** instead of your account password.

---

# 🖼 File Uploads

Uploaded files are stored inside

```
wwwroot/uploads
```

Supported uploads:

* User Avatars
* Product Images

Features:

* GUID-based filenames
* Automatic replacement
* Old image cleanup
* Persistent Docker volume support

---

# 📝 Logging

Every HTTP request is stored in the database.

Captured information includes:

* Request Method
* Path
* Status Code
* Processing Time
* Timestamp

---

# 🧪 Running Tests

```bash
dotnet test
```

---

# 🔮 Future Improvements

* Product Categories
* Product Search
* Wishlist
* Product Reviews
* Stripe Payment Integration
* Docker Production Configuration
* CI/CD Pipeline
* Redis Caching
* JWT API
* Unit & Integration Test Coverage

---

<div align="center">

### ⭐ If you found this project useful, please consider giving it a star.

Built with ❤️ using ASP.NET Core MVC and PostgreSQL.

</div>
