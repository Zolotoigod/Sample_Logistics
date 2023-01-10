namespace LogisticApi.DTO
{
    public class DocumentRequest
    {
        public bool? ActionType { get; set; }

        public string? ContragentName { get; set; }

        public string Storage { get; set; } = null!;

        public ICollection<ArticleRequest> Articles { get; set; } = new List<ArticleRequest>();
    }
}
