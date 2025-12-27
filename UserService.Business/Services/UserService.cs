using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Business.Interfaces;
using UserService.Repository.Entities;
using UserService.Repository.Interfaces;
using UserService.Shared.dtos;

namespace UserService.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            List<User> users = await repository.GetAllAsync();
            List<UserDto> userDtos = mapper.Map<List<UserDto>>(users);
            return userDtos;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            User? user = await repository.GetUserByIdAsync(id);
            UserDto? userDto = mapper.Map<UserDto>(user);
            return userDto;
        }
        public async Task AddAsync(CreateUserDto user)
        {
            User u = mapper.Map<User>(user);
            u.LastModified = DateTime.UtcNow;
            u.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await repository.AddAsync(u);
        }

        public async Task DeleteAsync(int id)
        {
            await repository.DeleteAsync(id);
        }

        public async Task UpdateAsync(int id, UpdateUserDto user)
        {
            User? userEntity = await repository.GetUserByIdAsync(id);
            if (userEntity == null)
                throw new Exception("Utente non trovato");
            mapper.Map(user, userEntity);
            await repository.UpdateAsync(userEntity);
        }

        public async Task<bool> ChangePasswordAsync(int id, ChangePasswordDto dto)
        {
            User? userEntity = await repository.GetUserByIdAsync(id);
            if (userEntity == null)
                throw new Exception("Utente non trovato");
            bool control = BCrypt.Net.BCrypt.Verify(dto.OldPassword, userEntity.Password);
            if(!control)
                return false;
            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await repository.UpdateAsync(userEntity);
            return true;
        }
        public async Task<string?> LoginAsync(LoginDto dto)
        {
            User? userEntity = await repository.GetUserByUsernameAsync(dto.Username);
            if (userEntity == null) return null;
            bool control = BCrypt.Net.BCrypt.Verify(dto.Password, userEntity.Password);
            if (!control) return null;
            return GenerateJwtToken(userEntity);
        }
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("b133a0c0e9bee3be20163d2ad31d6248db292aa6dcb1ee087a2aa50e0fc75ae2"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "UserService",
                audience: "ProjectMicroservizi",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
