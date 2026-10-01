using StockManagement.Application.Common.Models;
using StockManagement.Application.DTOs.Catalogue;

namespace StockManagement.Application.Interfaces;

public interface IFamilleService
{
    Task<List<FamilleDto>> ObtenirToutesAsync(CancellationToken ct = default);
    Task<FamilleDto?> ObtenirParIdAsync(Guid id, CancellationToken ct = default);
    Task<ServiceResult<FamilleDto>> CreerAsync(CreerFamilleDto dto, CancellationToken ct = default);
    Task<ServiceResult<FamilleDto>> ModifierAsync(Guid id, ModifierFamilleDto dto, CancellationToken ct = default);
    Task<ServiceResult<bool>> SupprimerAsync(Guid id, CancellationToken ct = default);
}

public interface IMarqueService
{
    Task<List<MarqueDto>> ObtenirToutesAsync(CancellationToken ct = default);
    Task<ServiceResult<MarqueDto>> CreerAsync(CreerMarqueDto dto, CancellationToken ct = default);
    Task<ServiceResult<MarqueDto>> ModifierAsync(Guid id, ModifierMarqueDto dto, CancellationToken ct = default);
    Task<ServiceResult<bool>> SupprimerAsync(Guid id, CancellationToken ct = default);
}

public interface IUniteService
{
    Task<List<UniteDto>> ObtenirToutesAsync(CancellationToken ct = default);
    Task<ServiceResult<UniteDto>> CreerAsync(CreerUniteDto dto, CancellationToken ct = default);
    Task<ServiceResult<UniteDto>> ModifierAsync(Guid id, ModifierUniteDto dto, CancellationToken ct = default);
    Task<ServiceResult<bool>> SupprimerAsync(Guid id, CancellationToken ct = default);
}
