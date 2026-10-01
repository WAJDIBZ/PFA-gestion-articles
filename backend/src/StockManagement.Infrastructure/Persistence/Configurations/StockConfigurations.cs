using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockManagement.Domain.Entities;

namespace StockManagement.Infrastructure.Persistence.Configurations;

public class DepotConfiguration : IEntityTypeConfiguration<Depot>
{
    public void Configure(EntityTypeBuilder<Depot> builder)
    {
        builder.Property(d => d.Nom).IsRequired().HasMaxLength(150);
        builder.HasQueryFilter(d => !d.EstSupprime);
    }
}

public class LotConfiguration : IEntityTypeConfiguration<Lot>
{
    public void Configure(EntityTypeBuilder<Lot> builder)
    {
        builder.Property(l => l.NumeroLot).IsRequired().HasMaxLength(100);
        builder.HasQueryFilter(l => !l.EstSupprime);

        builder.HasOne(l => l.Article).WithMany(a => a.Lots).HasForeignKey(l => l.ArticleId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(l => l.ArticleVariante).WithMany(v => v.Lots).HasForeignKey(l => l.ArticleVarianteId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class NumeroSerieConfiguration : IEntityTypeConfiguration<NumeroSerie>
{
    public void Configure(EntityTypeBuilder<NumeroSerie> builder)
    {
        builder.Property(n => n.Numero).IsRequired().HasMaxLength(150);
        builder.HasIndex(n => n.Numero).IsUnique();
        builder.HasQueryFilter(n => !n.EstSupprime);

        builder.HasOne(n => n.Article).WithMany(a => a.NumerosSerie).HasForeignKey(n => n.ArticleId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(n => n.ArticleVariante).WithMany(v => v.NumerosSerie).HasForeignKey(n => n.ArticleVarianteId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(n => n.Depot).WithMany(d => d.NumerosSerie).HasForeignKey(n => n.DepotId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class StockItemConfiguration : IEntityTypeConfiguration<StockItem>
{
    public void Configure(EntityTypeBuilder<StockItem> builder)
    {
        builder.HasQueryFilter(s => !s.EstSupprime);

        builder.HasOne(s => s.Article).WithMany(a => a.StockItems).HasForeignKey(s => s.ArticleId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(s => s.ArticleVariante).WithMany(v => v.StockItems).HasForeignKey(s => s.ArticleVarianteId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(s => s.Depot).WithMany(d => d.StockItems).HasForeignKey(s => s.DepotId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(s => s.Lot).WithMany(l => l.StockItems).HasForeignKey(s => s.LotId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class MouvementStockConfiguration : IEntityTypeConfiguration<MouvementStock>
{
    public void Configure(EntityTypeBuilder<MouvementStock> builder)
    {
        builder.Property(m => m.UtilisateurNom).IsRequired().HasMaxLength(200);
        builder.HasQueryFilter(m => !m.EstSupprime);

        builder.HasOne(m => m.Article).WithMany(a => a.Mouvements).HasForeignKey(m => m.ArticleId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.ArticleVariante).WithMany(v => v.Mouvements).HasForeignKey(m => m.ArticleVarianteId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.Lot).WithMany(l => l.Mouvements).HasForeignKey(m => m.LotId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.NumeroSerie).WithMany(n => n.Mouvements).HasForeignKey(m => m.NumeroSerieId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.DepotSource).WithMany().HasForeignKey(m => m.DepotSourceId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.DepotDestination).WithMany().HasForeignKey(m => m.DepotDestinationId).OnDelete(DeleteBehavior.Restrict);
    }
}
