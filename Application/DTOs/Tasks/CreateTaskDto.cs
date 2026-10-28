namespace Application.DTOs.Tasks
{
    public class CreateTaskDto
    {
        public string Title { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public Guid UserId { get; init; }
    }
}
