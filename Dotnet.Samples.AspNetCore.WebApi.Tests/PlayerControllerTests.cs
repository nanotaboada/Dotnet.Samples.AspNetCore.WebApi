using System.Data.Common;
using Dotnet.Samples.AspNetCore.WebApi.Controllers;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests;

public class PlayerControllerTests
{
    [Fact]
    [Trait("Category", "HTTP GET")]
    public async Task GivenHttpGet_WhenRequestPathHasNoId_ThenResponseStatusCodeShouldBeListOfPlayers()
    {
        // Arrange
        var players = PlayerDataBuilder.SeedWithStarting11();
        var logger = new Mock<ILogger<PlayersController>>();
        var service = new Mock<IPlayerService>();
        service.Setup(service => service.RetrieveAsync()).Returns(Task.FromResult(players));

        var controller = new PlayersController(service.Object, logger.Object);

        // Act
        var response = await controller.GetAsync();

        // Assert
        service.Verify(service => service.RetrieveAsync(), Times.Exactly(1));
        response.Should().BeOfType<Ok<List<Player>>>();
        var result = response as Ok<List<Player>>;
        result?.StatusCode.Should().Be(StatusCodes.Status200OK);
        result?.Value.Should().BeEquivalentTo(players);
    }
}
