# =========================
# BUILD STAGE
# =========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# =========================
# ARG per GitHub Packages
# =========================
ARG MIRKO_NUGET_USERNAME
ARG MIRKO_NUGET_PWD

# =========================
# REGISTRA FEED NUGET PRIVATO
# =========================
RUN dotnet nuget add source https://nuget.pkg.github.com/Mirko627/index.json \
    --name github \
    --username $MIRKO_NUGET_USERNAME \
    --password $MIRKO_NUGET_PWD \
    --store-password-in-clear-text

# =========================
# COPY csproj (layer caching)
# =========================
COPY NuGet.config . 
COPY UserService.API/UserService.API.csproj UserService.API/
COPY UserService.Business/UserService.Business.csproj UserService.Business/
COPY UserService.Repository/UserService.Repository.csproj UserService.Repository/
COPY UserService.Shared/UserService.Shared.csproj UserService.Shared/

# =========================
# RESTORE
# =========================
RUN dotnet restore UserService.API/UserService.API.csproj

# =========================
# COPY FULL SOURCE
# =========================
COPY . .

WORKDIR /src/UserService.API

# =========================
# PUBLISH
# =========================
RUN dotnet publish UserService.API.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# =========================
# RUNTIME STAGE
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "UserService.API.dll"]