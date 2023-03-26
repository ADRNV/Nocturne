using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Core.Managers;
using Nocturne.Infrastructure.Security.Entities;
using Nocturne.InfrastructureTests.Helpers;
using NUnit.Framework;

namespace Nocturne.Infrastructure.Messaging.Tests
{
    [TestFixture()]
    public class ConnectionsManagerTests
    {
        public readonly ConnectionsManager<User> _connectionsManager;

        public readonly UserManager<User> _userManager;

        private readonly IMapper _mapper;

        public ConnectionsManagerTests()
        {
            _connectionsManager = (ConnectionsManager<User>)
                DependencyHelper.Services
                .GetRequiredService<IConnectionsManager>();

            _mapper = DependencyHelper.Services.GetRequiredService<IMapper>();
        }

        [Test()]
        public void ConnectionsManagerTest()
        {
        }

        [Test()]
        public async Task ConnectTest()
        {
            var user = _mapper.Map<User, Core.Models.User>(DependencyHelper.UsersContextMock
                .First());

            await _connectionsManager.Connect(user, Guid.NewGuid().ToString());
        }

        [Test()]
        public void DisconectTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void DisconectTest1()
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