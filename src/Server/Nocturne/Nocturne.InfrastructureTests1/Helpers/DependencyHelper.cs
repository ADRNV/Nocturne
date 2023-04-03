using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nocturne.Infrastructure.Caching;
using Nocturne.Infrastructure.Security.Entities;
using Nocturne.Infrastructure.Security.MappersConfiguration;
using System.Reflection;

namespace Nocturne.InfrastructureTests.Helpers
{
    internal static class DependencyHelper
    {
        public static IServiceProvider Services { get; }

        private static Mock<IRedisCacheRepository<Connection>> _redisCacheRepositoryMock { get; }

        private static Mock<UserManager<User>> _userManagerMock { get; }

        public static IEnumerable<User> UsersContextMock { get; set; } = new Fixture()
            .Build<User>()
            .Without(u => u.UserGroups)
            .With(u => u.UserName)
            .CreateMany();

        public static IList<Connection> Connections { get; set; } = new Fixture()
            .Build<Connection>()
            .With(c => c.ConnectionId, Guid.NewGuid().ToString())
            .CreateMany()
            .ToList();

        static DependencyHelper()
        {
            _redisCacheRepositoryMock = new Mock<IRedisCacheRepository<Connection>>();

            _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(),
                null, null, null, null, null, null, null, null);

            Services = new ServiceCollection()
                .AddAutoMapper(c =>
                {
                    c.AddProfile<IdentityUserMappingProfile>();
                }, Assembly.GetExecutingAssembly())
                .AddSingleton(_redisCacheRepositoryMock)
                .AddSingleton(_userManagerMock)
                .BuildServiceProvider();
        }
    }
}
