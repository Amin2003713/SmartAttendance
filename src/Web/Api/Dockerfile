# --------------------------------------------------
# 1) Base Runtime Image (Runtime Only)
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# --------------------------------------------------
# 2) Build Environment (SDK)
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src


COPY src/Web/ApiFramework/Shifty.ApiFramework.csproj   ./src/Web/ApiFramework/
COPY src/Web/Api/Shifty.Api.csproj   ./src/Web/Api/
COPY src/Infrastructure/Persistance/Shifty.Persistence.csproj   ./src/Infrastructure/Persistance/
COPY src/Infrastructure/RequestHandlers/Shifty.RequestHandlers.csproj   ./src/Infrastructure/RequestHandlers/
COPY src/Common/Shifty.Common/Shifty.Common.csproj   ./src/Common/Shifty.Common/
COPY src/Core/Application/Shifty.Application.csproj   ./src/Core/Application/
COPY src/Core/Domain/Shifty.Domain.csproj   ./src/Core/Domain/

# Restore dependencies
RUN dotnet restore src/Web/Api/Shifty.Api.csproj    --verbosity detailed

# Copy all source files
COPY . .
# Build the project
RUN dotnet build src/Web/Api/Shifty.Api.csproj -c Release -o /app/build  

# --------------------------------------------------
# 3) Publish Project
# --------------------------------------------------
FROM build AS publish
RUN dotnet publish src/Web/Api/Shifty.Api.csproj -c Release -o /app/publish --no-restore

# --------------------------------------------------
# 4) Final Runtime Image
# --------------------------------------------------
FROM base AS final
WORKDIR /app

EXPOSE 8080

COPY --from=publish /app/publish ./

ENTRYPOINT ["dotnet", "Shifty.Api.dll"]
