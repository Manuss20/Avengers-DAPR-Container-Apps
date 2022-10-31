# Avengers-DARP-Container-Apps

dapr run --app-id Avengers --app-port 5000 --dapr-http-port 3500 dotnet run
dapr run --app-id Missions --app-port 4998 --dapr-http-port 3501 dotnet run
dapr run --app-id Payment --app-port 4999 --dapr-http-port 3502 dotnet run