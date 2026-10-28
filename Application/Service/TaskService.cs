using Application.DTOs.Tasks;
using Application.Interface;
using Application.Mapping;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interface;
using TaskStatus = Domain.Enums.TaskStatus;
using ApplicationException = Application.Exceptions.ApplicationException;

namespace Application.Service
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(
        ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Guid> CreateAsync(
        CreateTaskDto dto)
        {
            var task = new TodoTask(
            dto.Title,
            dto.Description,
            dto.UserId);

            await _taskRepository.AddAsync(task);

            return task.Id;
        }

        public async Task<TaskDto> GetByIdAsync(
        Guid id)
        {
            var task =
            await _taskRepository.GetByIdAsync(id);

            if (task is null)
                throw new ApplicationException(
                "Tarefa não encontrada.");

            return task.ToDto();
        }

        public async Task<IEnumerable<TaskDto>> GetAllAsync()
        {
            var tasks =
            await _taskRepository.GetAllAsync();

            return tasks.Select(x => x.ToDto());
        }

        public async Task<IEnumerable<TaskDto>> GetByStatusAsync(
        TaskStatus status)
        {
            var tasks =
            await _taskRepository
            .GetByStatusAsync(status);

            return tasks.Select(x => x.ToDto());
        }

        public async Task UpdateAsync(
        Guid id,
        UpdateTaskDto dto)
        {
            var task =
            await _taskRepository.GetByIdAsync(id);

            if (task is null)
                throw new ApplicationException(
                "Tarefa não encontrada.");

            task.Update(
            dto.Title,
            dto.Description);

            await _taskRepository.UpdateAsync(task);
        }

        public async Task ChangeStatusAsync(
        Guid id,
        TaskStatus status)
        {
            var task =
            await _taskRepository.GetByIdAsync(id);

            if (task is null)
                throw new ApplicationException(
                "Tarefa não encontrada.");

            task.ChangeStatus(status);

            await _taskRepository.UpdateAsync(task);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _taskRepository.DeleteAsync(id);
        }

    }
}
