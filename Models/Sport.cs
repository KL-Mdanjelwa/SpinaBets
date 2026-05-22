namespace SpinaBets.Models
{
    public class Sport
    {
        public int SportId { get; set; }

        public string Name { get; set; } = "";

        public ICollection<Game> Games { get; set; }
    }
}
