# Use the official .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the .csproj files
COPY OrderService/OrderService.csproj ./OrderService/
COPY OrderDBContext/OrderDBContext.csproj ./OrderDBContext/
COPY OrderModels/OrderModels.csproj ./OrderModels/

# Restore dependencies
RUN dotnet restore ./OrderService/OrderService.csproj

# Copy the entire project directories
COPY OrderService/. ./OrderService/
COPY OrderDBContext/. ./OrderDBContext/
COPY OrderModels/. ./OrderModels/

# Build the application
RUN dotnet publish ./OrderService/OrderService.csproj -c Release -o out

# Use the official ASP.NET runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Set the working directory
WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /app/out .

# Expose the port the app runs on
EXPOSE 81

# Command to run the application
ENTRYPOINT ["dotnet", "OrderService.dll"]
#ENTRYPOINT ["./OrderService"] 
