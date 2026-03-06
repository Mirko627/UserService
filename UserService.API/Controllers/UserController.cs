using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.Business.Interfaces;
using UserService.Shared.dtos;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<UserDto> users = await _service.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            UserDto? p = await _service.GetUserByIdAsync(id);
            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUserDto user)
        {
            await _service.AddAsync(user);
            return Created();
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto user)
        {
            int userId = GetUserId();
            await _service.UpdateAsync(id, user, userId);
            return Ok(new { message = "Utente aggiornato con successo" });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = GetUserId();
            await _service.DeleteAsync(id, userId);
            return Ok(new { message = "Utente eliminato con successo" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            string token = await _service.LoginAsync(loginDto);
            return Ok(new { Token = token });
        }
        [HttpPatch("change-password/{id}")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDto dto)
        {
            int userId = GetUserId();
            await _service.ChangePasswordAsync(id, dto, userId);
            return Ok(new { message = "Password cambiata con successo" });
        }
        private int GetUserId()
        {
            string? userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("ID utente non trovato nel token");

            return int.Parse(userIdClaim);
        }
    }
}
