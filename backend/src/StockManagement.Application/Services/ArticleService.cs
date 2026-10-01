using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Application.Common.Models;
using StockManagement.Application.DTOs.Catalogue;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Entities;

namespace StockManagement.Application.Services;

public class ArticleService(IUnitOfWork uow) : IArticleService
{
    public async Task<PagedResult<ArticleDto>> RechercherAsync(string? texte, Guid? familleId, bool? actifSeulement, int page, int pageSize, CancellationToken ct = default)
    {
        var query = uow.Articles.Query().Where(a => !a.EstSupprime);

        if (!string.IsNullOrWhiteSpace(texte))
        {
            var t = texte.Trim().ToLower();
            query = query.Where(a => a.Reference.ToLower().Contains(t) || a.Designation.ToLower().Contains(t));
        }
        if (familleId.HasValue) query = query.Where(a => a.FamilleId == familleId);
        if (actifSeulement.HasValue) query = query.Where(a => a.EstActif == actifSeulement);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(a => a.Designation)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new ArticleDto(
                a.Id, a.Reference, a.Designation, a.Description, a.CodeBarre, a.ImageUrl,
                a.FamilleId, a.Famille.Nom, a.MarqueId, a.Marque != null ? a.Marque.Nom : null,
                a.UniteId, a.Unite.Nom, a.ModeSuivi, a.GereVariantes, a.SuiviDatePeremption,
                a.PrixAchat, a.PrixVente, a.SeuilMinimum, a.EstActif,
                a.StockItems.Sum(s => s.Quantite)))
            .ToListAsync(ct);

        return new PagedResult<ArticleDto> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
    }

    public async Task<ArticleDto?> ObtenirParIdAsync(Guid id, CancellationToken ct = default)
    {
        return await uow.Articles.Query().Where(a => a.Id == id && !a.EstSupprime)
            .Select(a => new ArticleDto(
                a.Id, a.Reference, a.Designation, a.Description, a.CodeBarre, a.ImageUrl,
                a.FamilleId, a.Famille.Nom, a.MarqueId, a.Marque != null ? a.Marque.Nom : null,
                a.UniteId, a.Unite.Nom, a.ModeSuivi, a.GereVariantes, a.SuiviDatePeremption,
                a.PrixAchat, a.PrixVente, a.SeuilMinimum, a.EstActif,
                a.StockItems.Sum(s => s.Quantite)))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<ServiceResult<ArticleDto>> CreerAsync(CreerArticleDto dto, CancellationToken ct = default)
    {
        if (await uow.Articles.ReferenceExisteAsync(dto.Reference, null, ct))
            return ServiceResult<ArticleDto>.Echec("Cette référence existe déjà.");

        var article = new Article
        {
            Reference = dto.Reference,
            Designation = dto.Designation,
            Description = dto.Description,
            CodeBarre = dto.CodeBarre,
            ImageUrl = dto.ImageUrl,
            FamilleId = dto.FamilleId,
            MarqueId = dto.MarqueId,
            UniteId = dto.UniteId,
            ModeSuivi = dto.ModeSuivi,
            GereVariantes = dto.GereVariantes,
            SuiviDatePeremption = dto.SuiviDatePeremption,
            PrixAchat = dto.PrixAchat,
            PrixVente = dto.PrixVente,
            SeuilMinimum = dto.SeuilMinimum
        };

        if (dto.AttributsVarianteIds is { Count: > 0 })
        {
            foreach (var attributId in dto.AttributsVarianteIds)
                article.AttributsVariantes.Add(new AttributVarianteArticle { ArticleId = article.Id, AttributVarianteId = attributId });
        }

        await uow.Articles.AddAsync(article, ct);
        await uow.SaveChangesAsync(ct);

        var resultat = await ObtenirParIdAsync(article.Id, ct);
        return ServiceResult<ArticleDto>.Succes(resultat!);
    }

    public async Task<ServiceResult<ArticleDto>> ModifierAsync(Guid id, ModifierArticleDto dto, CancellationToken ct = default)
    {
        var article = await uow.Articles.GetByIdAsync(id, ct);
        if (article is null) return ServiceResult<ArticleDto>.Echec("Article introuvable.");

        article.Designation = dto.Designation;
        article.Description = dto.Description;
        article.CodeBarre = dto.CodeBarre;
        article.ImageUrl = dto.ImageUrl;
        article.FamilleId = dto.FamilleId;
        article.MarqueId = dto.MarqueId;
        article.UniteId = dto.UniteId;
        article.ModeSuivi = dto.ModeSuivi;
        article.SuiviDatePeremption = dto.SuiviDatePeremption;
        article.PrixAchat = dto.PrixAchat;
        article.PrixVente = dto.PrixVente;
        article.SeuilMinimum = dto.SeuilMinimum;
        article.EstActif = dto.EstActif;
        article.DateModification = DateTime.UtcNow;

        uow.Articles.Update(article);
        await uow.SaveChangesAsync(ct);

        var resultat = await ObtenirParIdAsync(article.Id, ct);
        return ServiceResult<ArticleDto>.Succes(resultat!);
    }

    public async Task<ServiceResult<bool>> SupprimerAsync(Guid id, CancellationToken ct = default)
    {
        var article = await uow.Articles.GetByIdAsync(id, ct);
        if (article is null) return ServiceResult<bool>.Echec("Article introuvable.");

        article.EstSupprime = true;
        uow.Articles.Update(article);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<bool>.Succes(true);
    }
}
