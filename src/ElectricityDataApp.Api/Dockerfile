#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ElectricityDataApp.Api/ElectricityDataApp.Api.csproj", "src/ElectricityDataApp.Api/"]
COPY ["src/ElectricityDataApp.Infrastructure/ElectricityDataApp.Infrastructure.csproj", "src/ElectricityDataApp.Infrastructure/"]
COPY ["src/ElectricityDataApp.Application/ElectricityDataApp.Application.csproj", "src/ElectricityDataApp.Application/"]
COPY ["src/ElectricityDataApp.DataParser/ElectricityDataApp.DataParser.csproj", "src/ElectricityDataApp.DataParser/"]
COPY ["src/ElectricityDataApp.Domain/ElectricityDataApp.Domain.csproj", "src/ElectricityDataApp.Domain/"]
RUN dotnet restore "src/ElectricityDataApp.Api/ElectricityDataApp.Api.csproj"
COPY . .
WORKDIR "/src/src/ElectricityDataApp.Api"
RUN dotnet build "ElectricityDataApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ElectricityDataApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ElectricityDataApp.Api.dll"]