#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=https://+:443;http://+:80   
ENV CONNECTION_STRING="Server=sqlserver_dev;Database=IdentityDb;User Id=SA;Password=Pass@word;Integrated security=False;TrustServerCertificate=True;"

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AuthManagerAPI/AuthManager.WebAPI.csproj", "AuthManagerAPI/"]
RUN dotnet restore "AuthManagerAPI/AuthManager.WebAPI.csproj"
COPY . .
WORKDIR "/src/AuthManagerAPI"
RUN dotnet build "AuthManager.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthManager.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthManager.WebAPI.dll", "--urls", "http://*:5000;https://*:5001"]