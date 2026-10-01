using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Domain.Entities;
using StockManagement.Infrastructure.Persistence;

namespace StockManagement.Infrastructure.Repositories;

public class StockRepository(AppDbContext context) : GenericRepository<StockItem>(context), IStockRepository
{
    public async Task<StockItem?> TrouverAsync(Guid articleId, Guid? articleVarianteId, Guid depotId, Guid? lotId, CancellationToken ct = default)
    {
        return await Set.FirstOrDefaultAsync(s =>
            s.ArticleId == articleId &&
            s.ArticleVarianteId == articleVarianteId &&
            s.DepotId == depotId &&
            s.LotId == lotId, ct);
    }

    public async Task<int> QuantiteTotaleAsync(Guid articleId, Guid? articleVarianteId = null, CancellationToken ct = default)
    {
        return await Set.Where(s => s.ArticleId == articleId && (articleVarianteId == null || s.ArticleVarianteId == articleVarianteId))
            .SumAsync(s => (int?)s.Quantite, ct) ?? 0;
    }

    public async Task<int> QuantiteDansDepotAsync(Guid articleId, Guid depotId, Guid? articleVarianteId = null, CancellationToken ct = default)
    {
        return await Set.Where(s => s.ArticleId == articleId && s.DepotId == depotId && (articleVarianteId == null || s.ArticleVarianteId == articleVarianteId))
            .SumAsync(s => (int?)s.Quantite, ct) ?? 0;
    }
}

public class MouvementRepository(AppDbContext context) : GenericRepository<MouvementStock>(context), IMouvementRepository
{
}
