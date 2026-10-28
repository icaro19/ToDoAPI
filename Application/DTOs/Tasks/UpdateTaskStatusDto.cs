using Domain.Enums;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Application.DTOs.Tasks
{
    public class UpdateTaskStatusDto
    {
        public TaskStatus Status { get; init; }
    }
}