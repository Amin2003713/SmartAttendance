# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
EXPOSE 5010
EXPOSE 5011

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src

# Copy project files
COPY ["src/Web/Api/Api.csproj", "src/Web/Api/"]
COPY ["src/Web/ApiFramework/ApiFramework.csproj", "src/Web/ApiFramework/"]
COPY ["src/Infrastructure/Persistance/Persistance.csproj", "src/Infrastructure/Persistance/"]
COPY ["src/Core/Domain/Domain.csproj", "src/Core/Domain/"]
COPY ["src/Core/Application/Application.csproj", "src/Core/Application/"]
COPY ["src/Common/Common.csproj", "src/Common/"]

# Restore dependencies
RUN dotnet restore "src/Web/Api/Api.csproj"

# Copy the full source and build
COPY . .
WORKDIR "/src/src/Web/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Use a non-root user for better security
RUN addgroup --system appgroup && adduser --system appuser --ingroup appgroup
USER appuser

ENTRYPOINT ["dotnet", "Api.dll"]
