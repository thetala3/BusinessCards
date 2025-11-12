# BusinessCards

Hello dear, welcome onboard for my BuisnessCards Project ^^ 
## 🧠 A delightful little full-stack adventure Built with **.Net 8 web API**, **Angular**, and just enough caffeine to make it to the deadline !! 

## ⚙️ Setup (for the brave)

1. **Clone the repo**
  git clone https://github.com/thetala3/BusinessCards.git
   
3. **Backend** :
    cd BusinessCards.WebApi
    dotnet run

4. **Frontend**:
   cd business-cards-ui
   npm install
   npm start

## 🧪 Features :
✅ Add, view, delete, and export business cards
✅ Export data as CSV and XML
✅ Beautiful clean architecture (because chaos hurts my eyes)
✅ Global exception handler (to pretend bugs don’t exist)
🚫 Import feature – it was there, but it broke Swagger so I sent it to a better place

## 🗃️ Project Structure
BusinessCards
├── Application       # DTOs, Interfaces, and Business Logic
├── Domain            # Entities and Core Models
├── Infrastructure    # EF Core, Services, Persistence
├── WebApi            # Controllers, Middleware, and Swagger
└── Tests             # Removed because I value my sanity

## 💾 Database
A SQL Server database file (BusinessCardsDB.sql) is included.
Just restore it, and you’re good to go.
If it fails — congratulations, you’ve now joined the club of every .NET developer ever.

## 🗄 Database Setup
The database is provided for convenience:

- Option 1: Restore `BusinessCardsDB.bak` using SQL Server Management Studio from : "BusinessCards\BusinessCards.WebApi\BusinessCardsDb.sql"
- Option 2: Run the included `BusinessCardsDB.sql` script
- Option 3: Or simply run `dotnet ef database update` to create the schema automatically

Connection string:
Note: you can find it in "appsettings.Development.json"
  "ConnectionStrings": {
    "SqlServer": "Server=(localdb)\\MSSQLLocalDB;Database=BusinessCards;Trusted_Connection=True;MultipleActiveResultSets=true"
  }

