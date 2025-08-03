namespace TuduBot.Application.Models;

public class AddTaskRequest
{
    public long TelegramUserId { get; set; }
    public string Text { get; set; } = default!;
}
