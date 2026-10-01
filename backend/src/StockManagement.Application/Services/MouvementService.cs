using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Application.Common.Models;
using StockManagement.Application.DTOs.Stock;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Entities;
using StockManagement.Domain.Enums;

namespace StockManagement.Application.Services;

public class MouvementService(IUnitOfWork uow, ICurrentUserService currentUser, INotificationService notificationService) : IMouvementService
{
    private static MouvementStockDto Projeter(MouvementStock m) => new(
        m.Id, m.Type, m.ArticleId, m.Article.Designation, m.ArticleVarianteId,
        m.ArticleVariante != null ? m.ArticleVariante.ReferenceVariante : null,
        m.DepotSourceId, m.DepotSource != null ? m.DepotSource.Nom : null,
        m.DepotDestinationId, m.DepotDestination != null ? m.DepotDestination.Nom : null,
        m.Quantite, m.DateMouvement, m.Motif, m.Reference, m.UtilisateurNom);

    public async Task<PagedResult<MouvementStockDto>> RechercherAsync(FiltreMouvementDto filtre, CancellationToken ct = default)
    {
        var query = uow.Mouvements.Query()
            .Include(m => m.Article)
            .Include(m => m.ArticleVariante)
            .Include(m => m.DepotSource)
            .Include(m => m.DepotDestination)
            .Where(m => !m.EstSupprime);

        if (filtre.ArticleId.HasValue) query = query.Where(m => m.ArticleId == filtre.ArticleId);
        if (filtre.DepotId.HasValue) query = query.Where(m => m.DepotSourceId == filtre.DepotId || m.DepotDestinationId == filtre.DepotId);
        if (filtre.Type.HasValue) query = query.Where(m => m.Type == filtre.Type);
        if (filtre.DateDebut.HasValue) query = query.Where(m => m.DateMouvement >= filtre.DateDebut);
        if (filtre.DateFin.HasValue) query = query.Where(m => m.DateMouvement <= filtre.DateFin);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(m => m.DateMouvement)
            .Skip((filtre.Page - 1) * filtre.PageSize)
            .Take(filtre.PageSize)
            .ToListAsync(ct);

        return new PagedResult<MouvementStockDto>
        {
            Items = items.Select(Projeter).ToList(),
            TotalCount = total,
            Page = filtre.Page,
            PageSize = filtre.PageSize
        };
    }

    public async Task<ServiceResult<MouvementStockDto>> EnregistrerEntreeAsync(CreerEntreeStockDto dto, CancellationToken ct = default)
    {
        if (dto.Quantite <= 0) return ServiceResult<MouvementStockDto>.Echec("La quantité doit être positive.");

        var article = await uow.Articles.GetByIdAsync(dto.ArticleId, ct);
        if (article is null) return ServiceResult<MouvementStockDto>.Echec("Article introuvable.");

        var depot = await uow.Depots.GetByIdAsync(dto.DepotDestinationId, ct);
        if (depot is null) return ServiceResult<MouvementStockDto>.Echec("Dépôt introuvable.");

        Guid? lotId = null;
        if (article.ModeSuivi == ModeSuivi.Lot && !string.IsNullOrWhiteSpace(dto.NumeroLot))
        {
            var lot = new Lot
            {
                ArticleId = dto.ArticleId,
                ArticleVarianteId = dto.ArticleVarianteId,
                NumeroLot = dto.NumeroLot,
                DateExpiration = dto.DateExpirationLot
            };
            await uow.Lots.AddAsync(lot, ct);
            lotId = lot.Id;
        }

        if (article.ModeSuivi == ModeSuivi.NumeroSerie && !string.IsNullOrWhiteSpace(dto.NumeroSerie))
        {
            var numeroSerie = new NumeroSerie
            {
                ArticleId = dto.ArticleId,
                ArticleVarianteId = dto.ArticleVarianteId,
                Numero = dto.NumeroSerie,
                DepotId = dto.DepotDestinationId,
                Statut = StatutNumeroSerie.EnStock
            };
            await uow.NumerosSerie.AddAsync(numeroSerie, ct);
        }

        var stockItem = await uow.StockItems.TrouverAsync(dto.ArticleId, dto.ArticleVarianteId, dto.DepotDestinationId, lotId, ct);
        if (stockItem is null)
        {
            stockItem = new StockItem { ArticleId = dto.ArticleId, ArticleVarianteId = dto.ArticleVarianteId, DepotId = dto.DepotDestinationId, LotId = lotId, Quantite = 0 };
            await uow.StockItems.AddAsync(stockItem, ct);
        }
        stockItem.Quantite += dto.Quantite;
        stockItem.DateModification = DateTime.UtcNow;
        uow.StockItems.Update(stockItem);

        var mouvement = new MouvementStock
        {
            Type = TypeMouvement.Entree,
            ArticleId = dto.ArticleId,
            ArticleVarianteId = dto.ArticleVarianteId,
            LotId = lotId,
            DepotDestinationId = dto.DepotDestinationId,
            Quantite = dto.Quantite,
            Motif = dto.Motif,
            Reference = dto.Reference,
            UtilisateurId = currentUser.UtilisateurId ?? Guid.Empty,
            UtilisateurNom = currentUser.NomComplet ?? "Système"
        };
        await uow.Mouvements.AddAsync(mouvement, ct);
        await uow.SaveChangesAsync(ct);

        mouvement = await uow.Mouvements.Query()
            .Include(m => m.Article).Include(m => m.ArticleVariante).Include(m => m.DepotDestination)
            .FirstAsync(m => m.Id == mouvement.Id, ct);

        return ServiceResult<MouvementStockDto>.Succes(Projeter(mouvement));
    }

    public async Task<ServiceResult<MouvementStockDto>> EnregistrerSortieAsync(CreerSortieStockDto dto, CancellationToken ct = default)
    {
        if (dto.Quantite <= 0) return ServiceResult<MouvementStockDto>.Echec("La quantité doit être positive.");

        var article = await uow.Articles.GetByIdAsync(dto.ArticleId, ct);
        if (article is null) return ServiceResult<MouvementStockDto>.Echec("Article introuvable.");

        var stockItem = await uow.StockItems.TrouverAsync(dto.ArticleId, dto.ArticleVarianteId, dto.DepotSourceId, dto.LotId, ct);
        if (stockItem is null || stockItem.Quantite < dto.Quantite)
            return ServiceResult<MouvementStockDto>.Echec("Quantité insuffisante en stock dans ce dépôt.");

        stockItem.Quantite -= dto.Quantite;
        stockItem.DateModification = DateTime.UtcNow;
        uow.StockItems.Update(stockItem);

        if (!string.IsNullOrWhiteSpace(dto.NumeroSerie))
        {
            var numeroSerie = await uow.NumerosSerie.Query()
                .FirstOrDefaultAsync(n => n.ArticleId == dto.ArticleId && n.Numero == dto.NumeroSerie && n.Statut == StatutNumeroSerie.EnStock, ct);
            if (numeroSerie is not null)
            {
                numeroSerie.Statut = StatutNumeroSerie.Vendu;
                uow.NumerosSerie.Update(numeroSerie);
            }
        }

        var mouvement = new MouvementStock
        {
            Type = TypeMouvement.Sortie,
            ArticleId = dto.ArticleId,
            ArticleVarianteId = dto.ArticleVarianteId,
            LotId = dto.LotId,
            DepotSourceId = dto.DepotSourceId,
            Quantite = dto.Quantite,
            Motif = dto.Motif,
            Reference = dto.Reference,
            UtilisateurId = currentUser.UtilisateurId ?? Guid.Empty,
            UtilisateurNom = currentUser.NomComplet ?? "Système"
        };
        await uow.Mouvements.AddAsync(mouvement, ct);
        await uow.SaveChangesAsync(ct);

        await VerifierSeuilAsync(article, dto.ArticleVarianteId, dto.DepotSourceId, ct);

        mouvement = await uow.Mouvements.Query()
            .Include(m => m.Article).Include(m => m.ArticleVariante).Include(m => m.DepotSource)
            .FirstAsync(m => m.Id == mouvement.Id, ct);

        return ServiceResult<MouvementStockDto>.Succes(Projeter(mouvement));
    }

    public async Task<ServiceResult<List<MouvementStockDto>>> EnregistrerTransfertAsync(CreerTransfertStockDto dto, CancellationToken ct = default)
    {
        if (dto.Quantite <= 0) return ServiceResult<List<MouvementStockDto>>.Echec("La quantité doit être positive.");
        if (dto.DepotSourceId == dto.DepotDestinationId) return ServiceResult<List<MouvementStockDto>>.Echec("Le dépôt source et destination doivent être différents.");

        var article = await uow.Articles.GetByIdAsync(dto.ArticleId, ct);
        if (article is null) return ServiceResult<List<MouvementStockDto>>.Echec("Article introuvable.");

        var stockSource = await uow.StockItems.TrouverAsync(dto.ArticleId, dto.ArticleVarianteId, dto.DepotSourceId, dto.LotId, ct);
        if (stockSource is null || stockSource.Quantite < dto.Quantite)
            return ServiceResult<List<MouvementStockDto>>.Echec("Quantité insuffisante dans le dépôt source.");

        stockSource.Quantite -= dto.Quantite;
        stockSource.DateModification = DateTime.UtcNow;
        uow.StockItems.Update(stockSource);

        var stockDestination = await uow.StockItems.TrouverAsync(dto.ArticleId, dto.ArticleVarianteId, dto.DepotDestinationId, dto.LotId, ct);
        if (stockDestination is null)
        {
            stockDestination = new StockItem { ArticleId = dto.ArticleId, ArticleVarianteId = dto.ArticleVarianteId, DepotId = dto.DepotDestinationId, LotId = dto.LotId, Quantite = 0 };
            await uow.StockItems.AddAsync(stockDestination, ct);
        }
        stockDestination.Quantite += dto.Quantite;
        stockDestination.DateModification = DateTime.UtcNow;
        uow.StockItems.Update(stockDestination);

        var mouvementLieId = Guid.NewGuid();
        var utilisateurId = currentUser.UtilisateurId ?? Guid.Empty;
        var utilisateurNom = currentUser.NomComplet ?? "Système";

        var mouvementSortie = new MouvementStock
        {
            Type = TypeMouvement.TransfertSortie,
            ArticleId = dto.ArticleId,
            ArticleVarianteId = dto.ArticleVarianteId,
            LotId = dto.LotId,
            DepotSourceId = dto.DepotSourceId,
            DepotDestinationId = dto.DepotDestinationId,
            Quantite = dto.Quantite,
            Motif = dto.Motif,
            Reference = dto.Reference,
            MouvementLieId = mouvementLieId,
            UtilisateurId = utilisateurId,
            UtilisateurNom = utilisateurNom
        };
        var mouvementEntree = new MouvementStock
        {
            Type = TypeMouvement.TransfertEntree,
            ArticleId = dto.ArticleId,
            ArticleVarianteId = dto.ArticleVarianteId,
            LotId = dto.LotId,
            DepotSourceId = dto.DepotSourceId,
            DepotDestinationId = dto.DepotDestinationId,
            Quantite = dto.Quantite,
            Motif = dto.Motif,
            Reference = dto.Reference,
            MouvementLieId = mouvementLieId,
            UtilisateurId = utilisateurId,
            UtilisateurNom = utilisateurNom
        };

        await uow.Mouvements.AddAsync(mouvementSortie, ct);
        await uow.Mouvements.AddAsync(mouvementEntree, ct);
        await uow.SaveChangesAsync(ct);

        await VerifierSeuilAsync(article, dto.ArticleVarianteId, dto.DepotSourceId, ct);

        var mouvements = await uow.Mouvements.Query()
            .Include(m => m.Article).Include(m => m.ArticleVariante).Include(m => m.DepotSource).Include(m => m.DepotDestination)
            .Where(m => m.MouvementLieId == mouvementLieId)
            .ToListAsync(ct);

        return ServiceResult<List<MouvementStockDto>>.Succes(mouvements.Select(Projeter).ToList());
    }

    private async Task VerifierSeuilAsync(Article article, Guid? articleVarianteId, Guid depotId, CancellationToken ct)
    {
        var quantiteRestante = await uow.StockItems.QuantiteDansDepotAsync(article.Id, depotId, articleVarianteId, ct);
        if (quantiteRestante <= article.SeuilMinimum)
            await notificationService.CreerAlerteStockFaibleAsync(article.Id, depotId, quantiteRestante, article.SeuilMinimum, ct);
    }
}
