using UserService.Shared.enums;

namespace UserService.Shared.dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public required string UserName { get; set; }
        public DateOnly BirthDay { get; set; }
        public UserType Role { get; set; } = UserType.Client;
    }
}
