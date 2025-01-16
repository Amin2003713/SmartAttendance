# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/Web/Api/Shifty.Api.csproj", "src/Web/Api/"]
COPY ["src/Web/ApiFramework/Shifty.ApiFramework.csproj", "src/Web/ApiFramework/"]
COPY ["src/Infrastructure/Shifty.Persistance/Persistance.csproj", "src/Infrastructure/Persistance/"]
COPY ["src/Core/Domain/Shifty.Domain.csproj", "src/Core/Domain/"]
COPY ["src/Core/Application/Shifty.Application.csproj", "src/Core/Application/"]
COPY ["src/Common/Shifty.Common.csproj", "src/Common/"]

# Restore dependencies
RUN dotnet restore "src/Web/Api/Shifty.Api.csproj"

# Copy the full source and build in Debug mode
COPY . .
WORKDIR "/src/src/Web/Api"
RUN dotnet build "Api.csproj" -c Debug -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Api.csproj" -c Debug -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .
  
# Copy source files for debugging
COPY --from=build /src/src /app/src

# Expose debugging port if necessary
EXPOSE 8080

# Use a non-root user for better security
RUN addgroup --system appgroup && adduser --system appuser --ingroup appgroup
USER appuser


ENTRYPOINT ["dotnet", "Shifty.Api.dll"]