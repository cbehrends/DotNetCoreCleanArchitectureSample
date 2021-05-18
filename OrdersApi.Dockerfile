FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app 
#
# copy csproj and restore as distinct layers
#COPY *.sln .
COPY Common.ApplicationCore/Common.ApplicationCore.csproj ./Common.ApplicationCore/
COPY Common.Messaging/Common.Messaging.csproj ./Common.Messaging/
COPY Orders.WebApi/*.csproj ./Orders.WebApi/
COPY Orders.Application/*.csproj ./Orders.Application/
COPY Orders.Domain/*.csproj ./Orders.Domain/
COPY Orders.Infrastructure/*.csproj ./Orders.Infrastructure/

#
RUN dotnet restore Orders.WebApi/Orders.WebApi.csproj
#
# copy everything else and build app
COPY Common.ApplicationCore/. ./Common.ApplicationCore/
COPY Common.Messaging/. ./Common.Messaging/
COPY Orders.WebApi/. ./Orders.WebApi/
COPY Orders.Application/. ./Orders.Application/
COPY Orders.Domain/. ./Orders.Domain/
COPY Orders.Infrastructure/. ./Orders.Infrastructure/

#
WORKDIR /app/Orders.WebApi
RUN dotnet publish -c Release -o out 
#
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app 
#
COPY --from=build /app/Orders.WebApi/out ./
ENTRYPOINT ["dotnet", "Orders.WebApi.dll"]