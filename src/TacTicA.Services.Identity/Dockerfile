FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TacTicA.Services.Identity/TacTicA.Services.Identity.csproj", "src/TacTicA.Services.Identity/"]
COPY ["src/TacTicA.Common/TacTicA.Common.csproj", "src/TacTicA.Common/"]
RUN dotnet restore "./src/TacTicA.Services.Identity/TacTicA.Services.Identity.csproj"
COPY . .
WORKDIR "/src/src/TacTicA.Services.Identity"
RUN dotnet build "./TacTicA.Services.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TacTicA.Services.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TacTicA.Services.Identity.dll"]