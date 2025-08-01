namespace TuduBot.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public long TelegramUserId { get; set; }
    public string? TodoistApiKey { get; set; }
    public string? DefaultProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
