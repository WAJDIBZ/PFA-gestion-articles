using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockManagement.Domain.Entities;

namespace StockManagement.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    public DbSet<Famille> Familles => Set<Famille>();
    public DbSet<Marque> Marques => Set<Marque>();
    public DbSet<Unite> Unites => Set<Unite>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<AttributVariante> AttributsVariantes => Set<AttributVariante>();
    public DbSet<ValeurAttribut> ValeursAttributs => Set<ValeurAttribut>();
    public DbSet<AttributVarianteArticle> AttributsVarianteArticles => Set<AttributVarianteArticle>();
    public DbSet<ArticleVariante> ArticleVariantes => Set<ArticleVariante>();
    public DbSet<ArticleVarianteValeur> ArticleVarianteValeurs => Set<ArticleVarianteValeur>();
    public DbSet<Depot> Depots => Set<Depot>();
    public DbSet<Lot> Lots => Set<Lot>();
    public DbSet<NumeroSerie> NumerosSerie => Set<NumeroSerie>();
    public DbSet<StockItem> StockItems => Set<StockItem>();
    public DbSet<MouvementStock> MouvementsStock => Set<MouvementStock>();
    public DbSet<Fournisseur> Fournisseurs => Set<Fournisseur>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
