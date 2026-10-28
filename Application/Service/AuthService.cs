using Application.Abstractions.Security;
using Application.DTOs.Auth;
using Application.DTOs.Users;
using Application.Interface;
using Domain.Entities;
using Domain.Interface;
using Application.Exceptions;
using Microsoft.AspNetCore.Identity;
using ApplicationException = Application.Exceptions.ApplicationException;

namespace Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtGenerator;

        public AuthService(
        IUserRepository userRepository,
        IAuthRepository authRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtGenerator)
        {
            _userRepository = userRepository;
            _authRepository = authRepository;
            _passwordHasher = passwordHasher;
            _jwtGenerator = jwtGenerator;
        }

        public async Task RegisterAsync(
        CreateUserDto dto)
        {
            var user = new User(
            dto.Name,
            dto.Email);

            if (await _authRepository.UsernameExistsAsync(dto.Username))
            {
                throw new ApplicationException(
                    "Username já cadastrado.");
            }

            await _userRepository.AddAsync(user);

            var credential =
            new UserCredential(
            user.Id,
            dto.Username,
            _passwordHasher.Hash(dto.Password));
                        
            await _authRepository
            .AddCredentialAsync(credential);
        }

        public async Task<LoginResponseDto> LoginAsync(
        LoginDto dto)
        {
            var credential =
            await _authRepository
            .GetByUsernameAsync(dto.Username);

            if (credential is null)
                throw new ApplicationException(
                "Usuário ou senha inválidos.");

            var validPassword =
            _passwordHasher.Verify(
            dto.Password,
            credential.PasswordHash);

            if (!validPassword)
                throw new ApplicationException(
                "Usuário ou senha inválidos.");

            return _jwtGenerator.Generate(
            credential.User);
        }
    }
}
