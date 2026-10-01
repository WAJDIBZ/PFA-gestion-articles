using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Application.DTOs.Dashboard;
using StockManagement.Application.DTOs.Stock;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Enums;

namespace StockManagement.Application.Services;

public class DashboardService(IUnitOfWork uow, IUserDirectoryService userDirectory) : IDashboardService
{
    public async Task<DashboardDto> ObtenirAsync(CancellationToken ct = default)
    {
        var nombreArticles = await uow.Articles.Query().CountAsync(a => !a.EstSupprime && a.EstActif, ct);
        var quantiteTotale = await uow.StockItems.Query().Where(s => !s.EstSupprime).SumAsync(s => (int?)s.Quantite, ct) ?? 0;
        var nombreDepots = await uow.Depots.Query().CountAsync(d => !d.EstSupprime && d.EstActif, ct);
        var nombreUtilisateurs = await userDirectory.CompterUtilisateursActifsAsync(ct);

        var articlesActifs = uow.Articles.Query().Where(a => !a.EstSupprime && a.EstActif);
        var alertes = await articlesActifs
            .Select(a => new { a.Id, a.Reference, a.Designation, a.SeuilMinimum, Quantite = a.StockItems.Sum(s => s.Quantite) })
            .Where(a => a.Quantite <= a.SeuilMinimum)
            .ToListAsync(ct);

        var derniersMouvements = await uow.Mouvements.Query()
            .Include(m => m.Article).Include(m => m.ArticleVariante).Include(m => m.DepotSource).Include(m => m.DepotDestination)
            .Where(m => !m.EstSupprime)
            .OrderByDescending(m => m.DateMouvement)
            .Take(10)
            .Select(m => new MouvementStockDto(
                m.Id, m.Type, m.ArticleId, m.Article.Designation, m.ArticleVarianteId,
                m.ArticleVariante != null ? m.ArticleVariante.ReferenceVariante : null,
                m.DepotSourceId, m.DepotSource != null ? m.DepotSource.Nom : null,
                m.DepotDestinationId, m.DepotDestination != null ? m.DepotDestination.Nom : null,
                m.Quantite, m.DateMouvement, m.Motif, m.Reference, m.UtilisateurNom))
            .ToListAsync(ct);

        return new DashboardDto(
            nombreArticles,
            quantiteTotale,
            alertes.Count(a => a.Quantite > 0),
            alertes.Count(a => a.Quantite <= 0),
            nombreDepots,
            nombreUtilisateurs,
            derniersMouvements,
            alertes.Select(a => new ArticleAlerteDto(a.Id, a.Reference, a.Designation, a.Quantite, a.SeuilMinimum)).ToList());
    }
}
