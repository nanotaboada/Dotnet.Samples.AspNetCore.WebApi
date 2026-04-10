using System.Data.Common;
using System.Net;
using System.Net.Http.Json;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Tests.Utilities;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests.Integration;

/// <summary>
/// Integration tests for the player endpoints exposed by the web application.
/// Each test exercises the full ASP.NET Core request pipeline — routing,
/// middleware, validation, serialization — via <see cref="WebApplicationFactory{TEntryPoint}"/>
/// backed by an in-memory SQLite database. The factory is created fresh per test
/// (via <see cref="IAsyncLifetime"/>) to ensure state isolation.
/// </summary>
public class PlayerWebApplicationTests : IAsyncLifetime
{
    private WebApplicationFactory<Program> _factory = default!;
    private WebApplicationFactory<Program> _authorizedFactory = default!;
    private HttpClient _client = default!;
    private HttpClient _authorizedClient = default!;
    private DbConnection _connection = default!;

    public Task InitializeAsync()
    {
        var (connection, _) = DatabaseFakes.CreateSqliteConnection();
        _connection = connection;

        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                // AddDbContextPool registers three service types. All three must be
                // removed before re-registering with AddDbContext; leaving any one
                // causes startup to fail when the pool tries to resolve its options.
                var descriptors = services
                    .Where(d =>
                        d.ServiceType == typeof(DbContextOptions<PlayerDbContext>)
                        || d.ServiceType == typeof(PlayerDbContext)
                        || (
                            d.ServiceType.IsGenericType
                            && d.ServiceType.GetGenericArguments().Length > 0
                            && d.ServiceType.GetGenericArguments()[0] == typeof(PlayerDbContext)
                        )
                    )
                    .ToList();

                foreach (var descriptor in descriptors)
                    services.Remove(descriptor);

                services.AddDbContext<PlayerDbContext>(options => options.UseSqlite(_connection));
            });
        });

        _client = _factory.CreateClient();

        // Derived factory that layers authentication on top of the base factory.
        // Used only by tests targeting endpoints protected with [Authorize].
        // Keeping auth out of the base factory means unauthenticated tests will
        // fail if an endpoint accidentally becomes protected.
        _authorizedFactory = _factory.WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
                services
                    .AddAuthentication(TestAuthHandler.SchemeName)
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        TestAuthHandler.SchemeName,
                        _ => { }
                    )
            )
        );

        _authorizedClient = _authorizedFactory.CreateClient();
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        _authorizedClient.Dispose();
        _client.Dispose();
        await _authorizedFactory.DisposeAsync();
        await _factory.DisposeAsync();
        await _connection.DisposeAsync();
    }

    /* -------------------------------------------------------------------------
     * GET /players
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Get_Players_Existing_Returns200Ok()
    {
        // Act
        var response = await _client.GetAsync("/players");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var players = await response.Content.ReadFromJsonAsync<List<PlayerResponseModel>>();
        players.Should().HaveCount(26);
    }

    /* -------------------------------------------------------------------------
     * GET /players/{id:Guid}
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Get_PlayerById_Existing_Returns200Ok()
    {
        // Arrange — resolve a real ID from the seeded database
        await using var scope = _authorizedFactory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<PlayerDbContext>();
        var existingId = (await db.Players.FirstAsync()).Id;

        // Act
        var response = await _authorizedClient.GetAsync($"/players/{existingId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var player = await response.Content.ReadFromJsonAsync<PlayerResponseModel>();
        player.Should().NotBeNull();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Get_PlayerById_Unknown_Returns404NotFound()
    {
        // Act
        var response = await _authorizedClient.GetAsync($"/players/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Status.Should().Be(StatusCodes.Status404NotFound);
        problem.Title.Should().Be("Not Found");
    }

    /* -------------------------------------------------------------------------
     * GET /players/squadNumber/{squadNumber:int}
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Get_PlayerBySquadNumber_Existing_Returns200Ok()
    {
        // Act
        var response = await _client.GetAsync("/players/squadNumber/23");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var player = await response.Content.ReadFromJsonAsync<PlayerResponseModel>();
        player.Should().NotBeNull();
        player!.Dorsal.Should().Be(23);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Get_PlayerBySquadNumber_Unknown_Returns404NotFound()
    {
        // Act
        var response = await _client.GetAsync("/players/squadNumber/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Status.Should().Be(StatusCodes.Status404NotFound);
        problem.Title.Should().Be("Not Found");
    }

    /* -------------------------------------------------------------------------
     * POST /players
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Post_Players_Nonexistent_Returns201Created()
    {
        // Arrange — squad number 27 (Lo Celso) is not in the seeded data
        var request = PlayerFakes.MakeRequestModelForCreate();

        // Act
        var response = await _client.PostAsJsonAsync("/players", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var player = await response.Content.ReadFromJsonAsync<PlayerResponseModel>();
        player.Should().NotBeNull();
        player!.Dorsal.Should().Be(request.SquadNumber);
    }

    // Note: the controller's 409 Conflict branch (squad number already exists) is
    // unreachable via the HTTP pipeline. The "Create" validation rule set includes
    // BeUniqueSquadNumber, which returns a 400 validation error before the
    // controller's own duplicate check ever runs. The 409 path is covered by the
    // unit test Post_Players_Existing_Returns409Conflict, where validation is mocked
    // to pass so the controller logic can be exercised in isolation.

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Post_Players_ValidationError_Returns400BadRequest()
    {
        // Arrange — SquadNumber 0 is the int default, fails NotEmpty
        var request = PlayerFakes.MakeRequestModelForCreate();
        request.SquadNumber = 0;

        // Act
        var response = await _client.PostAsJsonAsync("/players", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Status.Should().Be(StatusCodes.Status400BadRequest);
    }

    /* -------------------------------------------------------------------------
     * PUT /players/squadNumber/{squadNumber:int}
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Put_PlayerBySquadNumber_Existing_Returns204NoContent()
    {
        // Arrange
        var request = PlayerFakes.MakeRequestModelForUpdate(23);

        // Act
        var response = await _client.PutAsJsonAsync("/players/squadNumber/23", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Put_PlayerBySquadNumber_Unknown_Returns404NotFound()
    {
        // Arrange — squad number 999 does not exist; body matches route to avoid mismatch error
        var request = PlayerFakes.MakeRequestModelForUpdate(23);
        request.SquadNumber = 999;

        // Act
        var response = await _client.PutAsJsonAsync("/players/squadNumber/999", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Status.Should().Be(StatusCodes.Status404NotFound);
        problem.Title.Should().Be("Not Found");
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Put_PlayerBySquadNumber_ValidationError_Returns400BadRequest()
    {
        // Arrange — SquadNumber -1 fails GreaterThan(0)
        var request = PlayerFakes.MakeRequestModelForUpdate(23);
        request.SquadNumber = -1;

        // Act
        var response = await _client.PutAsJsonAsync("/players/squadNumber/23", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Status.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Put_PlayerBySquadNumber_SquadNumberMismatch_Returns400BadRequest()
    {
        // Arrange — body squad number (99) differs from route (23); both are valid values
        // so validation passes and the mismatch guard in the controller fires instead
        var request = PlayerFakes.MakeRequestModelForUpdate(23);
        request.SquadNumber = 99;

        // Act
        var response = await _client.PutAsJsonAsync("/players/squadNumber/23", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Status.Should().Be(StatusCodes.Status400BadRequest);
        problem.Title.Should().Be("Bad Request");
    }

    /* -------------------------------------------------------------------------
     * DELETE /players/squadNumber/{squadNumber:int}
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Delete_PlayerBySquadNumber_Existing_Returns204NoContent()
    {
        // Act
        var response = await _client.DeleteAsync("/players/squadNumber/23");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Delete_PlayerBySquadNumber_Unknown_Returns404NotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/players/squadNumber/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problem!.Status.Should().Be(StatusCodes.Status404NotFound);
        problem.Title.Should().Be("Not Found");
    }

    /* -------------------------------------------------------------------------
     * GET /health
     * ---------------------------------------------------------------------- */

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Get_Health_Healthy_Returns200Ok()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
