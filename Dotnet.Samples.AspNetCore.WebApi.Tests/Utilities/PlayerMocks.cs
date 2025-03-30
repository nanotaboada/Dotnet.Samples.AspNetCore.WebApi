using Dotnet.Samples.AspNetCore.WebApi.Data;
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
        public static Mock<IPlayerRepository> RepositoryMock()
        {
            var mock = new Mock<IPlayerRepository>();

            return mock;
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
    }
}
