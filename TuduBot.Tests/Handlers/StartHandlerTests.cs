using AwesomeAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TuduBot.Application.Handlers;
using TuduBot.Application.Interfaces;
using TuduBot.Domain.Entities;

namespace TuduBot.Tests.Handlers;

public class StartHandlerTests
{
    [Fact]
    public async Task Should_create_user_if_not_exists()
    {
        // Arrange
        var repo = Substitute.For<IUserRepository>();
        repo.GetByTelegramId(Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns((User?)null);

        var handler = new StartHandler(repo);
        var telegramId = 123456789;

        // Act
        await handler.Handle(telegramId, CancellationToken.None);

        // Assert
        await repo.Received(1).Add(Arg.Is<User>(u => u.TelegramUserId == telegramId), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Should_do_nothing_if_user_exists()
    {
        // Arrange
        var repo = Substitute.For<IUserRepository>();
        repo.GetByTelegramId(Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(new User { Id = Guid.NewGuid(), TelegramUserId = 987654321 });

        var handler = new StartHandler(repo);

        // Act
        await handler.Handle(987654321, CancellationToken.None);

        // Assert
        await repo.DidNotReceive().Add(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Should_throw_if_cancellation_is_requested()
    {
        // Arrange
        var repo = Substitute.For<IUserRepository>();
        var handler = new StartHandler(repo);
        var tokenSource = new CancellationTokenSource();
        tokenSource.Cancel();

        // Act
        var act = () => handler.Handle(111111, tokenSource.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task Should_throw_if_repository_fails()
    {
        // Arrange
        var repo = Substitute.For<IUserRepository>();
        repo.GetByTelegramId(Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Throws(new Exception("DB is down"));

        var handler = new StartHandler(repo);

        // Act
        var act = () => handler.Handle(222222, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("DB is down");
    }

}
