using System.ComponentModel.DataAnnotations;

namespace SpinaBets.DTO
{
    public class ProfileDto
    {
        [Required(ErrorMessage = "The First Name field is required"), MaxLength(100)]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "The Surname field is required"), MaxLength(100)]
        public string Surname { get; set; } = "";

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = "";

        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
