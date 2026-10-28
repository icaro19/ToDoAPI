using Domain.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using Domain.Validation;

namespace TodoApi.Domain.Tests.Entities;

[TestClass]
public class UserCredentialTests
{
    [TestMethod]
    public void Should_Create_Credential_When_Data_Is_Valid()
    {
        var credential = new UserCredential(
            Guid.NewGuid(),
            "joao.silva",
            "HASH123");

        credential.Should().NotBeNull();
    }

    [TestMethod]
    public void Should_Throw_When_Username_Is_Invalid()
    {
        Assert.Throws < DomainExceptionValidation > (() =>
        {
            new UserCredential(
                Guid.NewGuid(),
                "a",
                "HASH123");
        });
    }

    [TestMethod]
    public void Should_Throw_When_UserId_Is_Empty()
    {
        var exception = Assert.Throws<DomainExceptionValidation>(() =>
        {
            new UserCredential(
                Guid.Empty,
                "usuario",
                "HASH123");
        });

        Assert.AreEqual(
            "Usuário inválido.",
            exception.Message);
    }
}