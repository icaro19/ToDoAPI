using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Application.DTOs.Auth;
using Application.DTOs.Users;
using Application.Exceptions;
using Application.Abstractions.Security;
using Application.Service;
using Domain.Entities;
using Domain.Interface;
using ApplicationException = Application.Exceptions.ApplicationException;

namespace ApplicationTestes;

[TestClass]
public class AuthServiceTests
{
    private Mock<IUserRepository> _userRepository = null!;
    private Mock<IAuthRepository> _authRepository = null!;
    private Mock<IPasswordHasher> _passwordHasher = null!;
    private Mock<IJwtTokenGenerator> _jwtGenerator = null!;

    private AuthService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _userRepository =
        new Mock<IUserRepository>();

        _authRepository =
        new Mock<IAuthRepository>();

        _passwordHasher =
        new Mock<IPasswordHasher>();

        _jwtGenerator =
        new Mock<IJwtTokenGenerator>();

        _service = new AuthService(
        _userRepository.Object,
        _authRepository.Object,
        _passwordHasher.Object,
        _jwtGenerator.Object);
    }

    [TestMethod]
    public async Task Should_Register_User()
    {
        var dto = new CreateUserDto
        {
            Name = "João",
            Email = "joao@email.com",
            Username = "joao",
            Password = "123456"
        };

        _passwordHasher
        .Setup(x => x.Hash(dto.Password))
        .Returns("HASH");

        await _service.RegisterAsync(dto);

        _userRepository.Verify(
        x => x.AddAsync(It.IsAny<User>()),
        Times.Once);

        _authRepository.Verify(
        x => x.AddCredentialAsync(
        It.IsAny<UserCredential>()),
        Times.Once);
    }

    [TestMethod]
    public async Task Should_Login_Successfully()
    {
        var user = new User(
        "João",
        "joao@email.com");

        var credential = new UserCredential(
        user.Id,
        "joao",
        "HASH");

        credential.GetType();

        _authRepository
        .Setup(x =>
        x.GetByUsernameAsync("joao"))
        .ReturnsAsync(credential);

        _passwordHasher
        .Setup(x =>
        x.Verify("123456", "HASH"))
        .Returns(true);

        _jwtGenerator
        .Setup(x =>
        x.Generate(It.IsAny<User>()))
        .Returns(
        new LoginResponseDto
        {
            Token = "jwt-token",
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        });

        var result =
        await _service.LoginAsync(
        new LoginDto
        {
            Username = "joao",
            Password = "123456"
        });

        result.Should().NotBeNull();
        result.Token.Should().Be("jwt-token");
    }

    [TestMethod]
    public async Task Should_Throw_When_User_Not_Found()
    {
        _authRepository
        .Setup(x =>
        x.GetByUsernameAsync(It.IsAny<string>()))
        .ReturnsAsync((UserCredential?)null);

        await Assert.ThrowsAsync<ApplicationException>(
        async () =>
        {
            await _service.LoginAsync(
    new LoginDto
        {
            Username = "teste",
            Password = "123"
        });
        });
    }

    [TestMethod]
    public async Task Should_Throw_When_Password_Is_Invalid()
    {
        var user = new User(
        "João",
        "joao@email.com");

        var credential = new UserCredential(
        user.Id,
        "joao",
        "HASH");

        _authRepository
        .Setup(x =>
        x.GetByUsernameAsync("joao"))
        .ReturnsAsync(credential);

        _passwordHasher
        .Setup(x =>
        x.Verify("123456", "HASH"))
        .Returns(false);

        await Assert.ThrowsAsync<ApplicationException>(
        async () =>
        {
            await _service.LoginAsync(
    new LoginDto
        {
            Username = "joao",
            Password = "123456"
        });
        });
    }

    [TestMethod]
    public async Task Should_Generate_Token_Only_Once()
    {
        var user = new User(
        "João",
        "joao@email.com");

        var credential = new UserCredential(
        user.Id,
        "joao",
        "HASH");

        _authRepository
        .Setup(x =>
        x.GetByUsernameAsync("joao"))
        .ReturnsAsync(credential);

        _passwordHasher
        .Setup(x =>
        x.Verify("123456", "HASH"))
        .Returns(true);

        _jwtGenerator
        .Setup(x =>
        x.Generate(It.IsAny<User>()))
        .Returns(new LoginResponseDto
        {
            Token = "token"
        });

        await _service.LoginAsync(
        new LoginDto
        {
            Username = "joao",
            Password = "123456"
        });

        _jwtGenerator.Verify(
        x => x.Generate(It.IsAny<User>()),
        Times.Once);
    }

    [TestMethod]
    public async Task Should_Call_Hash_Once_During_Register()
    {
        var dto = new CreateUserDto
        {
            Name = "João",
            Email = "joao@email.com",
            Username = "joao",
            Password = "123456"
        };

        _passwordHasher
            .Setup(x => x.Hash(dto.Password))
            .Returns("HASH");

        await _service.RegisterAsync(dto);

        _passwordHasher.Verify(
            x => x.Hash(dto.Password),
            Times.Once);
    }

    [TestMethod]
    public async Task Should_Save_Hashed_Password_During_Register()
    {
        var dto = new CreateUserDto
        {
            Name = "João",
            Email = "joao@email.com",
            Username = "joao",
            Password = "123456"
        };

        _passwordHasher
            .Setup(x => x.Hash(dto.Password))
            .Returns("HASH-GERADO");

        UserCredential? capturedCredential = null;

        _authRepository
            .Setup(x =>
                x.AddCredentialAsync(
                    It.IsAny<UserCredential>()))
            .Callback<UserCredential>(c =>
            {
                capturedCredential = c;
            });

        await _service.RegisterAsync(dto);

        capturedCredential.Should().NotBeNull();
        capturedCredential!.PasswordHash
            .Should()
            .Be("HASH-GERADO");
    }

    [TestMethod]
    public async Task Should_Not_Generate_Token_When_User_Does_Not_Exist()
    {
        _authRepository
            .Setup(x =>
                x.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync((UserCredential?)null);

        await Assert.ThrowsAsync<ApplicationException>(
            async () =>
            {
                await _service.LoginAsync(
                    new LoginDto
                    {
                        Username = "inexistente",
                        Password = "123"
                    });
            });

        _jwtGenerator.Verify(
            x => x.Generate(It.IsAny<User>()),
            Times.Never);
    }

    [TestMethod]
    public async Task Should_Not_Generate_Token_When_Password_Is_Invalid()
    {
        var user = new User(
            "João",
            "joao@email.com");

        var credential = new UserCredential(
            user.Id,
            "joao",
            "HASH");

        _authRepository
            .Setup(x =>
                x.GetByUsernameAsync("joao"))
            .ReturnsAsync(credential);

        _passwordHasher
            .Setup(x =>
                x.Verify("123456", "HASH"))
            .Returns(false);

        await Assert.ThrowsAsync<ApplicationException>(
            async () =>
            {
                await _service.LoginAsync(
                    new LoginDto
                    {
                        Username = "joao",
                        Password = "123456"
                    });
            });

        _jwtGenerator.Verify(
            x => x.Generate(It.IsAny<User>()),
            Times.Never);
    }

    [TestMethod]
    public async Task Should_Call_Verify_Once_During_Login()
    {
        var user = new User(
            "João",
            "joao@email.com");

        var credential = new UserCredential(
            user.Id,
            "joao",
            "HASH");

        _authRepository
            .Setup(x => x.GetByUsernameAsync("joao"))
            .ReturnsAsync(credential);

        _passwordHasher
            .Setup(x => x.Verify("123456", "HASH"))
            .Returns(true);

        _jwtGenerator
            .Setup(x => x.Generate(It.IsAny<User>()))
            .Returns(new LoginResponseDto
            {
                Token = "token"
            });

        await _service.LoginAsync(
            new LoginDto
            {
                Username = "joao",
                Password = "123456"
            });

        _passwordHasher.Verify(
            x => x.Verify("123456", "HASH"),
            Times.Once);
    }
}
