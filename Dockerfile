# Multi-stage build for .NET Core API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory
WORKDIR /src

# Copy solution file
COPY Practice_Quiz_Generator.sln ./

# Copy project files
COPY Practice_Quiz_Generator/Practice_Quiz_Generator.csproj Practice_Quiz_Generator/
COPY Practice_Quiz_Generator.Application/Practice_Quiz_Generator.Application.csproj Practice_Quiz_Generator.Application/
COPY Practice_Quiz_Generator.Domain/Practice_Quiz_Generator.Domain.csproj Practice_Quiz_Generator.Domain/
COPY Practice_Quiz_Generator.Infrastructure/Practice_Quiz_Generator.Infrastructure.csproj Practice_Quiz_Generator.Infrastructure/
COPY Practice_Quiz_Generator.Shared/Practice_Quiz_Generator.Shared.csproj Practice_Quiz_Generator.Shared/
COPY Practice_Quiz_Generator.Test/Practice_Quiz_Generator.Test.csproj Practice_Quiz_Generator.Test/

# Restore dependencies with retry and increased timeout
RUN dotnet restore --disable-parallel /p:RestoreDisableParallel=true /p:RestoreTimeout=1000000

# Copy source code
COPY . .

# Clean and restore with increased timeout
RUN dotnet nuget locals all --clear && \
    dotnet restore --disable-parallel /p:RestoreDisableParallel=true /p:RestoreTimeout=1000000

# Build the application
RUN dotnet build -c Release --no-restore

# Publish the application
RUN dotnet publish Practice_Quiz_Generator/Practice_Quiz_Generator.csproj -c Release -o /app/publish --no-restore

# Production stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set working directory
WORKDIR /app

# Install wget for healthcheck compatibility
RUN apt-get update && apt-get install -y --no-install-recommends wget && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=build /app/publish .

# Create a non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Start the application
ENTRYPOINT ["dotnet", "Practice_Quiz_Generator.dll"]
