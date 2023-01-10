namespace DataModels.DTO
{
    public record ArticleResponse(
        Guid Id,
        string PositionName,
        int? PositionCount,
        string Unit,
        decimal? PriceNetto,
        decimal? Vat,
        decimal? PriceBrutto);

}
