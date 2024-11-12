namespace allnews.Models.DTOs
{
    public class AdminDto
    {
        public required string Username { get; set; }

        public required string PasswordHash { get; set; }
    }
}
