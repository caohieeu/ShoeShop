# Title

**ShoeShop Project**

# Description

This project is an online shoe store built using [ASP.NET](https://dotnet.microsoft.com/en-us/apps/aspnet) where customers can browse and shop for shoe products without going directly to the store.

# Installation

1. Clone the project to your machine.
2. Open the project in Visual Studio
3. Run the application on a brower to view the shoe store interface

```bash
pip install foobar
```

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
## Usage

```python
import foobar

# returns 'words'
foobar.pluralize('word')

# returns 'geese'
foobar.pluralize('goose')

# returns 'phenomenon'
foobar.singularize('phenomena')
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

[MIT](https://choosealicense.com/licenses/mit/)
