using Application.DTOs.Tasks;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Application.Interface
{
    public interface ITaskService
    {
        Task<TaskDto> GetByIdAsync(Guid id);

        Task<IEnumerable<TaskDto>> GetAllAsync();

        Task<IEnumerable<TaskDto>> GetByStatusAsync(
        TaskStatus status);

        Task<Guid> CreateAsync(CreateTaskDto dto);

        Task UpdateAsync(
        Guid id,
        UpdateTaskDto dto);

        Task ChangeStatusAsync(
        Guid id,
        TaskStatus status);

        Task DeleteAsync(Guid id);
    }
}
