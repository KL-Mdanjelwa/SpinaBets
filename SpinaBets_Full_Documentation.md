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
Admin((Administrator))

Register([Register])
Login([Login])
ManageProfile([Manage Profile])
CreateAccount([Create Betting Account])
Deposit([Deposit Funds])
Withdraw([Withdraw Funds])
ViewAccounts([View Accounts])
PlaceBet([Place Bet])
ViewBets([View Bets])
ViewGames([View Games])
ViewReports([View Reports])

ManageUsers([Manage Users])
ManageSports([Manage Sports])
ManageGames([Manage Games])
SetGameResult([Set Game Result])
SettleBets([Settle Bets])
AdminReports([Generate Reports])

Customer --> Register
Customer --> Login
Customer --> ManageProfile
Customer --> CreateAccount
Customer --> Deposit
Customer --> Withdraw
Customer --> ViewAccounts
Customer --> ViewGames
Customer --> PlaceBet
Customer --> ViewBets

Admin --> Login
Admin --> ManageUsers
Admin --> ManageSports
Admin --> ManageGames
Admin --> SetGameResult
Admin --> SettleBets
Admin --> AdminReports
```

### Architecture
```mermaid
flowchart TD

Browser[Web Browser]

Controllers[ASP.NET MVC Controllers]

Views[Razor Views]

Services[Business Services]

AccountService[AccountService]
TransactionService[TransactionService]
BetService[BetService]
SportService[SportService]
GameService[GameService]
ReportService[ReportService]

EF[Entity Framework Core]

DbContext[ApplicationDbContext]

SQL[(SQL Server)]

StoredProc[(Stored Procedures)]

ViewsDB[(SQL Views)]

Browser --> Controllers
Controllers --> Views

Controllers --> Services

Services --> AccountService
Services --> TransactionService
Services --> BetService
Services --> SportService
Services --> GameService
Services --> ReportService

AccountService --> EF
TransactionService --> EF
BetService --> EF
SportService --> EF
GameService --> EF
ReportService --> EF

EF --> DbContext

DbContext --> SQL
DbContext --> StoredProc
DbContext --> ViewsDB
```

### ERD
```mermaid
erDiagram

APPLICATIONUSER ||--o{ ACCOUNT : owns

ACCOUNT ||--o{ TRANSACTION : contains

ACCOUNT ||--o{ BET : places

SPORT ||--o{ GAME : contains

GAME ||--o{ BET : receives

APPLICATIONUSER {

string Id
string FirstName
string Surname
string Email
string IDNumber
}

ACCOUNT{

int AccountId
string AccountNumber
decimal Balance
bool IsClosed
AccountType AccountType
}

TRANSACTION{

int TransactionId
decimal Amount
TransactionType TransactionType
datetime TransactionDate
}

SPORT{

int SportId
string Name
}

GAME{

int GameId
string HomeTeam
string AwayTeam
datetime StartTime
decimal HomeOdds
decimal AwayOdds
decimal DrawOdds
string Status
string Result
}

BET{

int BetId
decimal Stake
decimal Odds
string Selection
BetStatus Status
datetime PlacedDate
datetime SettledDate
}
```

### Class
```mermaid
classDiagram

class ApplicationUser{
+Id
+FirstName
+Surname
+Email
+IDNumber
}

class Account{
+AccountId
+AccountNumber
+Balance
+AccountType
+IsClosed
}

class Transaction{
+TransactionId
+Amount
+TransactionType
+TransactionDate
}

class Sport{
+SportId
+Name
}

class Game{
+GameId
+HomeTeam
+AwayTeam
+HomeOdds
+AwayOdds
+DrawOdds
+Status
+Result
}

class Bet{
+BetId
+Selection
+Stake
+Odds
+Status
+PotentialWin()
}

ApplicationUser "1" --> "*" Account

Account "1" --> "*" Transaction

Account "1" --> "*" Bet

Sport "1" --> "*" Game

Game "1" --> "*" Bet

class AccountService

class TransactionService

class BetService

class SportService

class GameService

class ReportService

AccountService --> Account
TransactionService --> Transaction
BetService --> Bet
SportService --> Sport
GameService --> Game
ReportService --> Bet
```

### Bet Sequence
```mermaid
sequenceDiagram

actor Customer

participant View

participant BetsController

participant BetService

participant ApplicationDbContext

participant SQLServer

Customer->>View: Select Game

Customer->>View: Enter Stake

View->>BetsController: POST Create Bet

BetsController->>BetService: PlaceBetAsync()

BetService->>ApplicationDbContext: Retrieve Account

ApplicationDbContext->>SQLServer: Query Account

SQLServer-->>ApplicationDbContext: Account

BetService->>ApplicationDbContext: Retrieve Game

ApplicationDbContext->>SQLServer: Query Game

SQLServer-->>ApplicationDbContext: Game

BetService->>BetService: Validate Balance

BetService->>BetService: Calculate Odds

BetService->>ApplicationDbContext: Save Bet

ApplicationDbContext->>SQLServer: INSERT Bet

SQLServer-->>ApplicationDbContext: Success

ApplicationDbContext-->>BetService: Saved

BetService-->>BetsController: Success

BetsController-->>View: Redirect to My Bets
```

### Settlement
```mermaid
sequenceDiagram

actor Admin

participant GamesController

participant GameService

participant BetService

participant ApplicationDbContext

participant SQLServer

Admin->>GamesController: Select Match Result

GamesController->>GameService: SetResult()

GameService->>ApplicationDbContext: Update Game Result

ApplicationDbContext->>SQLServer: UPDATE Game

SQLServer-->>ApplicationDbContext: Success

GamesController->>BetService: SettleBet()

BetService->>ApplicationDbContext: Retrieve Pending Bets

ApplicationDbContext->>SQLServer: SELECT Bets

SQLServer-->>ApplicationDbContext: Bets

loop Every Pending Bet

BetService->>BetService: Compare Selection

alt Winning Bet

BetService->>ApplicationDbContext: Credit Account

else Losing Bet

BetService->>ApplicationDbContext: Mark Lost

end

end

ApplicationDbContext->>SQLServer: Save Changes

SQLServer-->>ApplicationDbContext: Success

BetService-->>GamesController: Settlement Complete

GamesController-->>Admin: Success Message
```

## Future Enhancements
- Live betting
- Payment gateways
- Notifications
- Mobile application
