using Microsoft.EntityFrameworkCore;
using Nocturne.Core.Repositories;
using Nocturne.Infrastructure.Messaging.Models;
using Nocturne.Infrastructure.Securiry;

namespace Nocturne.Infrastructure.Messaging.Storage
{
    public class MessagesRepository : IMessagesRepository<Message>
    {
        private readonly UsersContext _usersContext;

        public MessagesRepository(UsersContext usersContext)
        {
            _usersContext = usersContext;
        }

        public async Task<Guid> CreateMessage(Guid userId, Message message)
        {
            var user = await _usersContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                message.User = user;

                _usersContext.Messages.Add(message);

                _usersContext.Messages.Entry(message).State = EntityState.Added;

                await _usersContext.SaveChangesAsync();

                return message.Id;
            }
            else
            {
                throw new InvalidOperationException("User not found");
            }

        }

        public async Task<bool> RemoveMessage(Guid userId, Message message)
        {
            var user = await _usersContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var removeMessage = await _usersContext.Messages.Where(m => m.Id == message.Id)
                .FirstAsync();

            if (user is not null && removeMessage is not null)
            {
                message.User = user;

                _usersContext.Messages.Remove(removeMessage);

                _usersContext.Messages.Entry(message).State = EntityState.Added;

                await _usersContext.SaveChangesAsync();

                return await _usersContext.Messages.FindAsync(new object[] { message.Id }) is null;
            }
            else
            {
                throw new InvalidOperationException("User not found");
            }

        }
    }
}
