#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LePickaProducts.API/LePickaProducts.API.csproj", "LePickaProducts.API/"]
COPY ["LePickaProducts.Application/LePickaProducts.Application.csproj", "LePickaProducts.Application/"]
COPY ["LePickaProducts.Domain/LePickaProducts.Domain.csproj", "LePickaProducts.Domain/"]
COPY ["LePicka.Shared/LePicka.Shared.csproj", "LePicka.Shared/"]
COPY ["LePickaProducts.Infrastructure/LePickaProducts.Infrastructure.csproj", "LePickaProducts.Infrastructure/"]
RUN dotnet restore "./LePickaProducts.API/./LePickaProducts.API.csproj"
COPY . .
WORKDIR "/src/LePickaProducts.API"
RUN dotnet build "./LePickaProducts.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LePickaProducts.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LePickaProducts.API.dll"]