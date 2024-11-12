namespace allnews.Models.Entities
{
    public class Admin
    {
        public Guid Id { get; set; }

        public required string Username { get; set; }

        public required string PasswordHash { get; set; }
    }
}
