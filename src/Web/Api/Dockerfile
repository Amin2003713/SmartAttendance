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


COPY src/Web/ApiFramework/SmartAttendance.ApiFramework.csproj   ./src/Web/ApiFramework/
COPY src/Web/Api/SmartAttendance.Api.csproj   ./src/Web/Api/
COPY src/Infrastructure/Persistance/SmartAttendance.Persistence.csproj   ./src/Infrastructure/Persistance/
COPY src/Infrastructure/RequestHandlers/SmartAttendance.RequestHandlers.csproj   ./src/Infrastructure/RequestHandlers/
COPY src/Common/SmartAttendance.Common/SmartAttendance.Common.csproj   ./src/Common/SmartAttendance.Common/
COPY src/Core/Application/SmartAttendance.Application.csproj   ./src/Core/Application/
COPY src/Core/Domain/SmartAttendance.Domain.csproj   ./src/Core/Domain/

# Restore dependencies
RUN dotnet restore src/Web/Api/SmartAttendance.Api.csproj    --verbosity detailed

# Copy all source files
COPY . .
# Build the project
RUN dotnet build src/Web/Api/SmartAttendance.Api.csproj -c Release -o /app/build  

# --------------------------------------------------
# 3) Publish Project
# --------------------------------------------------
FROM build AS publish
RUN dotnet publish src/Web/Api/SmartAttendance.Api.csproj -c Release -o /app/publish --no-restore

# --------------------------------------------------
# 4) Final Runtime Image
# --------------------------------------------------
FROM base AS final
WORKDIR /app

EXPOSE 8080

COPY --from=publish /app/publish ./

ENTRYPOINT ["dotnet", "SmartAttendance.Api.dll"]
