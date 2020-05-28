FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app 
#
# copy csproj and restore as distinct layers
#COPY *.sln .
COPY Common.ApplicationCore/Common.ApplicationCore.csproj ./Common.ApplicationCore/
COPY Payments.WebApi/*.csproj ./Payments.WebApi/
COPY Payments.Application/*.csproj ./Payments.Application/
COPY Payments.Domain/*.csproj ./Payments.Domain/
COPY Payments.Infrastructure/*.csproj ./Payments.Infrastructure/

#
RUN dotnet restore Payments.WebApi/Payments.WebApi.csproj
#
# copy everything else and build app
COPY Common.ApplicationCore/. ./Common.ApplicationCore/
COPY Payments.WebApi/. ./Payments.WebApi/
COPY Payments.Application/. ./Payments.Application/
COPY Payments.Domain/. ./Payments.Domain/
COPY Payments.Infrastructure/. ./Payments.Infrastructure/

#
WORKDIR /app/Payments.WebApi
RUN dotnet publish -c Release -o out 
#
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app 
#
COPY --from=build /app/Payments.WebApi/out ./
ENTRYPOINT ["dotnet", "Payments.WebApi.dll"]