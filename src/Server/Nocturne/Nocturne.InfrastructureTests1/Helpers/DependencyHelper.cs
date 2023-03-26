using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nocturne.Infrastructure.Caching;
using Nocturne.Infrastructure.Security.Entities;
using AutoFixture;
using Nocturne.Infrastructure.Messaging;
using Nocturne.Core.Managers;
using System.Reflection;
using Nocturne.Infrastructure.Security.MappersConfiguration;

namespace Nocturne.InfrastructureTests.Helpers
{
    internal static class DependencyHelper
    {
        public static IServiceProvider Services { get; }

        private static Mock<IRedisCacheRepository<Connection>> _redisCacheRepositoryMock { get; }

        private static Mock<UserManager<User>> _userManagerMock { get; }

        private static ConnectionsManager<User> _connectionsManager { get; set; }

        public static IEnumerable<User> UsersContextMock { get; set; } = new Fixture()
            .Build<User>()
            .Without(u => u.UserGroups)
            .With(u => u.UserName)
            .CreateMany();

        static DependencyHelper()
        {
            _redisCacheRepositoryMock = new Mock<IRedisCacheRepository<Connection>>();

            _redisCacheRepositoryMock.Setup(r => r.Insert(It.IsAny<Connection>()))
                .ReturnsAsync(() =>
                Guid
                .NewGuid()
                .ToString());

           _redisCacheRepositoryMock.Setup(r => r.Delete(It.IsAny<Connection>()))
                .Returns(Task.CompletedTask);

            _userManagerMock = new Mock<UserManager<User>>();

            _userManagerMock.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string name) => UsersContextMock
                    .Where(u => u.UserName == name)
                    .First()
                );
                
            Services = new ServiceCollection()
                .AddAutoMapper(c => 
                {
                    c.AddProfile<IdentityUserMappingProfile>();
                }, Assembly.GetExecutingAssembly())
                .AddSingleton(_redisCacheRepositoryMock.Object)
                .AddSingleton(_userManagerMock.Object)
                .AddSingleton<IConnectionsManager>()
                .BuildServiceProvider();
        }
    }
}
