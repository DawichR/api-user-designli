# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY UmaDesignli.sln ./
COPY UmaDesignli.Api/UmaDesignli.Api.csproj UmaDesignli.Api/
COPY UmaDesignli.Application/UmaDesignli.Application.csproj UmaDesignli.Application/
COPY UmaDesignli.Domain/UmaDesignli.Domain.csproj UmaDesignli.Domain/
COPY UmaDesignli.Infrastructure/UmaDesignli.Infrastructure.csproj UmaDesignli.Infrastructure/
COPY UmaDesignli.UnitTest/UmaDesignli.UnitTest.csproj UmaDesignli.UnitTest/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY . .

# Run unit tests with detailed output
WORKDIR /src/UmaDesignli.UnitTest
RUN echo "========================================" && \
    echo "Running Unit Tests..." && \
    echo "========================================" && \
    dotnet test --configuration Release --no-restore --verbosity normal --logger "console;verbosity=detailed" && \
    echo "========================================" && \
    echo "✓ All tests passed successfully!" && \
    echo "========================================"

# Build the application
WORKDIR /src/UmaDesignli.Api
RUN dotnet build -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published files from publish stage
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "UmaDesignli.Api.dll"]
