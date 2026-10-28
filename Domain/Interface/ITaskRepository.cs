using Domain.Entities;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Domain.Interface
{
    public interface ITaskRepository
    {
        Task<TodoTask?> GetByIdAsync(Guid id);

        Task<IEnumerable<TodoTask>> GetAllAsync();

        Task<IEnumerable<TodoTask>> GetByStatusAsync(TaskStatus status);

        Task AddAsync(TodoTask task);

        Task UpdateAsync(TodoTask task);

        Task DeleteAsync(Guid id);

        Task<IReadOnlyCollection<TodoTask>> GetByUserIdAsync(Guid userId);
    }
}
