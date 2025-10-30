# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the .csproj file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the files
COPY . .

# Build and publish the application
RUN dotnet publish -c Release -o out

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published output
COPY --from=build /app/out .

# Expose the port your API listens on (default ASP.NET port is 8080 in containers)
EXPOSE 8080

# Set the entry point (replace 'YourApiProjectName.dll' with your actual DLL name)
ENTRYPOINT ["dotnet", "Server.dll"]
