using UserService.Repository.Entities;
using UserService.Shared.dtos;

namespace UserService.Business.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync(CancellationToken ct = default);
        Task<UserDto> GetUserByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(CreateUserDto user, CancellationToken ct = default);
        Task UpdateAsync(int id, UpdateUserDto user, int userId, CancellationToken ct = default);
        Task DeleteAsync(int id, int userId, CancellationToken ct = default);
        Task ChangePasswordAsync(int id, ChangePasswordDto dto, int userId, CancellationToken ct = default);
        Task<string> LoginAsync(LoginDto dto, CancellationToken ct = default);
    }
}
