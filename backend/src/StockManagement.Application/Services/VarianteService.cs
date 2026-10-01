using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Application.Common.Models;
using StockManagement.Application.DTOs.Catalogue;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Entities;

namespace StockManagement.Application.Services;

public class VarianteService(IUnitOfWork uow) : IVarianteService
{
    public async Task<List<AttributVarianteDto>> ObtenirAttributsAsync(CancellationToken ct = default)
    {
        return await uow.AttributsVariantes.Query().Where(a => !a.EstSupprime)
            .Select(a => new AttributVarianteDto(a.Id, a.Nom, a.Valeurs.Select(v => new ValeurAttributDto(v.Id, v.Valeur)).ToList()))
            .ToListAsync(ct);
    }

    public async Task<ServiceResult<AttributVarianteDto>> CreerAttributAsync(CreerAttributVarianteDto dto, CancellationToken ct = default)
    {
        var attribut = new AttributVariante { Nom = dto.Nom };
        await uow.AttributsVariantes.AddAsync(attribut, ct);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<AttributVarianteDto>.Succes(new AttributVarianteDto(attribut.Id, attribut.Nom, []));
    }

    public async Task<ServiceResult<ValeurAttributDto>> CreerValeurAsync(CreerValeurAttributDto dto, CancellationToken ct = default)
    {
        var attribut = await uow.AttributsVariantes.GetByIdAsync(dto.AttributVarianteId, ct);
        if (attribut is null) return ServiceResult<ValeurAttributDto>.Echec("Attribut introuvable.");

        var valeur = new ValeurAttribut { AttributVarianteId = dto.AttributVarianteId, Valeur = dto.Valeur };
        attribut.Valeurs.Add(valeur);
        uow.AttributsVariantes.Update(attribut);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<ValeurAttributDto>.Succes(new ValeurAttributDto(valeur.Id, valeur.Valeur));
    }

    public async Task<List<ArticleVarianteDto>> ObtenirParArticleAsync(Guid articleId, CancellationToken ct = default)
    {
        return await uow.ArticleVariantes.Query()
            .Where(v => v.ArticleId == articleId && !v.EstSupprime)
            .Select(v => new ArticleVarianteDto(
                v.Id, v.ArticleId, v.ReferenceVariante, v.CodeBarre, v.EstActif,
                v.StockItems.Sum(s => s.Quantite),
                v.Valeurs.Select(vv => new ValeurAttributDto(vv.ValeurAttribut.Id, vv.ValeurAttribut.Valeur)).ToList()))
            .ToListAsync(ct);
    }

    public async Task<ServiceResult<ArticleVarianteDto>> CreerVarianteAsync(CreerArticleVarianteDto dto, CancellationToken ct = default)
    {
        var article = await uow.Articles.GetByIdAsync(dto.ArticleId, ct);
        if (article is null) return ServiceResult<ArticleVarianteDto>.Echec("Article introuvable.");

        var existante = await uow.ArticleVariantes.RechercherCombinaisonAsync(dto.ArticleId, dto.ValeurAttributIds, ct);
        if (existante is not null) return ServiceResult<ArticleVarianteDto>.Echec("Cette combinaison de variante existe déjà pour cet article.");

        var variante = new ArticleVariante { ArticleId = dto.ArticleId, ReferenceVariante = dto.ReferenceVariante, CodeBarre = dto.CodeBarre };
        foreach (var valeurId in dto.ValeurAttributIds)
            variante.Valeurs.Add(new ArticleVarianteValeur { ArticleVarianteId = variante.Id, ValeurAttributId = valeurId });

        await uow.ArticleVariantes.AddAsync(variante, ct);
        await uow.SaveChangesAsync(ct);

        return ServiceResult<ArticleVarianteDto>.Succes(new ArticleVarianteDto(variante.Id, variante.ArticleId, variante.ReferenceVariante, variante.CodeBarre, variante.EstActif, 0, []));
    }

    public async Task<ResultatCombinaisonDto> RechercherCombinaisonAsync(RechercheCombinaisonDto dto, CancellationToken ct = default)
    {
        var variante = await uow.ArticleVariantes.RechercherCombinaisonAsync(dto.ArticleId, dto.ValeurAttributIds, ct);
        if (variante is null) return new ResultatCombinaisonDto(false, null, null, 0);

        var quantite = variante.StockItems.Sum(s => s.Quantite);
        return new ResultatCombinaisonDto(quantite > 0, variante.Id, variante.ReferenceVariante, quantite);
    }
}
