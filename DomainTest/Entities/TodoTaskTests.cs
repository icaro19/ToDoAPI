using Domain.Entities;
using FluentAssertions;
using Domain.Enums;
using Domain.Validation;
using Xunit;
using TaskStatus = Domain.Enums.TaskStatus;

namespace TodoApi.Domain.Tests.Entities;

public class TodoTaskTests
{
    [Fact]
    public void Should_Create_Task_With_Pending_Status()
    {
        var task = new TodoTask(
            "Implementar API",
            "Criar endpoint de tarefas",
            Guid.NewGuid());

        task.Status.Should().Be(TaskStatus.Pending);
    }

    [Fact]
    public void Should_Throw_When_Title_Is_Empty()
    {
        Action action = () =>
            new TodoTask(
                "",
                "Descrição",
                Guid.NewGuid());

        action.Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Título é obrigatório.");
    }

    [Fact]
    public void Should_Change_Status()
    {
        var task = new TodoTask(
            "Implementar API",
            "Descrição",
            Guid.NewGuid());

        task.ChangeStatus(TaskStatus.InProgress);

        task.Status.Should().Be(TaskStatus.InProgress);
    }

    [Fact]
    public void Should_Mark_Task_As_Completed()
    {
        var task = new TodoTask(
            "Implementar API",
            "Descrição",
            Guid.NewGuid());

        task.MarkAsCompleted();

        task.Status.Should().Be(TaskStatus.Completed);
    }
}