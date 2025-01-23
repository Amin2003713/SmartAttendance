

# --------------------------------------------------
# 1) Base runtime image
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 443
EXPOSE 80

# --------------------------------------------------
# 2) Build + Restore stage
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Define build argument with default value
ARG CONFIGURATION=Development

# Copy the solution file and restore dependencies
COPY ["Shifty.sln", "./"]
COPY ["src/Web/Api/Shifty.Api.csproj", "src/Web/Api/"]
COPY ["src/Web/ApiFramework/Shifty.ApiFramework.csproj", "src/Web/ApiFramework/"]
COPY ["src/Infrastructure/Persistance/Shifty.Persistence.csproj", "src/Infrastructure/Persistance/"]
COPY ["src/Core/Domain/Shifty.Domain.csproj", "src/Core/Domain/"]
COPY ["src/Core/Application/Shifty.Application.csproj", "src/Core/Application/"]
COPY ["src/Common/Shifty.Common/Shifty.Common.csproj", "src/Common/"]
COPY ["src/Common/Shifty.Resources/Shifty.Resources.csproj", "src/Resources/"]
# Restore dependencies for all projects
RUN dotnet restore

# Copy the entire source code
COPY . .

# --------------------------------------------------
# 3) Build stage
# --------------------------------------------------
RUN dotnet build "src/Web/Api/Shifty.Api.csproj" -c $CONFIGURATION -o /app/build

# --------------------------------------------------
# 4) Publish stage
# --------------------------------------------------
FROM build AS publish
RUN dotnet publish "src/Web/Api/Shifty.Api.csproj" -c $CONFIGURATION -o /app/publish --no-restore

# --------------------------------------------------
# 5) Final runtime stage
# --------------------------------------------------
FROM base AS final
WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .


# Trust the HTTPS developer certificate (runtime)
RUN dotnet dev-certs https --trust || echo "Skipping certificate trust"

ENTRYPOINT ["dotnet", "Shifty.Api.dll"]
