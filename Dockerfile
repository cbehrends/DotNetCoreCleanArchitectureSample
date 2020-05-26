FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app 
#
# copy csproj and restore as distinct layers
COPY *.sln .
COPY Claims.WebApi/*.csproj ./Claims.WebApi/
COPY Claims.Application/*.csproj ./Claims.Application/
COPY Claims.Domain/*.csproj ./Claims.Domain/
COPY Claims.Infrastructure/*.csproj ./Claims.Infrastructure/
COPY Claims.UnitTests/*.csproj ./Claims.UnitTests/ 
COPY Claims.IntegrationTests/*.csproj ./Claims.IntegrationTests/
#
RUN dotnet restore 
#
# copy everything else and build app

COPY Claims.WebApi/. ./Claims.WebApi/
COPY Claims.Application/. ./Claims.Application/
COPY Claims.Domain/. ./Claims.Domain/
COPY Claims.Infrastructure/. ./Claims.Infrastructure/
COPY Claims.UnitTests/. ./Claims.UnitTests/ 
COPY Claims.IntegrationTests/. ./Claims.IntegrationTests/

#
WORKDIR /app/Claims.WebApi
RUN dotnet publish -c Release -o out 
#
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app 
#
COPY --from=build /app/Claims.WebApi/out ./
ENTRYPOINT ["dotnet", "Claims.WebApi.dll"]