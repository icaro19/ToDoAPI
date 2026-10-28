
using Application.DTOs.Tasks;
using Application.DTOs.Users;
using Domain.Entities;

namespace Application.Mapping
{
    public static class MappingExtensions
    {
        public static UserDto ToDto(
        this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public static TaskDto ToDto(
        this TodoTask task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                UserId = task.UserId
            };
        }
    }
}
