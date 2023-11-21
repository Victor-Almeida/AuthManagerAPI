#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_ENVIRONMENT=Development
ENV CONNECTION_STRING="Server=sqlserver_dev;Database=IdentityDb;User Id=SA;Password=Pass@word;Integrated security=False;TrustServerCertificate=True;"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AuthManager.WebAPI/AuthManager.WebAPI.csproj", "AuthManager.WebAPI/"]
RUN dotnet restore "AuthManager.WebAPI/AuthManager.WebAPI.csproj"
COPY . .
WORKDIR "/src/AuthManager.WebAPI"
RUN dotnet build "AuthManager.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthManager.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthManager.WebAPI.dll"]