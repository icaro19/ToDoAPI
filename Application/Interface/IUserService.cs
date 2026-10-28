using Application.DTOs.Users;

namespace Application.Interface
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<UserDto> GetByEmailAsync(string email);

        Task<IEnumerable<UserDto>> GetAllAsync();

        Task<Guid> CreateAsync(CreateUserDto dto);

        Task UpdateAsync(Guid id, UpdateUserDto dto);

        Task DeleteAsync(Guid id);
    }
}
