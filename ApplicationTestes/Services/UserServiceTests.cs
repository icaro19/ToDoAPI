using Application.DTOs.Users;
using Application.Service;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interface;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ApplicationException = Application.Exceptions.ApplicationException;

namespace ApplicationTestes;

[TestClass]
public class UserServiceTests
{
    private Mock<IUserRepository> _userRepository = null!;
    private UserService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _userRepository =
        new Mock<IUserRepository>();

        _service =
        new UserService(
        _userRepository.Object);
    }

    [TestMethod]
    public async Task Should_Return_User_When_User_Exists()
    {
        var user =
        new User(
        "João",
        "joao@email.com");

        _userRepository
        .Setup(x => x.GetByIdAsync(user.Id))
        .ReturnsAsync(user);

        var result =
        await _service.GetByIdAsync(user.Id);

        result.Should().NotBeNull();
        result.Name.Should().Be("João");
    }

    [TestMethod]
    public async Task Should_Throw_When_User_Not_Found()
    {
        _userRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<ApplicationException>( async
            () =>
            {
                await _service.GetByIdAsync(Guid.NewGuid());
            });
    }

    [TestMethod]
    public async Task Should_Create_User()
    {
        var dto = new CreateUserDto
        {
            Name = "João",
            Email = "joao@email.com"
        };

        var id =
            await _service.CreateAsync(dto);

        _userRepository.Verify(
            x => x.AddAsync(
                It.IsAny<User>()),
            Times.Once);
    }

    [TestMethod]
    public async Task Should_Update_User()
    {
        var user =
            new User(
                "João",
                "joao@email.com");

        _userRepository
            .Setup(x => x.GetByIdAsync(user.Id))
            .ReturnsAsync(user);

        var dto =
            new UpdateUserDto
            {
                Name = "João Atualizado",
                Email = "novo@email.com"
            };

        await _service.UpdateAsync(
            user.Id,
            dto);

        _userRepository.Verify(
            x => x.UpdateAsync(user),
            Times.Once);
    }
}
