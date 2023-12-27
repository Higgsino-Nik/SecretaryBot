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

        public async Task<bool> HasAccessAsync(long userId) => await _userRepository.CheckAccessAsync(userId);

        public async Task ChangeAccessAsync(long userId) => await _userRepository.ChangeAccessAsync(userId);
    }
}
