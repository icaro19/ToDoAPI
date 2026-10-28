using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application.DTOs.Tasks;
using Application.Service;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interface;
using Domain.Enums;
using Moq;
using ApplicationException = Application.Exceptions.ApplicationException;
using TaskStatus = Domain.Enums.TaskStatus;

namespace ApplicationTestes;

[TestClass]
public class TaskServiceTests
{
    private Mock<ITaskRepository> _taskRepository = null!;
    private TaskService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _taskRepository = new Mock<ITaskRepository>();

        _service = new TaskService(
        _taskRepository.Object);
    }

    [TestMethod]
    public async Task Should_Create_Task()
    {
        var dto = new CreateTaskDto
        {
            Title = "Implementar API",
            Description = "Criar endpoints",
            UserId = Guid.NewGuid()
        };

        var taskId = await _service.CreateAsync(dto);

        taskId.Should().NotBeEmpty();

        _taskRepository.Verify(
        x => x.AddAsync(It.IsAny<TodoTask>()),
        Times.Once);
    }

    [TestMethod]
    public async Task Should_Return_Task_When_Task_Exists()
    {
        var task = new TodoTask(
        "Task Test",
        "Descrição",
        Guid.NewGuid());

        _taskRepository
        .Setup(x => x.GetByIdAsync(task.Id))
        .ReturnsAsync(task);

        var result =
        await _service.GetByIdAsync(task.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(task.Id);
        result.Title.Should().Be(task.Title);
    }

    [TestMethod]
    public async Task Should_Throw_When_Task_Not_Found()
    {
        _taskRepository
        .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
        .ReturnsAsync((TodoTask?)null);

        await Assert.ThrowsAsync<ApplicationException>(
        async () =>
        {
            await _service.GetByIdAsync(Guid.NewGuid());
        });
    }

    [TestMethod]
    public async Task Should_Return_All_Tasks()
    {
        var tasks = new List<TodoTask>
{
new(
"Task 1",
"Descrição 1",
Guid.NewGuid()),

new(
"Task 2",
"Descrição 2",
Guid.NewGuid())
};

        _taskRepository
        .Setup(x => x.GetAllAsync())
        .ReturnsAsync(tasks);

        var result =
        await _service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [TestMethod]
    public async Task Should_Return_Tasks_By_Status()
    {
        var task1 = new TodoTask(
        "Task 1",
        "Descrição",
        Guid.NewGuid());

        var task2 = new TodoTask(
        "Task 2",
        "Descrição",
        Guid.NewGuid());

        task1.ChangeStatus(TaskStatus.Completed);
        task2.ChangeStatus(TaskStatus.Completed);

        var tasks = new List<TodoTask>
{
task1,
task2
};

        _taskRepository
        .Setup(x =>
        x.GetByStatusAsync(TaskStatus.Completed))
        .ReturnsAsync(tasks);

        var result =
        await _service.GetByStatusAsync(
        TaskStatus.Completed);

        result.Should().HaveCount(2);
    }

    [TestMethod]
    public async Task Should_Update_Task()
    {
        var task = new TodoTask(
        "Task Original",
        "Descrição",
        Guid.NewGuid());

        _taskRepository
        .Setup(x => x.GetByIdAsync(task.Id))
        .ReturnsAsync(task);

        var dto = new UpdateTaskDto
        {
            Title = "Novo Título",
            Description = "Nova Descrição"
        };

        await _service.UpdateAsync(
        task.Id,
        dto);

        _taskRepository.Verify(
        x => x.UpdateAsync(task),
        Times.Once);

        task.Title.Should().Be("Novo Título");
    }

    [TestMethod]
    public async Task Should_Change_Status()
    {
        var task = new TodoTask(
        "Task",
        "Descrição",
        Guid.NewGuid());

        _taskRepository
        .Setup(x => x.GetByIdAsync(task.Id))
        .ReturnsAsync(task);

        await _service.ChangeStatusAsync(
        task.Id,
        TaskStatus.Completed);

        task.Status.Should()
        .Be(TaskStatus.Completed);

        _taskRepository.Verify(
        x => x.UpdateAsync(task),
        Times.Once);
    }

    [TestMethod]
    public async Task Should_Delete_Task()
    {
        var taskId = Guid.NewGuid();

        await _service.DeleteAsync(taskId);

        _taskRepository.Verify(
        x => x.DeleteAsync(taskId),
        Times.Once);
    }

    [TestMethod]
    public async Task Should_Throw_When_Updating_Nonexistent_Task()
    {
        _taskRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((TodoTask?)null);

        var dto = new UpdateTaskDto
        {
            Title = "Novo Título",
            Description = "Nova Descrição"
        };

        var exception =
            await Assert.ThrowsAsync<ApplicationException>(
                async () =>
                {
                    await _service.UpdateAsync(
                        Guid.NewGuid(),
                        dto);
                });

        Assert.AreEqual(
            "Tarefa não encontrada.",
            exception.Message);

        _taskRepository.Verify(
            x => x.UpdateAsync(It.IsAny<TodoTask>()),
            Times.Never);
    }

    [TestMethod]
    public async Task Should_Throw_When_Changing_Status_Of_Nonexistent_Task()
    {
        _taskRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((TodoTask?)null);

        var exception =
            await Assert.ThrowsAsync<ApplicationException>(
                async () =>
                {
                    await _service.ChangeStatusAsync(
                        Guid.NewGuid(),
                        TaskStatus.Completed);
                });

        Assert.AreEqual(
            "Tarefa não encontrada.",
            exception.Message);

        _taskRepository.Verify(
            x => x.UpdateAsync(It.IsAny<TodoTask>()),
            Times.Never);
    }

    [TestMethod]
    public async Task Should_Return_Empty_List_When_No_Task_With_Status_Exists()
    {
        _taskRepository
            .Setup(x => x.GetByStatusAsync(TaskStatus.Completed))
            .ReturnsAsync(new List<TodoTask>());

        var result =
            await _service.GetByStatusAsync(
                TaskStatus.Completed);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task Should_Return_Empty_List_When_No_Task_Exists()
    {
        _taskRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<TodoTask>());

        var result =
            await _service.GetAllAsync();

        result.Should().NotBeNull();

    }
}