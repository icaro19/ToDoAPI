using Domain.Entities;

namespace Domain.Interface
{
    public interface IAuthRepository
    {
        Task<UserCredential?> GetByUsernameAsync(string username);
        Task AddCredentialAsync(UserCredential credential);
        Task<UserCredential?> GetByUserIdAsync(Guid userId);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
    }
}
