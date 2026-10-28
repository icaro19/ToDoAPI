using Application.DTOs.Auth;
using Application.DTOs.Users;

namespace Application.Interface
{
    public interface IAuthService
    {
        Task RegisterAsync(
        CreateUserDto dto);

        Task<LoginResponseDto> LoginAsync(
        LoginDto dto);
    }
}
