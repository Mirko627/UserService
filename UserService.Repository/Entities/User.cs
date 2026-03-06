using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.Shared.enums;

namespace UserService.Repository.Entities
{
    public class User
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }
        public DateOnly BirthDay { get; set; }
        public DateTime LastModified { get; set; } = DateTime.MinValue;
        [Required]
        public UserType Role { get; set; } = UserType.Client;
    }
}
