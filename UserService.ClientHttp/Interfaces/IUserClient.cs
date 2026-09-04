using UserService.Shared.dtos;

namespace UserService.ClientHttp.Interfaces
{
    public interface IUserClient
    {
        Task<List<UserDto>> GetAllAsync(CancellationToken ct = default);
        Task<UserDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(CreateUserDto user, CancellationToken ct = default);
        Task UpdateAsync(int id, UpdateUserDto user, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
        Task<string?> LoginAsync(LoginDto loginDto, CancellationToken ct = default);
    }
}