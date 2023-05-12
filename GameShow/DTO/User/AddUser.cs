namespace GameShow.DTO.User
{
    public class AddUser
    {
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string NameDisplay { get; set; } = string.Empty;
        //public bool IsAdmin { get; set; }
    }
}
