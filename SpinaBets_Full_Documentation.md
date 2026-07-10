# SpinaBets Complete Project Documentation

## Table of Contents
1. Introduction
2. Objectives
3. Features
4. Technology Stack
5. Architecture
6. Database
7. Security
8. Admin Features
9. Customer Features
10. SQL Optimisation
11. Diagrams
12. Future Enhancements

## Introduction
SpinaBets is a full-stack ASP.NET Core MVC sports betting platform implementing role-based access control, betting accounts, transactions, betting, reporting and administration.

## Objectives
- Secure betting platform
- Account management
- Reporting
- Maintainable layered architecture

## Features
### Customer
- Registration/Login
- Betting accounts
- Deposits & Withdrawals
- Place Bets
- View History

### Administrator
- Dashboard
- Manage Users
- Manage Sports
- Manage Games
- Set Results
- Reports

## Technology Stack
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- ASP.NET Identity
- Bootstrap 5

## SQL Optimisation
The application uses indexes, SQL Views and Stored Procedures for efficient reporting.

## Diagrams
### Use Case
```mermaid
flowchart LR
Customer((Customer))
Admin((Admin))
Customer-->Register
Customer-->Login
Customer-->Accounts
Customer-->Deposit
Customer-->Withdraw
Customer-->PlaceBet
Customer-->ViewBets
Admin-->Dashboard
Admin-->ManageUsers
Admin-->ManageSports
Admin-->ManageGames
Admin-->SetResults
Admin-->Reports
```

### Architecture
```mermaid
flowchart LR
Browser-->MVC["ASP.NET Core MVC"]
MVC-->Services
Services-->EF["Entity Framework Core"]
EF-->SQL["SQL Server"]
SQL-->Views["SQL Views / Stored Procedures"]
```

### ERD
```mermaid
erDiagram
ApplicationUser ||--o{ Account : owns
Account ||--o{ Transaction : contains
Account ||--o{ Bet : places
Sport ||--o{ Game : has
Game ||--o{ Bet : receives
```

### Class
```mermaid
classDiagram
ApplicationUser --> Account
Account --> Transaction
Account --> Bet
Sport --> Game
Game --> Bet
```

### Bet Sequence
```mermaid
sequenceDiagram
Customer->>MVC: Place Bet
MVC->>BetService: Validate
BetService->>DB: Load Account/Game
BetService->>DB: Save Bet
DB-->>BetService: Success
BetService-->>MVC: Success
MVC-->>Customer: Confirmation
```

### Settlement
```mermaid
sequenceDiagram
Admin->>MVC:Set Result
MVC->>GameService:SetResult
GameService->>BetService:Settle Bets
BetService->>DB:Update Bets & Balance
DB-->>BetService:Saved
BetService-->>Admin:Settlement Complete
```

## Future Enhancements
- Live betting
- Payment gateways
- Notifications
- Mobile application
