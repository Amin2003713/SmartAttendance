# --------------------------------------------------
# 1) Base runtime image
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
# --------------------------------------------------
# 2) Build + Restore stage
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the solution file and restore dependencies
#COPY ["Shifty.sln", "./"]
COPY ["src/Web/Api/Shifty.Api.csproj", "./Web/Api/"]
COPY ["src/Web/ApiFramework/Shifty.ApiFramework.csproj", "./Web/ApiFramework/"]
COPY ["src/Infrastructure/Persistance/Shifty.Persistence.csproj", "./Infrastructure/Persistance/"]
COPY ["src/Core/Domain/Shifty.Domain.csproj", "./Core/Domain/"]
COPY ["src/Core/Application/Shifty.Application.csproj", "./Core/Application/"]
COPY ["src/Common/Shifty.Common/Shifty.Common.csproj", "./Common/"]
COPY ["src/Common/Shifty.Resources/Shifty.Resources.csproj", "./Resources/"]
# Restore dependencies for all projects
RUN dotnet restore "./Web/Api/" -f 

# Copy the entire source code
COPY . .

# --------------------------------------------------
# 3) Build stage
# --------------------------------------------------
RUN dotnet build "src/Web/Api/Shifty.Api.csproj"  -o /app/build

# --------------------------------------------------
# 4) Publish stage
# --------------------------------------------------
FROM build AS publish
RUN dotnet publish "src/Web/Api/Shifty.Api.csproj" -o /app/publish --no-restore  
# --------------------------------------------------
# 5) Final runtime stage
# --------------------------------------------------
FROM build AS final
WORKDIR /app
EXPOSE 8080
# Copy published output
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Shifty.Api.dll"]
