using System.ComponentModel.DataAnnotations;

namespace SpinaBets.DTO
{
    public class ProfileDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string Surname { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
