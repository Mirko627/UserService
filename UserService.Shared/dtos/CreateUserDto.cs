using UserService.Shared.enums;

namespace UserService.Shared.dtos
{
    public class CreateUserDto
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public DateOnly BirthDay { get; set; }
        public UserType Role { get; set; }
    }
}