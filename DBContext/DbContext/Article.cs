namespace LogisticApi.DBContext;

public partial class Article
{
    public Guid Id { get; set; }

    public Guid? DocumentNumber { get; set; }

    public bool ActionType { get; set; }

    public string PositionName { get; set; } = null!;

    public int? PositionCount { get; set; }

    public string Unit { get; set; } = null!;

    public decimal? PriceNetto { get; set; }

    public decimal? Vat { get; set; }

    public decimal? PriceBrutto { get; set; }

    public string Storage { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public virtual Document? DocumentNumberNavigation { get; set; }
}
