using UserService.Shared.dtos;

namespace UserService.ClientHttp.Interfaces
{
    public interface IUserClient
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task AddAsync(CreateUserDto user);
        Task UpdateAsync(int id, UpdateUserDto user);
        Task DeleteAsync(int id);
        Task<string?> LoginAsync(LoginDto loginDto);
    }
}