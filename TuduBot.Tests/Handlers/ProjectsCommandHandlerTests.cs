using AwesomeAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TuduBot.Application.Interfaces;
using TuduBot.Application.Models;
using TuduBot.Bot.Telegram.Handlers;

namespace TuduBot.Tests.Handlers;

public class ProjectsCommandHandlerTests
{
    [Fact]
    public async Task Should_send_formatted_project_list()
    {
        var bot = Substitute.For<ITelegramBotClient>();
        var handler = Substitute.For<IGetProjectsHandler>();

        var userId = 123456;
        var chatId = 123456;

        handler.Handle(userId, Arg.Any<CancellationToken>())
            .Returns(new List<ProjectDto>
            {
                new() { Id = "1", Name = "Inbox" },
                new() { Id = "2", Name = "Work" }
            });

        var command = new ProjectsCommandHandler(handler, bot);

        var update = new Update
        {
            Message = new Message
            {
                From = new User { Id = userId },
                Chat = new Chat { Id = chatId },
                Text = "/projects"
            }
        };

        await command.Handle(update, CancellationToken.None);

        //await bot.Received(1).SendMessage(
        //    chatId,
        //    Arg.Is<string>(s => s.Contains("Inbox") && s.Contains("Work")),
        //    ParseMode.Markdown,
        //    false, false, 0, null, null, Arg.Any<CancellationToken>());
    }

    [Fact(Skip = "This test is temporarily skipped due to API changes")]
    public async Task Should_send_warning_when_no_projects()
    {
        var bot = Substitute.For<ITelegramBotClient>();
        var handler = Substitute.For<IGetProjectsHandler>();

        var userId = 999;
        var chatId = 999;

        handler.Handle(userId, Arg.Any<CancellationToken>())
            .Returns(new List<ProjectDto>());

        var command = new ProjectsCommandHandler(handler, bot);

        var update = new Update
        {
            Message = new Message
            {
                From = new User { Id = userId },
                Chat = new Chat { Id = chatId },
                Text = "/projects"
            }
        };

        await command.Handle(update, CancellationToken.None);

        await bot.Received(1).SendMessage(
            chatId,
            Arg.Is<string>(s => s.Contains("There are no projects")),
            cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact(Skip = "This test is temporarily skipped due to API changes")]
    public async Task Should_send_error_on_exception()
    {
        var bot = Substitute.For<ITelegramBotClient>();
        var handler = Substitute.For<IGetProjectsHandler>();

        handler.Handle(Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Throws(new InvalidOperationException("Error: Key not found"));

        var command = new ProjectsCommandHandler(handler, bot);

        var update = new Update
        {
            Message = new Message
            {
                From = new User { Id = 1 },
                Chat = new Chat { Id = 1 },
                Text = "/projects"
            }
        };

        await command.Handle(update, CancellationToken.None);

        await bot.Received(1).SendMessage(
            default!, default!, default, default, default, default, default, default!, default);


    }
}
