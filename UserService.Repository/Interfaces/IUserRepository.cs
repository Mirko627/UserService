using UserService.Repository.Entities;

namespace UserService.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(CancellationToken ct = default);
        Task<User?> GetUserByIdAsync(int id, CancellationToken ct = default);
        Task<User?> GetUserByUsernameAsync(string username, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
        Task UpdateAsync(User user, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
