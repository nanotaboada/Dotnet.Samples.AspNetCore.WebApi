# ------------------------------------------------------------------------------
# Stage 1: Builder
# This stage builds the application and its dependencies.
# ------------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /src

# Restore dependencies
COPY src/Dotnet.Samples.AspNetCore.WebApi/*.csproj  ./Dotnet.Samples.AspNetCore.WebApi/
RUN dotnet restore ./Dotnet.Samples.AspNetCore.WebApi

# Copy source code and pre-seeded SQLite database
COPY src/Dotnet.Samples.AspNetCore.WebApi/          ./Dotnet.Samples.AspNetCore.WebApi/

WORKDIR /src/Dotnet.Samples.AspNetCore.WebApi

# Build solution and publish release
RUN dotnet publish -c Release -o /app/publish

# ------------------------------------------------------------------------------
# Stage 2: Runtime
# This stage creates the final, minimal image to run the application.
# ------------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

# Install curl for health check
RUN apt-get update && apt-get install -y --no-install-recommends curl && \
    rm -rf /var/lib/apt/lists/*

# Metadata labels for the image. These are useful for registries and inspection.
LABEL org.opencontainers.image.title="🧪 Web API made with .NET 10 (LTS) and ASP.NET Core"
LABEL org.opencontainers.image.description="Proof of Concept for a Web API made with .NET 10 (LTS) and ASP.NET Core"
LABEL org.opencontainers.image.licenses="MIT"
LABEL org.opencontainers.image.source="https://github.com/nanotaboada/Dotnet.Samples.AspNetCore.WebApi"

# Set environment variables
ENV ASPNETCORE_URLS=http://+:9000
ENV ASPNETCORE_ENVIRONMENT=Production

# Copy published app from builder
COPY --from=builder     /app/publish/               .

# Copy metadata docs for container registries (e.g.: GitHub Container Registry)
COPY --chmod=444        README.md                   ./

# https://rules.sonarsource.com/docker/RSPEC-6504/

# Copy entrypoint and healthcheck scripts
COPY --chmod=555        scripts/entrypoint.sh       ./entrypoint.sh
COPY --chmod=555        scripts/healthcheck.sh      ./healthcheck.sh
# The 'hold' is our storage compartment within the image. Here, we copy a
# pre-seeded SQLite database file, which Compose will mount as a persistent
# 'storage' volume when the container starts up.
COPY --from=builder /src/Dotnet.Samples.AspNetCore.WebApi/storage/players-sqlite3.db ./hold/players-sqlite3.db

# Add non-root user and make volume mount point writable
RUN groupadd -r aspnetcore && useradd -r -g aspnetcore aspnetcore && \
    mkdir -p /storage && \
    chown aspnetcore:aspnetcore /storage

USER aspnetcore

HEALTHCHECK --interval=30s --timeout=5s --start-period=5s --retries=3 \
    CMD ["./healthcheck.sh"]

EXPOSE 9000

ENTRYPOINT ["./entrypoint.sh"]
CMD ["dotnet", "Dotnet.Samples.AspNetCore.WebApi.dll"]
