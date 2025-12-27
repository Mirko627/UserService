using UserService.Shared.enums;

namespace UserService.Shared.dtos
{
    public class UpdateUserDto
    {
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public DateOnly BirthDay { get; set; }
        public UserType Role { get; set; }
    }
}
