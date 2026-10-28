using Application.DTOs.Auth;
using Domain.Entities;

namespace Application.Abstractions.Security
{
    public interface IJwtTokenGenerator
    {
        LoginResponseDto Generate(User user);
    }
}
