using AutoMapper;
using Dotnet.Samples.AspNetCore.WebApi.Controllers;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Dotnet.Samples.AspNetCore.WebApi.Models;
using Dotnet.Samples.AspNetCore.WebApi.Services;
using FluentValidation;
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
        public static (
            Mock<IPlayerService> service,
            Mock<ILogger<PlayerController>> logger,
            Mock<IValidator<PlayerRequestModel>> validator
        ) InitControllerMocks()
        {
            var service = new Mock<IPlayerService>();
            var logger = new Mock<ILogger<PlayerController>>();
            var validator = new Mock<IValidator<PlayerRequestModel>>();

            return (service, logger, validator);
        }

        public static Mock<IUrlHelper> SetupUrlHelperMock()
        {
            var mock = new Mock<IUrlHelper>();
            mock.Setup(url => url.Action(It.IsAny<UrlActionContext>())).Returns(It.IsAny<string>());

            return mock;
        }

        public static (
            Mock<IPlayerRepository> repository,
            Mock<ILogger<PlayerService>> logger,
            Mock<IMemoryCache> memoryCache,
            Mock<IMapper> mapper
        ) InitServiceMocks(object? cacheValue = null)
        {
            var repository = new Mock<IPlayerRepository>();
            var logger = new Mock<ILogger<PlayerService>>();
            var memoryCache = SetupMemoryCacheMock(cacheValue ?? It.IsAny<object>());
            var mapper = new Mock<IMapper>();
            return (repository, logger, memoryCache, mapper);
        }

        public static Mock<IMemoryCache> SetupMemoryCacheMock(object? value)
        {
            var cachedValue = false;
            var mock = new Mock<IMemoryCache>();

            mock.Setup(cache => cache.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny!))
                .Callback(
                    new TryGetValueDelegate(
                        (object _, out object? output) =>
                        {
                            output = cachedValue ? value : null;
                        }
                    )
                )
                .Returns(() =>
                {
                    bool hasValue = cachedValue;
                    cachedValue = true; // Subsequent invocations will return true
                    return hasValue;
                });

            mock.Setup(cache => cache.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);
            mock.Setup(cache => cache.Remove(It.IsAny<object>()));

            return mock;
        }

        private delegate void TryGetValueDelegate(object key, out object? value);
    }
}
