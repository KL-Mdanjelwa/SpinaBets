namespace SpinaBets.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; } = "";

        public string FirstName { get; set; } = "";

        public string Surname { get; set; } = "";

        public string Email { get; set; } = "";

        public string? PhoneNumber { get; set; }

        public string IDNumber { get; set; } = "";

        public List<string> Roles { get; set; } = new();

        public DateTime CreatedDate { get; set; }
    }
}
