using Application.DTOs.Users;
using Application.Interface;
using Application.Mapping;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interface;
using ApplicationException = Application.Exceptions.ApplicationException;

namespace Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(
        IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> CreateAsync(
        CreateUserDto dto)
        {
            var user = new User(
            dto.Name,
            dto.Email);

            await _userRepository.AddAsync(user);

            return user.Id;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users
            .Select(user => user.ToDto())
            .ToList();
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var user =
            await _userRepository.GetByIdAsync(id);

            if (user is null)
                throw new ApplicationException(
                "Usuário não encontrado.");

            return user.ToDto();
        }

        public async Task UpdateAsync(
        Guid id,
        UpdateUserDto dto)
        {
            var user =
            await _userRepository.GetByIdAsync(id);

            if (user is null)
                throw new ApplicationException(
                "Usuário não encontrado.");

            user.Update(
            dto.Name,
            dto.Email);

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user =
            await _userRepository.GetByEmailAsync(email);

            if (user is null)
                throw new ApplicationException(
                "Usuário não encontrado.");

            return user.ToDto();
        }
    }
}
