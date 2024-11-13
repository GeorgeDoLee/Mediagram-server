namespace Mediagram.Models.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int TrendingScore { get; set; } = 0;
        //public List<Article> Articles { get; set; } = new List<Article>();
    }
}
