#!/bin/bash
docker rm -f dotnet-app-3
docker build -t dotnet-app-3 .
docker run -it --name dotnet-app-3 -p 5250:8080 dotnet-app-3

# Run locally:
# APIROOT=http://localhost:5250 dotnet run
