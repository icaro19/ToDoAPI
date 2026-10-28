using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using Domain.Validation;

namespace TodoApi.Domain.Tests.Entities;

[TestClass]
public class UserTests
{
    [TestMethod]
    public void Should_Create_User_When_Data_Is_Valid()
    {
        var user = new User(
            "João Silva",
            "joao@email.com");

        user.Should().NotBeNull();
        user.Name.Should().Be("João Silva");
        user.Email.Should().Be("joao@email.com");
    }

    [TestMethod]
    public void Should_Throw_Exception_When_Name_Is_Empty()
    {
        var exception = Assert.Throws<DomainExceptionValidation>(() =>
        {
            new User("", "teste@email.com");
        });

        Assert.AreEqual(
            "Nome é obrigatório.",
            exception.Message);
    }

    [TestMethod]
    public void Should_Throw_Exception_When_Email_Is_Invalid()
    {
        var exception = Assert.Throws<DomainExceptionValidation>(() =>
        {
            new User("João", "emailinvalido");
        });

        Assert.AreEqual(
            "E-mail inválido.",
            exception.Message);
    }
}