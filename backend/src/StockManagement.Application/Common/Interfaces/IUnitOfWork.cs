using StockManagement.Domain.Entities;

namespace StockManagement.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<Famille> Familles { get; }
    IGenericRepository<Marque> Marques { get; }
    IGenericRepository<Unite> Unites { get; }
    IArticleRepository Articles { get; }
    IGenericRepository<AttributVariante> AttributsVariantes { get; }
    IArticleVarianteRepository ArticleVariantes { get; }
    IGenericRepository<Depot> Depots { get; }
    IGenericRepository<Lot> Lots { get; }
    IGenericRepository<NumeroSerie> NumerosSerie { get; }
    IStockRepository StockItems { get; }
    IMouvementRepository Mouvements { get; }
    IGenericRepository<Fournisseur> Fournisseurs { get; }
    IGenericRepository<Notification> Notifications { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
