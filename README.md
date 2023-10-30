# AuthManagerAPI
A simple .NET REST API for handling authentication and authorization (still in progress). This API uses Swagger to document the endpoints' descriptions, return objects and HTTP codes.

## Getting started
To run this application using docker-compose, run the following commands in the terminal:

```
$ docker compose up
$ cd src/AuthManager.Persistence
$ dotnet ef --startup-project ../AuthManager.WebAPI/ database update --connection "Server=localhost\sqlserver_dev,5434;Database=IdentityDb;User Id=SA;Password=Pass@word;Integrated security=False;TrustServerCertificate=True;"
$ cd ../..
$ docker compose run webapi_dev
```

Now you can access the application at `localhost:5000/swagger/index.html`. 
To authenticate, you can use the admin account that was created with the migration by passing the following credentials to the `POST api/auth/login` route's body:
```
{
  "email": "admin@admin.com",
  "password": "Admin@123"
}
```
