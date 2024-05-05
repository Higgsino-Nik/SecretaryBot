using SecretaryBot.Domain.Abstractions.Repositories;
using SecretaryBot.Domain.Abstractions.Services;
using SecretaryBot.Domain.Models;

namespace SecretaryBot.Bll.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task AddUserAsync(long userId, long chatId)
        {
            var user = new User { TelegramId = userId, ChatId = chatId };
            await _userRepository.AddUserAsync(user);
        }

        public Task<bool> HasAccessAsync(long userId) => _userRepository.CheckAccessAsync(userId);

        public Task ChangeAccessAsync(long userId) => _userRepository.ChangeAccessAsync(userId);
    }
}
