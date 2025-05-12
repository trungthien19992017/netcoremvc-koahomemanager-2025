# Base image cho runtime ASP.NET (sử dụng khi chạy thật)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["KOAHome.csproj", "."]
RUN dotnet restore "./KOAHome.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./KOAHome.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./KOAHome.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
EXPOSE 5000
# Thêm thư mục cho DataProtection keys
RUN mkdir -p /app/data-protection-keys
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "KOAHome.dll"]
