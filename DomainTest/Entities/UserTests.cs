using Domain.Entities;
using FluentAssertions;
using Domain.Validation;
using Xunit;

namespace TodoApi.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Should_Create_User_When_Data_Is_Valid()
    {
        var user = new User(
            "João Silva",
            "joao@email.com");

        user.Should().NotBeNull();
        user.Name.Should().Be("João Silva");
        user.Email.Should().Be("joao@email.com");
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Empty()
    {
        Action action = () =>
            new User("", "teste@email.com");

        action.Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Nome é obrigatório.");
    }

    [Fact]
    public void Should_Throw_Exception_When_Email_Is_Invalid()
    {
        Action action = () =>
            new User("João", "emailinvalido");

        action.Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("E-mail inválido.");
    }
}