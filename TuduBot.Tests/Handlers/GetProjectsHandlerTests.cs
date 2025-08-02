using AwesomeAssertions;
using NSubstitute;
using TuduBot.Application.Handlers;
using TuduBot.Application.Interfaces;
using TuduBot.Domain.Entities;
using Todoist.Net.Models;

namespace TuduBot.Tests.Handlers;

public class GetProjectsHandlerTests
{
    [Fact]
    public async Task Should_return_project_list_when_user_has_valid_key()
    {
        var user = new TuduBot.Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            TelegramUserId = 123,
            TodoistApiKey = "encrypted-key"
        };

        var users = Substitute.For<IUserRepository>();
        var crypto = Substitute.For<ICryptoService>();
        var adapter = Substitute.For<ITodoistClientAdapter>();

        users.GetByTelegramId(user.TelegramUserId, Arg.Any<CancellationToken>())
            .Returns(user);
        crypto.Decrypt("encrypted-key").Returns("decrypted-key");

        adapter.GetProjects("decrypted-key", Arg.Any<CancellationToken>())
            .Returns(new List<Project>
            {
                new Project("Inbox") { Id = new ComplexId("1") },
                new Project("Work") { Id = new ComplexId("2") }
            });

        var handler = new GetProjectsHandler(users, crypto, adapter);

        var result = await handler.Handle(user.TelegramUserId, CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Inbox");
    }

    [Fact]
    public async Task Should_throw_if_user_not_found()
    {
        var users = Substitute.For<IUserRepository>();
        var crypto = Substitute.For<ICryptoService>();
        var adapter = Substitute.For<ITodoistClientAdapter>();

        users.GetByTelegramId(Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns((TuduBot.Domain.Entities.User?)null);

        var handler = new GetProjectsHandler(users, crypto, adapter);

        var act = () => handler.Handle(123, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*set API-key*");
    }

    [Fact]
    public async Task Should_throw_if_key_is_empty()
    {
        var user = new TuduBot.Domain.Entities.User
        {
            TelegramUserId = 321,
            TodoistApiKey = null
        };

        var users = Substitute.For<IUserRepository>();
        var crypto = Substitute.For<ICryptoService>();
        var adapter = Substitute.For<ITodoistClientAdapter>();

        users.GetByTelegramId(user.TelegramUserId, Arg.Any<CancellationToken>())
            .Returns(user);

        var handler = new GetProjectsHandler(users, crypto, adapter);

        var act = () => handler.Handle(user.TelegramUserId, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*set API-key*");
    }
}
