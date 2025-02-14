# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["dotnet-app-3.csproj", "./"]
RUN dotnet restore "dotnet-app-3.csproj"

# Copy the remaining files and build/publish the app
COPY . .
RUN dotnet publish "dotnet-app-3.csproj" -c Release -o /app/publish

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port the app listens on (default is 80)
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "dotnet-app-3.dll"]
