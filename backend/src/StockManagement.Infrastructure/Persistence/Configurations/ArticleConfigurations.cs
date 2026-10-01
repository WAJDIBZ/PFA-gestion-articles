using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockManagement.Domain.Entities;

namespace StockManagement.Infrastructure.Persistence.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.Property(a => a.Reference).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Designation).IsRequired().HasMaxLength(250);
        builder.Property(a => a.PrixAchat).HasColumnType("numeric(18,2)");
        builder.Property(a => a.PrixVente).HasColumnType("numeric(18,2)");
        builder.HasIndex(a => a.Reference).IsUnique();
        builder.HasQueryFilter(a => !a.EstSupprime);

        builder.HasOne(a => a.Famille).WithMany(f => f.Articles).HasForeignKey(a => a.FamilleId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(a => a.Marque).WithMany(m => m.Articles).HasForeignKey(a => a.MarqueId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(a => a.Unite).WithMany(u => u.Articles).HasForeignKey(a => a.UniteId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class AttributVarianteConfiguration : IEntityTypeConfiguration<AttributVariante>
{
    public void Configure(EntityTypeBuilder<AttributVariante> builder)
    {
        builder.Property(a => a.Nom).IsRequired().HasMaxLength(100);
        builder.HasQueryFilter(a => !a.EstSupprime);
        builder.HasMany(a => a.Valeurs).WithOne(v => v.AttributVariante).HasForeignKey(v => v.AttributVarianteId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class ValeurAttributConfiguration : IEntityTypeConfiguration<ValeurAttribut>
{
    public void Configure(EntityTypeBuilder<ValeurAttribut> builder)
    {
        builder.Property(v => v.Valeur).IsRequired().HasMaxLength(100);
        builder.HasQueryFilter(v => !v.EstSupprime);
    }
}

public class AttributVarianteArticleConfiguration : IEntityTypeConfiguration<AttributVarianteArticle>
{
    public void Configure(EntityTypeBuilder<AttributVarianteArticle> builder)
    {
        builder.HasKey(x => new { x.ArticleId, x.AttributVarianteId });
        builder.HasOne(x => x.Article).WithMany(a => a.AttributsVariantes).HasForeignKey(x => x.ArticleId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.AttributVariante).WithMany(a => a.ArticlesAssocies).HasForeignKey(x => x.AttributVarianteId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class ArticleVarianteConfiguration : IEntityTypeConfiguration<ArticleVariante>
{
    public void Configure(EntityTypeBuilder<ArticleVariante> builder)
    {
        builder.Property(v => v.ReferenceVariante).IsRequired().HasMaxLength(150);
        builder.HasQueryFilter(v => !v.EstSupprime);
        builder.HasOne(v => v.Article).WithMany(a => a.Variantes).HasForeignKey(v => v.ArticleId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class ArticleVarianteValeurConfiguration : IEntityTypeConfiguration<ArticleVarianteValeur>
{
    public void Configure(EntityTypeBuilder<ArticleVarianteValeur> builder)
    {
        builder.HasKey(x => new { x.ArticleVarianteId, x.ValeurAttributId });
        builder.HasOne(x => x.ArticleVariante).WithMany(v => v.Valeurs).HasForeignKey(x => x.ArticleVarianteId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.ValeurAttribut).WithMany(v => v.VariantesAssociees).HasForeignKey(x => x.ValeurAttributId).OnDelete(DeleteBehavior.Restrict);
    }
}
