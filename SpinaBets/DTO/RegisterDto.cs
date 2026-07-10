using System.ComponentModel.DataAnnotations;

namespace SpinaBets.DTO
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string Surname { get; set; } = "";

        [Required]
        [StringLength(13, MinimumLength = 13)]
        public string IDNumber { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string PhoneNumber { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = "";
    }
}
