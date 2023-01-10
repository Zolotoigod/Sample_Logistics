using Microsoft.EntityFrameworkCore;

namespace LogisticApi.DBContext;

public partial class SampleLogisticContext : DbContext
{
    public SampleLogisticContext()
    {
    }

    public SampleLogisticContext(DbContextOptions<SampleLogisticContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Articles__3213E83F3F9CEF8F");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.DocumentNumber).HasColumnName("documentNumber");
            entity.Property(e => e.ActionType).HasColumnName("actionType");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.PositionCount).HasColumnName("positionCount");
            entity.Property(e => e.PositionName)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("positionName");
            entity.Property(e => e.PriceBrutto)
                .HasColumnType("money")
                .HasColumnName("priceBrutto");
            entity.Property(e => e.PriceNetto)
                .HasColumnType("money")
                .HasColumnName("priceNetto");
            entity.Property(e => e.Storage)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("storage");
            entity.Property(e => e.Unit)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("unit");
            entity.Property(e => e.Vat)
                .HasColumnType("money")
                .HasColumnName("vat");

            entity.HasOne(d => d.DocumentNumberNavigation).WithMany(p => p.Articles)
                .HasForeignKey(d => d.DocumentNumber)
                .HasConstraintName("FK__Articles__docume__2B3F6F97");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Number).HasName("PK__Document__FD291E40B4087449");

            entity.Property(e => e.Number)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("number");
            entity.Property(e => e.ActionType).HasColumnName("actionType");
            entity.Property(e => e.ContragentName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("contragentName");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date")
                .HasColumnName("createDate");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.PriceBrutto)
                .HasColumnType("money")
                .HasColumnName("priceBrutto");
            entity.Property(e => e.PriceNetto)
                .HasColumnType("money")
                .HasColumnName("priceNetto");
            entity.Property(e => e.Storage)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("storage");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date")
                .HasColumnName("updateDate");
            entity.Property(e => e.Vat)
                .HasColumnType("money")
                .HasColumnName("vat");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
