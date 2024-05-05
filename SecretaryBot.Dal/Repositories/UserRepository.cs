using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretaryBot.Dal.Models;
using SecretaryBot.Domain.Abstractions.Repositories;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Dal.Repositories
{
    public class UserRepository(IMapper mapper, PostgresContext context) : IUserRepository
    {
        private readonly IMapper _mapper = mapper;
        private readonly PostgresContext _context = context;

        public async Task AddUserAsync(User user)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.TelegramId == user.TelegramId);
            if (currentUser != null)
                return;

            var dalUser = _mapper.Map<DalUser>(user);
            _context.Users.Add(dalUser);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeAccessAsync(long telegramId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.TelegramId == telegramId);
            if (user != null)
            {
                user.HasAccess = !user.HasAccess;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CheckAccessAsync(long telegramId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.TelegramId == telegramId);
            return user != null && user.HasAccess;
        }

        public async Task GiveAccessAsync(long telegramId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.TelegramId == telegramId);
            if (user is null)
                return;
            user.HasAccess = !user.HasAccess;
            await _context.SaveChangesAsync();
        }
    }
}
