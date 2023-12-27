using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretaryBot.Dal.Models;
using SecretaryBot.Domain.Abstractions.Repositories;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Dal.Repositories
{
    public class UserRepository : DbContext, IUserRepository
    {
        private readonly IMapper _mapper;
        private DbSet<DalUser> Users { get; set; }

        public UserRepository(DbContextOptions<UserRepository> options, IMapper mapper) : base(options)
        {
            _mapper = mapper;
        }

        public async Task AddUserAsync(User user)
        {
            var currentUser = await Users.FirstOrDefaultAsync(x => x.TelegramId == user.TelegramId);
            if (currentUser != null)
                return;

            var dalUser = _mapper.Map<DalUser>(user);
            Users.Add(dalUser);
            await SaveChangesAsync();
        }

        public async Task ChangeAccessAsync(long telegramId)
        {
            var user = await Users.FirstOrDefaultAsync(x => x.TelegramId == telegramId);
            if (user != null)
            {
                user.HasAccess = !user.HasAccess;
                await SaveChangesAsync();
            }
        }

        public async Task<bool> CheckAccessAsync(long telegramId)
        {
            var user = await Users.FirstOrDefaultAsync(x => x.TelegramId == telegramId);
            return user != null && user.HasAccess;
        }

        public async Task GiveAccessAsync(long telegramId)
        {
            var user = await Users.FirstOrDefaultAsync(x => x.TelegramId == telegramId);
            if (user is null)
                return;
            user.HasAccess = !user.HasAccess;
            await SaveChangesAsync();
        }
    }
}
