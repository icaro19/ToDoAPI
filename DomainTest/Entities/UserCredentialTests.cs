using Domain.Entities;
using FluentAssertions;
using Domain.Validation;
using Xunit;

namespace TodoApi.Domain.Tests.Entities;

public class UserCredentialTests
{
    [Fact]
    public void Should_Create_Credential_When_Data_Is_Valid()
    {
        var credential = new UserCredential(
            Guid.NewGuid(),
            "joao.silva",
            "HASH123");

        credential.Should().NotBeNull();
    }

    [Fact]
    public void Should_Throw_When_Username_Is_Invalid()
    {
        Action action = () =>
            new UserCredential(
                Guid.NewGuid(),
                "a",
                "HASH123");

        action.Should()
            .Throw<DomainExceptionValidation>();
    }

    [Fact]
    public void Should_Throw_When_UserId_Is_Empty()
    {
        Action action = () =>
            new UserCredential(
                Guid.Empty,
                "usuario",
                "HASH123");

        action.Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Usuário inválido.");
    }
}