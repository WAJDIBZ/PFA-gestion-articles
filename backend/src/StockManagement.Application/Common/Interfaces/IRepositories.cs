using StockManagement.Domain.Entities;

namespace StockManagement.Application.Common.Interfaces;

public interface IArticleRepository : IGenericRepository<Article>
{
    Task<Article?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);
    Task<bool> ReferenceExisteAsync(string reference, Guid? ignorerId = null, CancellationToken ct = default);
}

public interface IArticleVarianteRepository : IGenericRepository<ArticleVariante>
{
    Task<ArticleVariante?> GetByIdWithValeursAsync(Guid id, CancellationToken ct = default);
    Task<ArticleVariante?> RechercherCombinaisonAsync(Guid articleId, List<Guid> valeurAttributIds, CancellationToken ct = default);
}

public interface IStockRepository : IGenericRepository<StockItem>
{
    Task<StockItem?> TrouverAsync(Guid articleId, Guid? articleVarianteId, Guid depotId, Guid? lotId, CancellationToken ct = default);
    Task<int> QuantiteTotaleAsync(Guid articleId, Guid? articleVarianteId = null, CancellationToken ct = default);
    Task<int> QuantiteDansDepotAsync(Guid articleId, Guid depotId, Guid? articleVarianteId = null, CancellationToken ct = default);
}

public interface IMouvementRepository : IGenericRepository<MouvementStock>
{
}
