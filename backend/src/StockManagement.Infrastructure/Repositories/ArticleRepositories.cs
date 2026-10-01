using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Domain.Entities;
using StockManagement.Infrastructure.Persistence;

namespace StockManagement.Infrastructure.Repositories;

public class ArticleRepository(AppDbContext context) : GenericRepository<Article>(context), IArticleRepository
{
    public async Task<Article?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default)
    {
        return await Set
            .Include(a => a.Famille)
            .Include(a => a.Marque)
            .Include(a => a.Unite)
            .Include(a => a.Variantes).ThenInclude(v => v.Valeurs).ThenInclude(vv => vv.ValeurAttribut)
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    }

    public async Task<bool> ReferenceExisteAsync(string reference, Guid? ignorerId = null, CancellationToken ct = default)
    {
        return await Set.AnyAsync(a => a.Reference == reference && (ignorerId == null || a.Id != ignorerId), ct);
    }
}

public class ArticleVarianteRepository(AppDbContext context) : GenericRepository<ArticleVariante>(context), IArticleVarianteRepository
{
    public async Task<ArticleVariante?> GetByIdWithValeursAsync(Guid id, CancellationToken ct = default)
    {
        return await Set.Include(v => v.Valeurs).ThenInclude(vv => vv.ValeurAttribut).FirstOrDefaultAsync(v => v.Id == id, ct);
    }

    public async Task<ArticleVariante?> RechercherCombinaisonAsync(Guid articleId, List<Guid> valeurAttributIds, CancellationToken ct = default)
    {
        var candidates = await Set
            .Include(v => v.Valeurs)
            .Include(v => v.StockItems)
            .Where(v => v.ArticleId == articleId)
            .ToListAsync(ct);

        var recherchees = valeurAttributIds.OrderBy(x => x).ToList();
        return candidates.FirstOrDefault(v =>
            v.Valeurs.Select(vv => vv.ValeurAttributId).OrderBy(x => x).SequenceEqual(recherchees));
    }
}
