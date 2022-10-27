FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/src/Avengers.PaymentApi/Avengers.PaymentApi.csproj", "Avengers.PaymentApi/"]
COPY ["/src/Monitoring/Monitoring.csproj", "Monitoring/"]
RUN dotnet restore "Avengers.PaymentApi/Avengers.PaymentApi.csproj"
COPY ["/src/Avengers.PaymentApi/", "Avengers.PaymentApi/"]
COPY ["/src/Monitoring/", "Monitoring/"]
WORKDIR "/src/Avengers.PaymentApi"
RUN dotnet build "Avengers.PaymentApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Avengers.PaymentApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Avengers.PaymentApi.dll"]