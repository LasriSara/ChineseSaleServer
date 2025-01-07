# Web API - מכירה סינית (Server Side)

This is the backend part of the system for managing a Chinese lottery sale. It is built using ASP.NET Core Web API.

## Technologies:
- .NET Core (Web API)
- Entity Framework Core (Code First)
- JWT Authentication (for security)
- Dependency Injection (DI)

## Features:
As an admin, you can:
- Login with username and password (JWT authentication).
- Manage donors (view, add, edit, delete).
- Manage gifts (view, add, edit, delete).
- Manage ticket purchases and view buyer details.
- Perform lotteries for each gift and generate reports.
- Send emails to winners (Challenge task).

## Running the API:
1. Pre-requisites: Microsoft Visual Studio 2022 version (and on). Microsoft SQL Server Management Studio (SSMS)
2. Ensure you're using .NET Core 3.1 or higher.
3. Clone this repository and navigate to the project directory.
4. Build the database with the command:
   ```bash
   dotnet ef database update
   You can use the capabilities of CODE FIRST and this is how the DB will be created.
   
