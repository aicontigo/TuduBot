using TuduBot.Application.Models;

namespace TuduBot.Application.Interfaces;

public interface IAddTaskHandler
{
    Task Handle(AddTaskRequest request, CancellationToken cancellationToken);
}
