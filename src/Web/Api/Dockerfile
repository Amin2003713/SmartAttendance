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


COPY Tenants/src/Web/ApiFramework/Tenant.ApiFramework.csproj   ./Tenants/src/Web/ApiFramework/
COPY Tenants/src/Web/Api/Tenant.Api.csproj   ./Tenants/src/Web/Api/
COPY Tenants/src/Infrastructure/Persistance/Tenant.Persistence.csproj   ./Tenants/src/Infrastructure/Persistance/
COPY Tenants/src/Infrastructure/RequestHandlers/Tenant.RequestHandlers.csproj   ./Tenants/src/Infrastructure/RequestHandlers/
COPY Tenants/src/Common/Tenant.Common/Tenant.Common.csproj   ./Tenants/src/Common/Tenant.Common/
COPY Tenants/src/Core/Application/Tenant.Application.csproj   ./Tenants/src/Core/Application/
COPY Tenants/src/Core/Domain/Tenant.Domain.csproj   ./Tenants/src/Core/Domain/

COPY Shared/Ipa.Models/Ipa.Models.csproj       ./Shared/Ipa.Models/
COPY Shared/Ipa.Shared/Ipa.Shared.csproj         ./Shared/Ipa.Shared/
COPY Shared/Ipa.Framework/Ipa.Framework.csproj     ./Shared/Ipa.Framework/

# Restore dependencies
RUN dotnet restore Tenants/src/Web/Api/Tenant.Api.csproj    --verbosity detailed

# Copy all source files
COPY Tenants/ ./Tenants/
COPY Shared/ ./Shared/
# Build the project
RUN dotnet build Tenants/src/Web/Api/Tenant.Api.csproj -c Release -o /app/build  

# --------------------------------------------------
# 3) Publish Project
# --------------------------------------------------
FROM build AS publish
RUN dotnet publish Tenants/src/Web/Api/Tenant.Api.csproj -c Release -o /app/publish --no-restore

# --------------------------------------------------
# 4) Final Runtime Image
# --------------------------------------------------
FROM base AS final
WORKDIR /app

EXPOSE 8080

COPY --from=publish /app/publish ./

ENTRYPOINT ["dotnet", "Tenant.Api.dll"]
