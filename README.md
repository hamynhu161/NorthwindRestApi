Note:
- Scaffold-DbContext is run again in Package Manager Console when database has changed or want to update the C# code that represents tables: <br/>
   Scaffold-DbContext -Connection "Server=NHUHA\SQLEXPRESS;Database=northwind;Trusted_Connection=True;TrustServerCertificate=True" -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force
- "-force": Tells EF to overwrite existing files if they already exist in the Models folder
