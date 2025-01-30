# --------------------------------------------------
# 1) Base runtime image
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# نصب گواهینامه‌های مورد نیاز برای دسترسی به NuGet
RUN apt-get update && apt-get install -y ca-certificates && update-ca-certificates

# --------------------------------------------------
# 2) Build + Restore stage
# --------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# نصب گواهینامه‌ها در محیط Alpine
RUN apt-get update && apt-get install -y ca-certificates && update-ca-certificates

# کپی فایل‌های پروژه
COPY ["src/Web/Api/Shifty.Api.csproj", "./Web/Api/"]
COPY ["src/Web/ApiFramework/Shifty.ApiFramework.csproj", "./Web/ApiFramework/"]
COPY ["src/Infrastructure/Persistance/Shifty.Persistence.csproj", "./Infrastructure/Persistance/"]
COPY ["src/Infrastructure/RequestHandlers/Shifty.RequestHandlers.csproj", "./Infrastructure/RequestHandlers/"]
COPY ["src/Core/Domain/Shifty.Domain.csproj", "./Core/Domain/"]
COPY ["src/Core/Application/Shifty.Application.csproj", "./Core/Application/"]
COPY ["src/Common/Shifty.Common/Shifty.Common.csproj", "./Common/"]
COPY ["src/Common/Shifty.Resources/Shifty.Resources.csproj", "./Resources/"]

# تست اتصال به NuGet قبل از restore
RUN wget --no-check-certificate https://api.nuget.org/v3/index.json

RUN dotnet restore "./Web/Api/Shifty.Api.csproj" -f 

# کپی کل سورس
COPY . .

# --------------------------------------------------
# 3) Build stage
# --------------------------------------------------
RUN dotnet build "src/Web/Api/Shifty.Api.csproj" -o /app/build

# --------------------------------------------------
# 4) Publish stage
# --------------------------------------------------
FROM build AS publish
RUN dotnet publish "src/Web/Api/Shifty.Api.csproj" -o /app/publish --no-restore  

# --------------------------------------------------
# 5) Final runtime stage
# --------------------------------------------------
FROM base AS final
WORKDIR /app
EXPOSE 8080

# کپی خروجی منتشر شده
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Shifty.Api.dll"]
