# Title

### ShoeShop Project

# Description

This project is an online shoe store built using [ASP.NET](https://dotnet.microsoft.com/en-us/apps/aspnet) where customers can browse and shop for shoe products without going directly to the store.

# Installation

1. Clone the project to your machine.
2. Open the project in Visual Studio.
   - Step 1: Create a database with the name 'QL_WBG'.
   - Step 2: Change the connection string according to your host name in the Web.config file
   - Step 3: Open the Package Manager Console by following the path Tool -> Nuget Package Manager -> Package Manager Console.
   - Step 4: Type ```add-migration initDB``` and press enter, then type ```update-database``` and press enter.
4. Run the application on a brower to view the shoe store interface.

# Features

- Display list of shoe products.
- Product details.
- Online ordering and payment.
- Order management and purchase history
- Send email notifications when customers place orders.
- Customer can rate the item after completing the order

# Technologies used
- ASP.NET MVC
- Entity Framework
- HTML, CSS, Javascript

# Configuration
Configure the database connection in the Web.config file

```bash
  <connectionStrings>
    <add name="MyConnectionString" connectionString="Data Source=<SQL_NAME>;Initial Catalog=QL_WBG;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False" providerName="System.Data.SqlClient" />
  </connectionStrings>
```
