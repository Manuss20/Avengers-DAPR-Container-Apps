[![Build and deploy .NET application to Azure Container App](https://github.com/Manuss20/Avengers-DAPR-Container-Apps/actions/workflows/main.yml/badge.svg)](https://github.com/Manuss20/Avengers-DAPR-Container-Apps/actions/workflows/main.yml)

# Avengers-DAPR-Container-Apps

<p>Avengers Mission Control Sample Application.</p>

<code>dapr run --app-id Avengers --app-port 5000 --dapr-http-port 3500 dotnet run</code>

<code>dapr run --app-id Missions --app-port 4998 --dapr-http-port 3501 dotnet run</code>

<code>dapr run --app-id Payment --app-port 4999 --dapr-http-port 3502 dotnet run</code>

<code>dapr run -a avengers dotnet watch run</code>
