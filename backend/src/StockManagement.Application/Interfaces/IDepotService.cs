using StockManagement.Application.Common.Models;
using StockManagement.Application.DTOs.Depots;

namespace StockManagement.Application.Interfaces;

public interface IDepotService
{
    Task<List<DepotDto>> ObtenirTousAsync(CancellationToken ct = default);
    Task<DepotDto?> ObtenirParIdAsync(Guid id, CancellationToken ct = default);
    Task<ServiceResult<DepotDto>> CreerAsync(CreerDepotDto dto, CancellationToken ct = default);
    Task<ServiceResult<DepotDto>> ModifierAsync(Guid id, ModifierDepotDto dto, CancellationToken ct = default);
    Task<ServiceResult<bool>> SupprimerAsync(Guid id, CancellationToken ct = default);
}
