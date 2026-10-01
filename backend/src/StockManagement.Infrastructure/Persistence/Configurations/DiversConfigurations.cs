using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockManagement.Domain.Entities;

namespace StockManagement.Infrastructure.Persistence.Configurations;

public class FamilleConfiguration : IEntityTypeConfiguration<Famille>
{
    public void Configure(EntityTypeBuilder<Famille> builder)
    {
        builder.Property(f => f.Nom).IsRequired().HasMaxLength(150);
        builder.Property(f => f.Description).HasMaxLength(500);
        builder.HasQueryFilter(f => !f.EstSupprime);

        builder.HasOne(f => f.FamilleParent)
            .WithMany(f => f.SousFamilles)
            .HasForeignKey(f => f.FamilleParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class MarqueConfiguration : IEntityTypeConfiguration<Marque>
{
    public void Configure(EntityTypeBuilder<Marque> builder)
    {
        builder.Property(m => m.Nom).IsRequired().HasMaxLength(150);
        builder.HasQueryFilter(m => !m.EstSupprime);
    }
}

public class UniteConfiguration : IEntityTypeConfiguration<Unite>
{
    public void Configure(EntityTypeBuilder<Unite> builder)
    {
        builder.Property(u => u.Nom).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Symbole).IsRequired().HasMaxLength(20);
        builder.HasQueryFilter(u => !u.EstSupprime);
    }
}

public class FournisseurConfiguration : IEntityTypeConfiguration<Fournisseur>
{
    public void Configure(EntityTypeBuilder<Fournisseur> builder)
    {
        builder.Property(f => f.Nom).IsRequired().HasMaxLength(200);
        builder.HasQueryFilter(f => !f.EstSupprime);
    }
}

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.Property(n => n.Message).IsRequired().HasMaxLength(500);
        builder.HasQueryFilter(n => !n.EstSupprime);

        builder.HasOne(n => n.Article).WithMany().HasForeignKey(n => n.ArticleId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(n => n.Depot).WithMany().HasForeignKey(n => n.DepotId).OnDelete(DeleteBehavior.Restrict);
    }
}
