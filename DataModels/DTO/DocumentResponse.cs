namespace DataModels.DTO
{
    public record DocumentResponse(
        Guid Number,
        bool? ActionType,
        DateTime? CreateDate,
        DateTime? UpdateDate,
        string? ContragentName,
        string Market,
        decimal? PriceNetto,
        decimal? Vat,
        decimal? PriceBrutto,
        ICollection<ArticleResponse>? Articles);

}
