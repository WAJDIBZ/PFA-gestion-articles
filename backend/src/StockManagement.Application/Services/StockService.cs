using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Application.DTOs.Stock;
using StockManagement.Application.Interfaces;

namespace StockManagement.Application.Services;

public class StockService(IUnitOfWork uow) : IStockService
{
    private static StockItemDto Projeter(Domain.Entities.StockItem s) => new(
        s.Id, s.ArticleId, s.Article.Designation, s.Article.Reference,
        s.ArticleVarianteId, s.ArticleVariante != null ? s.ArticleVariante.ReferenceVariante : null,
        s.DepotId, s.Depot.Nom, s.LotId, s.Lot != null ? s.Lot.NumeroLot : null,
        s.Quantite, s.Article.SeuilMinimum, s.Quantite <= s.Article.SeuilMinimum);

    public async Task<List<StockItemDto>> ObtenirParDepotAsync(Guid depotId, CancellationToken ct = default)
    {
        var items = await uow.StockItems.Query()
            .Include(s => s.Article)
            .Include(s => s.ArticleVariante)
            .Include(s => s.Depot)
            .Include(s => s.Lot)
            .Where(s => s.DepotId == depotId && !s.EstSupprime)
            .ToListAsync(ct);
        return items.Select(Projeter).ToList();
    }

    public async Task<StockParArticleDto?> ObtenirParArticleAsync(Guid articleId, CancellationToken ct = default)
    {
        var article = await uow.Articles.GetByIdAsync(articleId, ct);
        if (article is null) return null;

        var items = await uow.StockItems.Query()
            .Include(s => s.Article)
            .Include(s => s.ArticleVariante)
            .Include(s => s.Depot)
            .Include(s => s.Lot)
            .Where(s => s.ArticleId == articleId && !s.EstSupprime)
            .ToListAsync(ct);

        var repartition = items.Select(Projeter).ToList();
        return new StockParArticleDto(article.Id, article.Reference, article.Designation, repartition.Sum(r => r.Quantite), article.SeuilMinimum, repartition);
    }

    public async Task<List<StockItemDto>> ObtenirAlertesAsync(CancellationToken ct = default)
    {
        var items = await uow.StockItems.Query()
            .Include(s => s.Article)
            .Include(s => s.ArticleVariante)
            .Include(s => s.Depot)
            .Include(s => s.Lot)
            .Where(s => !s.EstSupprime && s.Quantite <= s.Article.SeuilMinimum)
            .ToListAsync(ct);
        return items.Select(Projeter).ToList();
    }
}
