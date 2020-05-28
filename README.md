# Clean Architecture Style

# Docker Support

To build the docker env

`docker-compose build`

This builds all of the projects and packages them into a docker container.  
It also setups up SQL Server and sets the env var for the connection string as well as RabbitMQ

To support AQMP, I've used  [MassTransit](https://masstransit-project.com/).  This allows me to easily use RabbitMQ, Azure Service Bus or Amazon SQS as needed.

To run the project with all required services:

`docker-compose up`

This will setup the Claims container to listen on port 5000 and the Payments to listen on 5004


When docker-compose has started the services `http://localhost:5000/swagger` (Claims) or `http://localhost:5004/swagger` (Payments) and you should see the Swagger UI


# Database Migrations

Add Migration

`dotnet ef migrations add <migration name>  --project <Project>.Infrastructure --startup-project <Project>.WebApi --output-dir Data/Migrations`


To update the database after migration has been added

`dotnet ef database update  --project <Project>.Infrastructure --startup-project <Project>.WebApi`
