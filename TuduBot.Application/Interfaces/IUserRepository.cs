using TuduBot.Domain.Entities;

namespace TuduBot.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByTelegramId(long telegramUserId, CancellationToken cancellationToken = default);
    Task<User> Add(User user, CancellationToken cancellationToken = default);
    Task Update(User user, CancellationToken cancellationToken = default);
    Task Delete(User user, CancellationToken cancellationToken = default);
}
