namespace GameShow.DTO.User
{
    public class UpdateUser
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string NameDisplay { get; set; } = string.Empty;
    }
}
