using StockManagement.Application.Common.Models;
using StockManagement.Application.DTOs.Catalogue;

namespace StockManagement.Application.Interfaces;

public interface IArticleService
{
    Task<PagedResult<ArticleDto>> RechercherAsync(string? texte, Guid? familleId, bool? actifSeulement, int page, int pageSize, CancellationToken ct = default);
    Task<ArticleDto?> ObtenirParIdAsync(Guid id, CancellationToken ct = default);
    Task<ServiceResult<ArticleDto>> CreerAsync(CreerArticleDto dto, CancellationToken ct = default);
    Task<ServiceResult<ArticleDto>> ModifierAsync(Guid id, ModifierArticleDto dto, CancellationToken ct = default);
    Task<ServiceResult<bool>> SupprimerAsync(Guid id, CancellationToken ct = default);
}

public interface IVarianteService
{
    Task<List<AttributVarianteDto>> ObtenirAttributsAsync(CancellationToken ct = default);
    Task<ServiceResult<AttributVarianteDto>> CreerAttributAsync(CreerAttributVarianteDto dto, CancellationToken ct = default);
    Task<ServiceResult<ValeurAttributDto>> CreerValeurAsync(CreerValeurAttributDto dto, CancellationToken ct = default);

    Task<List<ArticleVarianteDto>> ObtenirParArticleAsync(Guid articleId, CancellationToken ct = default);
    Task<ServiceResult<ArticleVarianteDto>> CreerVarianteAsync(CreerArticleVarianteDto dto, CancellationToken ct = default);
    Task<ResultatCombinaisonDto> RechercherCombinaisonAsync(RechercheCombinaisonDto dto, CancellationToken ct = default);
}
