using StockManagement.Application.Common.Models;
using StockManagement.Application.DTOs.Fournisseurs;

namespace StockManagement.Application.Interfaces;

public interface IFournisseurService
{
    Task<List<FournisseurDto>> ObtenirTousAsync(CancellationToken ct = default);
    Task<ServiceResult<FournisseurDto>> CreerAsync(CreerFournisseurDto dto, CancellationToken ct = default);
    Task<ServiceResult<FournisseurDto>> ModifierAsync(Guid id, ModifierFournisseurDto dto, CancellationToken ct = default);
    Task<ServiceResult<bool>> SupprimerAsync(Guid id, CancellationToken ct = default);
}
