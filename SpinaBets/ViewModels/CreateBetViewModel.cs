using System.ComponentModel.DataAnnotations;

namespace SpinaBets.ViewModels
{
    public class CreateBetViewModel
    {
        [Required]
        public int GameId { get; set; }

        [Required]
        public decimal Stake { get; set; }

        [Required]
        public string Selection { get; set; } = "";
    }
}
