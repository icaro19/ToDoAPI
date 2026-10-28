using Domain.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using Domain.Enums;
using Domain.Validation;
using TaskStatus = Domain.Enums.TaskStatus;

namespace TodoApi.Domain.Tests.Entities;

[TestClass]
public class TodoTaskTests
{
    [TestMethod]
    public void Should_Create_Task_With_Pending_Status()
    {
        var task = new TodoTask(
            "Implementar API",
            "Criar endpoint de tarefas",
            Guid.NewGuid());

        task.Status.Should().Be(TaskStatus.Pending);
    }

    [TestMethod]
    public void Should_Throw_When_Title_Is_Empty()
    {
        var exception = Assert.Throws<DomainExceptionValidation>(() =>
        {
            new TodoTask(
                "",
                "Descrição",
                Guid.NewGuid());
        });

        Assert.AreEqual(
            "Título é obrigatório.",
            exception.Message);
    }

    [TestMethod]
    public void Should_Change_Status()
    {
        var task = new TodoTask(
            "Implementar API",
            "Descrição",
            Guid.NewGuid());

        task.ChangeStatus(TaskStatus.InProgress);

        task.Status.Should().Be(TaskStatus.InProgress);
    }

    [TestMethod]
    public void Should_Mark_Task_As_Completed()
    {
        var task = new TodoTask(
            "Implementar API",
            "Descrição",
            Guid.NewGuid());

        task.MarkAsCompleted();

        task.Status.Should().Be(TaskStatus.Completed);
    }

    [TestMethod]
    public void Should_Not_Allow_Update_Completed_Task()
    {
        var task = new TodoTask(
            "Task 1",
            "Descrição",
            Guid.NewGuid());

        task.MarkAsCompleted();

        var exception = Assert.Throws<DomainExceptionValidation>(() =>
        {
            task.Update(
                "Novo título",
                "Nova descrição");
        });

        Assert.AreEqual(
            "Não é permitido alterar uma tarefa concluída.",
            exception.Message);
    }
}