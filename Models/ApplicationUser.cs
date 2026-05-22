using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace SpinaBets.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string Surname { get; set; } = "";

        [Required]
        [StringLength(13, MinimumLength = 13)]
        public string IDNumber { get; set; } = "";

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<Account>? Accounts { get; set; }
    }
}
