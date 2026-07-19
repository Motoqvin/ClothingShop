<div align="center">

# 🛍️ ClothingStoreApp

### Modern ASP.NET Core MVC Clothing Store

A full-stack e-commerce application built with **ASP.NET Core MVC**, featuring user authentication, shopping cart, checkout, profile management, email notifications, and an admin dashboard.

<p>
  <img src="https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
  <img src="https://img.shields.io/badge/ASP.NET_Core-MVC-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
  <img src="https://img.shields.io/badge/Entity_Framework-Core-5C2D91?style=for-the-badge"/>
  <img src="https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white"/>
  <img src="https://img.shields.io/badge/Bootstrap-5-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white"/>
</p>

---

### ⭐ Features

👤 Authentication • 🛒 Shopping Cart • 📦 Orders • 📧 Email Notifications • 👑 Admin Dashboard • 🖼️ Avatar Uploads

</div>

---

# 📑 Table of Contents

- 🚀 Getting Started
- 🛠 Tech Stack
- 📁 Project Structure
- ⚙️ Configuration
- 📧 Email Setup
- ✨ Features
- 🔐 Roles
- 🖼 Avatar Uploads

---

# 🛠 Tech Stack

| Category | Technologies |
|-----------|--------------|
| Backend | ASP.NET Core MVC (.NET 9) |
| Database | SQL Server |
| ORM | Entity Framework Core + Dapper |
| Authentication | ASP.NET Core Identity |
| Validation | FluentValidation |
| Frontend | Bootstrap 5, jQuery |
| Email | SMTP |

---

# 📁 Project Structure

```text
ClothingStoreApp
│
├── src
│   ├── ClothingStoreApp.Core
│   │      Domain models
│   │      DTOs
│   │      Interfaces
│   │      Enums
│   │      Exceptions
│   │
│   ├── ClothingStoreApp.Infrastructure
│   │      DbContext
│   │      Repositories
│   │      Services
│   │      Migrations
│   │
│   └── ClothingStoreApp.Presentation
│          Controllers
│          Views
│          ViewModels
│          Validators
│          wwwroot
│
└── tests
    └── ClothingStoreApp.Tests
```

---

# 🚀 Getting Started

## Prerequisites

- .NET 9 SDK
- SQL Server

---

## Configure Database

Inside

```text
src/ClothingStoreApp.Presentation/appsettings.json
```

configure

```json
"ConnectionStrings": {
  "IdentityDB": "Server=localhost;Database=StoreDB;Integrated Security=True;TrustServerCertificate=True;"
}
```

The application automatically:

- ✅ Creates the database
- ✅ Seeds roles
- ✅ Creates an administrator account

Default admin credentials

```text
Email:
admin@store.com

Password:
Admin123!
```

> ⚠️ Change the password before deploying.

---

# 📧 Email Configuration

Order confirmations are sent through SMTP.

```json
"Smtp": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "EnableSsl": true,
  "Username": "",
  "Password": "",
  "FromEmail": "",
  "FromName": "Clothing Store"
}
```

If SMTP is not configured:

- Checkout still succeeds ✅
- Email sending is skipped ✅

For Gmail use an **App Password**, not your Google account password.

You can also store credentials securely:

```bash
dotnet user-secrets set "Smtp:Username" "your@email.com"
dotnet user-secrets set "Smtp:Password" "your-password"
```

---

# ▶️ Running the Project

Restore packages

```bash
dotnet restore
```

Run

```bash
dotnet run --project src/ClothingStoreApp.Presentation
```

Run tests

```bash
dotnet test tests/ClothingStoreApp.Tests
```

---

# ✨ Features

## 👤 User Authentication

- Register
- Login
- Logout
- Role-based authorization
- ASP.NET Identity

---

## 👤 User Profile

- View profile
- Edit username
- Edit email
- Edit phone number
- Upload avatar
- Replace avatar
- Automatic deletion of previous avatar

---

## 🧥 Product Catalog

- Browse products
- Product details
- Categories
- Product images

---

## 🛒 Shopping Cart

- Add items
- Remove items
- Update quantities
- Session-based cart

---

## 💳 Checkout

- Create orders
- Save order history
- Send confirmation email

---

## 👑 Admin Dashboard

Available only for **Admin** users.

Features include:

- Dashboard
- Product Management
- User Management
- Role Management
- Order Management
- Order Details

---

## 📝 Logging

Every HTTP request is logged using

```
IHttpLogRepository
```

---

# 🔐 Roles

| Role | Permissions |
|------|-------------|
| 👤 User | Shop, manage profile, place orders |
| 👑 Admin | Full administrative access |

---

# 🖼 Avatar Uploads

User avatars are stored in

```text
wwwroot/uploads/avatars/
```

Features

- Upload profile pictures
- Automatically generate GUID filenames
- Delete previous avatar when uploading a new one
- Default avatar for users without an uploaded image

---

# 📸 Screenshots

You can add screenshots here.

```md
## Home

![Home](screenshots/home.png)

## Products

![Products](screenshots/products.png)

## Shopping Cart

![Cart](screenshots/cart.png)

## Admin Dashboard

![Admin](screenshots/admin.png)
```

---

<div align="center">

### ⭐ If you like this project, consider giving it a star!

Made with ❤️ using ASP.NET Core MVC

</div>