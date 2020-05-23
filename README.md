# Clean Architecture Style

# Docker Support

To build the container

`docker build -t <my_container_name> .`

To run the container in your local Docker instance

`docker run -p 8080:80 <my_container_name>`

This will setup the container to listen on port 8080, you may adjust as needed.

Once the container is running, navigate to `http://localhost:8080`

# Database Migrations

Add Migration

`dotnet ef migrations add <migration name>  --project Claims.Infrastructure --startup-project Claims.WebApi --output-dir Data/Migrations`


To update the database after migration has been added

`dotnet ef database update  --project Claims.Infrastructure --startup-project Claims.WebApi`