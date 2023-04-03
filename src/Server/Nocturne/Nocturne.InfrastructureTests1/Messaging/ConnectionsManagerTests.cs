using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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

        private readonly Mock<UserManager<User>> _userManager;

        private readonly Mock<IRedisCacheRepository<Connection>> _redisCacheRepository;

        private readonly IMapper _mapper;

        public ConnectionsManagerTests()
        {
            _redisCacheRepository = DependencyHelper
                .Services.GetRequiredService<Mock<IRedisCacheRepository<Connection>>>();

            _userManager = DependencyHelper.Services.GetRequiredService<Mock<UserManager<User>>>();

            _redisCacheRepository.Setup(r => r.Insert(It.IsAny<Connection>()))
                .ReturnsAsync((Connection c) =>
                {
                    DependencyHelper.Connections.Add(c);

                    return c.Id;
                });

            _redisCacheRepository.Setup(r => r.Delete(It.IsAny<Connection>()))
                 .Returns((Connection c) =>
                 {
                     DependencyHelper.Connections.Remove(c);

                     return Task.CompletedTask;
                 });

            
            _userManager.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string name) => DependencyHelper.UsersContextMock
                    .Where(u => u.UserName == name)
                    .First()
                );
            
            _mapper = DependencyHelper.Services.GetRequiredService<IMapper>();

            _connectionsManager = new ConnectionsManager<User>(
                _redisCacheRepository.Object,
                _userManager.Object
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
            var inChache = DependencyHelper.Connections.Where(c => c.Id == chacheId)
                .FirstOrDefault() is not null;

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
            var idUser = Guid.NewGuid();

            var user = new Fixture()
                .Build<Core.Models.User>()
                .With(u => u.Id, idUser)
                .Create();

            var connections = new Fixture()
                .Build<Infrastructure.Security.Entities.Connection>()
                .With(c => c.UserId, idUser);

            DependencyHelper.Connections.AddMany(() => connections.Create(), 3);

            await _connectionsManager.Disconect(user);
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