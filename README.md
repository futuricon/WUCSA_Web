dotnet ef migrations add InitPostgres --project src/WUCSA.Infrastructure --startup-project src/WUCSA.Web

dotnet ef database update --project src/WUCSA.Infrastructure --startup-project src/WUCSA.Web
