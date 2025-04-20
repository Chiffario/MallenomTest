FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
RUN mkdir -p /home/app/storage
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MallenomTest.Server/MallenomTest.Server.csproj", "MallenomTest.Contracts/MallenomTest.Contracts.csproj", "./"]
RUN dotnet restore "MallenomTest.Server.csproj"
COPY . .
WORKDIR "/src/MallenomTest.Server"
RUN dotnet build "MallenomTest.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MallenomTest.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MallenomTest.Server.dll"]
