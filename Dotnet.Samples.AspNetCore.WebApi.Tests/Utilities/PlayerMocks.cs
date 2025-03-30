using Dotnet.Samples.AspNetCore.WebApi.Controllers;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests.Utilities
{
    /// <summary>
    /// A Mock is a dynamic test double that not only provides responses but
    /// also records interactions. Mocks are pre‑programmed with expectations
    /// (like “this method should be called once with these parameters”) and can
    /// later verify that the expected calls occurred. Frameworks such as Moq
    /// make it easy to set up and verify these interactions.
    /// </summary>
    public static class PlayerMocks
    {
        public static Mock<IPlayerService> ServiceMock()
        {
            return new Mock<IPlayerService>();
        }

        public static Mock<IPlayerRepository> RepositoryMock()
        {
            return new Mock<IPlayerRepository>();
        }

        public static Mock<ILogger<T>> LoggerMock<T>()
            where T : class
        {
            return new Mock<ILogger<T>>();
        }

        public static Mock<IMemoryCache> MemoryCacheMock(object? value)
        {
            var fromCache = false;
            var mock = new Mock<IMemoryCache>();
            mock.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out value))
                .Returns(() =>
                {
                    bool hasValue = fromCache;
                    fromCache = true; // Subsequent invocations will return true
                    return hasValue;
                });
            mock.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);
            mock.Setup(cache => cache.Remove(It.IsAny<object>()));

            return mock;
        }

        public static Mock<IUrlHelper> UrlHelperMock()
        {
            var mock = new Mock<IUrlHelper>();
            mock.Setup(url => url.Action(It.IsAny<UrlActionContext>())).Returns(It.IsAny<string>());

            return mock;
        }

        public static (
            Mock<IPlayerRepository> repository,
            Mock<ILogger<PlayerService>> logger,
            Mock<IMemoryCache> memoryCache
        ) SetupServiceMocks(object? cacheValue = null)
        {
            var repository = RepositoryMock();
            var logger = LoggerMock<PlayerService>();
            var memoryCache = MemoryCacheMock(cacheValue ?? It.IsAny<object>());
            return (repository, logger, memoryCache);
        }

        public static (
            Mock<IPlayerService> service,
            Mock<ILogger<PlayerController>> logger
        ) SetupControllerMocks(object? cacheValue = null)
        {
            var service = ServiceMock();
            var logger = LoggerMock<PlayerController>();
            return (service, logger);
        }
    }
}
