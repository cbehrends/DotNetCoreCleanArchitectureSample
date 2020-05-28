# Clean Architecture Style

# Architecture

This project is using the [Clean Archeture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) style.  I've found that this really helps enforce a SOLID and human-readable style of coding.  

Using the [mediator pattern](https://en.wikipedia.org/wiki/Mediator_pattern) to implment [CQRS](https://martinfowler.com/bliki/CQRS.html) helps encapsulate the logic and enforce the [Single Responsiblty Principal](https://en.wikipedia.org/wiki/Single-responsibility_principle).

To facilitate back-end communication between services, I've implmented AQMP to avoid direct access between services.  In this example, the "Claims" service tracks the total cost of the claim.  When payment of the claim is approved, an event is raised and pushed onto the queue.  The "Payments" service will then read that message and perform the payment logic.  If successful, the payment service will fire an event stating that the claim has been paid.  The claim service will then consume that message and update the Amount Due field.

#ToDo

1.  Add transaction support the the mediator pipeline


# Docker Support

To build the docker env

`docker-compose build`

This builds all of the projects and packages them into a docker container.  
It also setups up SQL Server and sets the env var for the connection string as well as RabbitMQ

To support AQMP, I've used  [MassTransit](https://masstransit-project.com/).  This allows me to easily use RabbitMQ, Azure Service Bus or Amazon SQS as needed.

To run the project with all required services:

`docker-compose up -d`

This will setup the Claims container to listen on port 5000 and the Payments to listen on 5004


When docker-compose has started the services `http://localhost:5000/swagger` (Claims) or `http://localhost:5004/swagger` (Payments) and you should see the Swagger UI


# Database Migrations

Add Migration

`dotnet ef migrations add <migration name>  --project <Project>.Infrastructure --startup-project <Project>.WebApi --output-dir Data/Migrations`


To update the database after migration has been added

`dotnet ef database update  --project <Project>.Infrastructure --startup-project <Project>.WebApi`
