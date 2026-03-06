# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Imposta la cartella di lavoro
WORKDIR /UserService

# Copia tutto il progetto
COPY . .

# Ripristina i pacchetti NuGet usando il NuGet.config
RUN dotnet restore /UserService/UserService.API/UserService.API.csproj --configfile /UserService/NuGet.config

# Build e publish in modalità Release
RUN dotnet publish /UserService/UserService.API/UserService.API.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

# Copia i file pubblicati dallo stage di build
COPY --from=build /app/publish .

# Espone la porta dell’API
EXPOSE 80

# Avvio dell’API
ENTRYPOINT ["dotnet", "UserService.API.dll"]
