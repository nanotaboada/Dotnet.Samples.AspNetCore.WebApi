using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests
{
    public static class PlayerMocks
    {
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
