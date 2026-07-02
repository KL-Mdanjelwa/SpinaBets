# 🎲 Spina Bets

**Spina Bets** is an educational betting simulation web application developed using **ASP.NET Core MVC**, **Entity Framework Core**, **SQL Server**, and **ASP.NET Core Identity**.

> **Disclaimer:** This application is a simulation developed for educational purposes only. It does not facilitate real-money gambling or financial transactions.

---

# 📖 Overview

Spina Bets allows users to create betting accounts, deposit and withdraw virtual funds, place bets on sporting events, and track betting history. Administrators can manage sports, games, users, reports, and settle betting results.

The application demonstrates the implementation of:

- ASP.NET Core MVC Architecture
- Entity Framework Core
- SQL Server
- Identity Authentication & Authorization
- Service Layer Architecture
- CRUD Operations
- Stored Procedures
- SQL Views
- Database Indexing
- Bootstrap Responsive UI

---

# 🏗️ Technology Stack

- ASP.NET Core MVC (.NET)
- C#
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- Bootstrap 5
- HTML5
- CSS3
- JavaScript

---

# 📂 Project Architecture

```
Presentation Layer
        │
        ▼
Controllers
        │
        ▼
Services
        │
        ▼
Entity Framework Core
        │
        ▼
SQL Server Database
```

Business logic is contained within the Service layer while Controllers remain lightweight.

---

# 🔐 User Roles

## Customer

Customers can:

- Register
- Login
- Manage profile
- Change password
- Create betting accounts
- Deposit virtual funds
- Withdraw virtual funds
- View available games
- Place bets
- View betting history
- View transactions
- Monitor account balances

---

## Administrator

Administrators can:

- View dashboard
- Manage users
- Create sports
- Edit sports
- Delete sports
- Create games
- Edit games
- Delete games
- Set game results
- Settle bets
- View reports

---

# 💰 Betting Flow

1. Customer registers
2. Customer creates a betting account
3. Customer deposits virtual funds
4. Customer views available games
5. Customer places a bet
6. Stake is deducted immediately
7. Administrator settles the game
8. Winning bets receive payouts
9. Losing bets remain deducted

---

# 🏦 Account Management

Each user can create betting accounts for different sports.

Supported account types include:

- Soccer
- Horse Racing
- Basketball
- Esports
- General

Accounts can be:

- Created
- Viewed
- Closed
- Reopened

Business rules ensure:

- Closed accounts cannot receive transactions.
- Accounts cannot be closed while carrying a balance.

---

# 💸 Transaction System

Supported transactions:

- Deposit
- Withdrawal

Business rules:

- Deposits increase account balance.
- Withdrawals reduce account balance.
- Insufficient funds are prevented.
- Future-dated transactions are not allowed.
- Closed accounts cannot receive transactions.

---

# 🎮 Supported Sports

- Soccer
- Horse Racing
- Basketball
- Esports
- General

Sports are managed by administrators.

---

# ⚽ Games

Administrators can:

- Create games
- Edit games
- Delete games
- Set results

Games include:

- Home Team
- Away Team
- Start Time
- Home Odds
- Away Odds
- Draw Odds
- Status
- Result

---

# 🎯 Betting

Customers may place bets by selecting:

- Home
- Away
- Draw

Each bet stores:

- Stake
- Odds
- Selection
- Potential Win
- Status
- Settlement Date

---

# 📊 Reports

Reporting functionality demonstrates:

- SQL Stored Procedures
- SQL Views
- Entity Framework Core integration

Reports include:

- Betting reports
- Dashboard statistics
- Administrative summaries

---

# 🗄 Database Features

The project demonstrates database optimization through:

- SQL Views
- Stored Procedures
- Foreign Keys
- Entity Relationships
- Indexed Columns

Indexes are applied to commonly searched fields such as:

- UserId
- AccountId
- GameId
- SportId
- TransactionDate
- Bet Status

---

# 🔒 Authentication

Authentication is implemented using ASP.NET Core Identity.

Features include:

- Registration
- Login
- Logout
- Role-based Authorization
- Profile Management
- Password Management

Roles:

- Admin
- Customer

---

# 📱 Responsive Design

The application uses Bootstrap 5 to provide:

- Responsive navigation
- Mobile-friendly layouts
- Dashboard cards
- Tables
- Forms
- Modern user interface

---

# 📁 Project Structure

```
Controllers/
Models/
Services/
Interfaces/
ViewModels/
Views/
wwwroot/
Data/
Migrations/
DbContext/
DTO/
```

---

# 🚀 Getting Started

## Requirements

- Visual Studio 2022
- .NET SDK
- SQL Server
- SQL Server Management Studio (optional)

---

## Installation

1. Clone the repository

```bash
git clone https://github.com/yourusername/SpinaBets.git
```

2. Open the solution in Visual Studio.

3. Configure the connection string inside:

```
appsettings.json
```

4. Apply migrations:

```powershell
Update-Database
```

5. Run the project.

---

# 📚 Educational Purpose

This project was developed to demonstrate:

- ASP.NET Core MVC
- Entity Framework Core
- Authentication & Authorization
- CRUD Operations
- Database Design
- Layered Architecture
- SQL Optimization
- Software Engineering Principles

It is intended solely for educational demonstration and does **not** support real-money betting.

---

# 👨‍💻 Author

Developed by **Kamvalethu Mdanjelwa**

Spina Bets Betting Simulation System
