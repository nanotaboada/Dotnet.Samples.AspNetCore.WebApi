# - Stage 1 --------------------------------------------------------------------

    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /src

    # Copy and restore dependencies
    COPY src/Dotnet.Samples.AspNetCore.WebApi/*.csproj ./Dotnet.Samples.AspNetCore.WebApi/
    RUN dotnet restore ./Dotnet.Samples.AspNetCore.WebApi

    # Copy source and publish
    COPY src/Dotnet.Samples.AspNetCore.WebApi ./Dotnet.Samples.AspNetCore.WebApi
    WORKDIR /src/Dotnet.Samples.AspNetCore.WebApi
    RUN dotnet publish -c Release -o /app/publish

# - Stage 2 --------------------------------------------------------------------

    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
    WORKDIR /app

    # Copy published output
    # Note: This includes the SQLite database because it's marked as <Content> with
    # <CopyToOutputDirectory> in the .csproj file. No need to copy it manually.
    COPY --from=build /app/publish .

    # Add non-root user (aspnetcore) for security hardening
    RUN adduser --disabled-password --gecos '' aspnetcore \
        && chown -R aspnetcore:aspnetcore /app
    USER aspnetcore

    # Set environment variables
    ENV ASPNETCORE_URLS=http://+:9000
    ENV ASPNETCORE_ENVIRONMENT=Production

    # Default entrypoint
    ENTRYPOINT ["dotnet", "Dotnet.Samples.AspNetCore.WebApi.dll"]
