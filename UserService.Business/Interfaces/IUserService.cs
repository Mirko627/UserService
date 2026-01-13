using UserService.Repository.Entities;
using UserService.Shared.dtos;

namespace UserService.Business.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task AddAsync(CreateUserDto user);
        Task UpdateAsync(int id, UpdateUserDto user, int userId);
        Task DeleteAsync(int id, int userId);
        Task ChangePasswordAsync(int id, ChangePasswordDto dto, int userId);
        Task<string> LoginAsync(LoginDto dto);
    }
}
