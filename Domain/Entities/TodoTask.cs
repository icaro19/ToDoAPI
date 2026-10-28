using Domain.Validation;
using Domain.Enums;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Domain.Entities;

public class TodoTask
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public TaskStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public Guid UserId { get; private set; }

    public User User { get; private set; }

    protected TodoTask()
    {
    }

    public TodoTask(string title, string description, Guid userId)
    {
        Validate(title, description, userId);

        Id = Guid.NewGuid();
        Title = title.Trim();
        Description = description?.Trim();
        UserId = userId;

        Status = TaskStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string description)
    {
        if (Status == TaskStatus.Completed)
            throw new DomainExceptionValidation(
            "Não é permitido alterar uma tarefa concluída.");

        Validate(title, description, UserId);

        Title = title.Trim();
        Description = description?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(TaskStatus status)
    {

        if (Status == TaskStatus.Completed && status == TaskStatus.InProgress)
        {
            throw new DomainExceptionValidation(
            "Uma tarefa concluída não pode voltar para Em Andamento.");
        }

        if (!Enum.IsDefined(typeof(TaskStatus), status))
            throw new DomainExceptionValidation("Status inválido.");

        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsCompleted()
    {
        Status = TaskStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(string title, string description, Guid userId)
    {
        if (userId == Guid.Empty)
            throw new DomainExceptionValidation("Usuário inválido.");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainExceptionValidation("Título é obrigatório.");

        if (title.Length < 3)
            throw new DomainExceptionValidation(
            "Título deve possuir pelo menos 3 caracteres.");

        if (title.Length > 200)
            throw new DomainExceptionValidation(
            "Título deve possuir no máximo 200 caracteres.");

        if (!string.IsNullOrWhiteSpace(description) &&
        description.Length > 1000)
        {
            throw new DomainExceptionValidation(
            "Descrição deve possuir no máximo 1000 caracteres.");
        }
    }
}