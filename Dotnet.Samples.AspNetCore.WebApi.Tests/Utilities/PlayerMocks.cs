using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests
{
    public static class PlayerMocks
    {
        public static ILogger<T> LoggerMock<T>()
            where T : class
        {
            var mock = new Mock<ILogger<T>>();
            return mock.Object;
        }

        public static IMemoryCache MemoryCacheMock(object? value)
        {
            var mock = new Mock<IMemoryCache>();
            var fromCache = false;

            mock.Setup(x => x.TryGetValue(It.IsAny<object>(), out value))
                .Returns(() =>
                {
                    bool hasValue = fromCache;
                    fromCache = true; // Subsequent invocations will return true
                    return hasValue;
                });

            mock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);
            return mock.Object;
        }
    }
}
