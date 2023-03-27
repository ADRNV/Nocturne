using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Core.Managers;
using Nocturne.Infrastructure.Caching;
using Nocturne.Infrastructure.Security.Entities;
using Nocturne.InfrastructureTests.Helpers;
using NUnit.Framework;

namespace Nocturne.Infrastructure.Messaging.Tests
{
    [TestFixture()]
    public class ConnectionsManagerTests
    {
        private readonly ConnectionsManager<User> _connectionsManager;

        private readonly UserManager<User> _userManager;

        private readonly IRedisCacheRepository<Connection> _redisCacheRepository;

        private readonly IMapper _mapper;

        public ConnectionsManagerTests()
        {
            _mapper = DependencyHelper.Services.GetRequiredService<IMapper>();

            _redisCacheRepository = DependencyHelper
                .Services.GetRequiredService<IRedisCacheRepository<Connection>>();

            _userManager = DependencyHelper.Services.GetRequiredService<UserManager<User>>();

            _connectionsManager = new ConnectionsManager<User>(
                _redisCacheRepository,
                _userManager
                );
        }

        [Test()]
        public void ConnectionsManagerTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public async Task ConnectTest()
        {
            //arrange
            var user = _mapper.Map<User, Core.Models.User>(DependencyHelper.UsersContextMock
                .First());

            var connection = Guid.NewGuid().ToString();
            //act
            var chacheId = await _connectionsManager.Connect(user, connection);
            //assert
            var inChache = chacheId is not null;

            Assert.IsTrue(inChache);
        }

        [Test()]
        public async Task ConnectNotExistsUserTest()
        {
            //arrange
            var user = new Fixture()
                .Build<Core.Models.User>()
                .With(u => u.UserName, "Not_exists_user")
                .Create();

            var connection = Guid.NewGuid().ToString();

            //act,assert
            Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _connectionsManager.Connect(user, connection));
        }

        [Test()]
        public async Task DisconectAllUserConnectionsTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void DisconectUserConnectionTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void GetUserConnectionsTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void IsConnectedUserTest()
        {
            throw new NotImplementedException();
        }
    }
}