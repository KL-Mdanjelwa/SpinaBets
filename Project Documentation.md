# SpinaBets Documentation

## Overview
SpinaBets is an ASP.NET Core MVC sports betting platform with two roles:
- Customer
- Admin

Core features include:
- Authentication and role-based authorization
- Betting account management
- Deposits and withdrawals
- Sports and games management
- Bet placement and settlement
- Reports using SQL views/stored procedures
- Admin and Customer dashboards

## System Architecture
- Presentation Layer (ASP.NET Core MVC)
- Business Layer (Services)
- Data Layer (Entity Framework Core + SQL Server)

## Functional Modules
### Customer
- Register/Login
- Manage profile
- Create betting accounts
- Deposit & withdraw funds
- Place bets
- View betting history

### Administrator
- Dashboard
- User management
- Sport CRUD
- Game CRUD
- Set results
- Settle bets
- Reports

## Database
Main entities:
- ApplicationUser
- Account
- Transaction
- Sport
- Game
- Bet

Indexes are configured on frequently queried columns and reporting is supported using SQL views and stored procedures.

## Security
- ASP.NET Identity
- Role-based authorization
- Anti-forgery validation
- Entity Framework parameterized queries

## Technologies
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Identity
- Bootstrap 5
