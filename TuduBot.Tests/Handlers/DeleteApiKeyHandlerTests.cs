using AwesomeAssertions;
using NSubstitute;
using TuduBot.Application.Handlers;
using TuduBot.Application.Interfaces;
using TuduBot.Domain.Entities;

namespace TuduBot.Tests.Handlers;

public class DeleteApiKeyHandlerTests
{
    [Fact]
    public async Task Should_clear_api_key_and_project()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            TelegramUserId = 123,
            TodoistApiKey = "encrypted-key",
            DefaultProjectId = "123456",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow.AddMinutes(-10)
        };

        var repo = Substitute.For<IUserRepository>();
        repo.GetByTelegramId(user.TelegramUserId, Arg.Any<CancellationToken>())
            .Returns(user);

        var handler = new DeleteApiKeyHandler(repo);

        await handler.Handle(user.TelegramUserId, CancellationToken.None);

        user.TodoistApiKey.Should().BeNull();
        user.DefaultProjectId.Should().BeNull();
        user.UpdatedAt.Should().BeAfter(user.CreatedAt);

        await repo.Received(1).Update(user, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Should_throw_if_user_not_found()
    {
        var repo = Substitute.For<IUserRepository>();
        repo.GetByTelegramId(Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns((User?)null);

        var handler = new DeleteApiKeyHandler(repo);

        var act = () => handler.Handle(123, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("User not found");
    }
}
