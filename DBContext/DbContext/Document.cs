namespace LogisticApi.DBContext;

public partial class Document
{
    public Guid Number { get; set; }

    public bool ActionType { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? ContragentName { get; set; }

    public string Storage { get; set; } = null!;

    public decimal? PriceNetto { get; set; }

    public decimal? Vat { get; set; }

    public decimal? PriceBrutto { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Article> Articles { get; } = new List<Article>();
}
