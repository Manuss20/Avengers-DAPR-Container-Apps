FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/src/Avengers/Avengers.csproj", "Avengers/"]
COPY ["/src/Monitoring/Monitoring.csproj", "Monitoring/"]
RUN dotnet restore "src/Avengers/Avengers.csproj"

COPY . .
WORKDIR "/src/Avengers/"
RUN dotnet build "Avengers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Avengers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Avengers.dll"]