# --------------------------------------------------
# 1) Base runtime image
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
# Ports exposed by the app
EXPOSE 80
EXPOSE 443
EXPOSE 8080

# --------------------------------------------------
# 2) Build + Restore stage
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/Web/Api/Shifty.Api.csproj", "src/Web/Api/"]
COPY ["src/Web/ApiFramework/Shifty.ApiFramework.csproj", "src/Web/ApiFramework/"]
COPY ["src/Infrastructure/Persistance/Shifty.Persistence.csproj", "src/Infrastructure/Persistance/"]
COPY ["src/Core/Domain/Shifty.Domain.csproj", "src/Core/Domain/"]
COPY ["src/Core/Application/Shifty.Application.csproj", "src/Core/Application/"]
COPY ["src/Common/Shifty.Common.csproj", "src/Common/"]

# Restore dependencies
RUN dotnet restore "src/Web/Api/Shifty.Api.csproj"

# Copy the rest of the source
COPY . . 

# Build in Debug mode
WORKDIR "/src/src/Web/Api"
RUN dotnet build "Shifty.Api.csproj" -c Debug -o /app/build

# --------------------------------------------------
# 3) Publish stage
# --------------------------------------------------
FROM build AS publish
WORKDIR "/src/src/Web/Api"
RUN dotnet publish "Shifty.Api.csproj" -c Debug -o /app/publish

# --------------------------------------------------
# 4) Final runtime stage
# --------------------------------------------------
FROM base AS final
WORKDIR /app

# Copy published output (DLLs etc.)
COPY --from=publish /app/publish /app

# (Optional) If you also want the *pdb files*, they are already in /app, 
# but you can arrange them differently if needed.

# Use a non-root user (recommended for .NET images)
RUN addgroup --system appgroup && adduser --system appuser --ingroup appgroup
USER appuser

# The app listens on 80/443/8080 by default
ENTRYPOINT ["dotnet", "Shifty.Api.dll"]
