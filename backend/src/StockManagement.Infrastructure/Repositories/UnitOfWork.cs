using StockManagement.Application.Common.Interfaces;
using StockManagement.Domain.Entities;
using StockManagement.Infrastructure.Persistence;

namespace StockManagement.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Familles = new GenericRepository<Famille>(context);
        Marques = new GenericRepository<Marque>(context);
        Unites = new GenericRepository<Unite>(context);
        Articles = new ArticleRepository(context);
        AttributsVariantes = new GenericRepository<AttributVariante>(context);
        ArticleVariantes = new ArticleVarianteRepository(context);
        Depots = new GenericRepository<Depot>(context);
        Lots = new GenericRepository<Lot>(context);
        NumerosSerie = new GenericRepository<NumeroSerie>(context);
        StockItems = new StockRepository(context);
        Mouvements = new MouvementRepository(context);
        Fournisseurs = new GenericRepository<Fournisseur>(context);
        Notifications = new GenericRepository<Notification>(context);
    }

    public IGenericRepository<Famille> Familles { get; }
    public IGenericRepository<Marque> Marques { get; }
    public IGenericRepository<Unite> Unites { get; }
    public IArticleRepository Articles { get; }
    public IGenericRepository<AttributVariante> AttributsVariantes { get; }
    public IArticleVarianteRepository ArticleVariantes { get; }
    public IGenericRepository<Depot> Depots { get; }
    public IGenericRepository<Lot> Lots { get; }
    public IGenericRepository<NumeroSerie> NumerosSerie { get; }
    public IStockRepository StockItems { get; }
    public IMouvementRepository Mouvements { get; }
    public IGenericRepository<Fournisseur> Fournisseurs { get; }
    public IGenericRepository<Notification> Notifications { get; }

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);
}
