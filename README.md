# Clean Architecture Style

# Docker Support

To build the docker env

`docker-compose build`

This builds all of the projects and packages them into a docker container.  
It also setups up SQL Server and sets the env var for the connection string

To run the service in docker along with SQL

`docker-compose up`

This will setup the container to listen on port 5000

When docker-compose has started teh services `http://localhost:5000/swagger` and you should see the Swagger UI


# Database Migrations

Add Migration

`dotnet ef migrations add <migration name>  --project Claims.Infrastructure --startup-project Claims.WebApi --output-dir Data/Migrations`


To update the database after migration has been added

`dotnet ef database update  --project Claims.Infrastructure --startup-project Claims.WebApi`