namespace SpinaBets.ViewModels
{
    public class UsersIndexViewModel
    {
        public List<UserViewModel> Users { get; set; } = new();

        public string Search { get; set; } = "";

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
    }
}
