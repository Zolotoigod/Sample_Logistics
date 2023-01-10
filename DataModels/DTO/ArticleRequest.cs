namespace LogisticApi.DTO
{
    public class ArticleRequest
    {
        public string PositionName { get; set; } = null!;

        public int? PositionCount { get; set; }

        public string Unit { get; set; } = null!;

        public decimal? PriceNetto { get; set; }

        public decimal? Vat { get; set; }

        public decimal? PriceBrutto { get; set; }
    }
}
