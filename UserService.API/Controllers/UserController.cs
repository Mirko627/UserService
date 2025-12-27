using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            if (p == null)
                return NotFound();
            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUserDto user)
        {
            await _service.AddAsync(user);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto user)
        {
            await _service.UpdateAsync(id, user);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            string? token = await _service.LoginAsync(loginDto);

            if (token == null)
                return Unauthorized("Username o Password errati");

            return Ok(new { Token = token });
        }
    }
}
