using Domain.Enums;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Application.DTOs.Tasks
{
    public class TaskDto
    {
        public Guid Id { get; init; }

        public string Title { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public TaskStatus Status { get; init; }

        public DateTime CreatedAt { get; init; }

        public DateTime? UpdatedAt { get; init; }

        public Guid UserId { get; init; }
    }
}
