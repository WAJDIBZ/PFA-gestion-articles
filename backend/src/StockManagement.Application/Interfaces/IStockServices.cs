using StockManagement.Application.Common.Models;
using StockManagement.Application.DTOs.Stock;

namespace StockManagement.Application.Interfaces;

public interface IStockService
{
    Task<List<StockItemDto>> ObtenirParDepotAsync(Guid depotId, CancellationToken ct = default);
    Task<StockParArticleDto?> ObtenirParArticleAsync(Guid articleId, CancellationToken ct = default);
    Task<List<StockItemDto>> ObtenirAlertesAsync(CancellationToken ct = default);
}

public interface IMouvementService
{
    Task<PagedResult<MouvementStockDto>> RechercherAsync(FiltreMouvementDto filtre, CancellationToken ct = default);
    Task<ServiceResult<MouvementStockDto>> EnregistrerEntreeAsync(CreerEntreeStockDto dto, CancellationToken ct = default);
    Task<ServiceResult<MouvementStockDto>> EnregistrerSortieAsync(CreerSortieStockDto dto, CancellationToken ct = default);
    Task<ServiceResult<List<MouvementStockDto>>> EnregistrerTransfertAsync(CreerTransfertStockDto dto, CancellationToken ct = default);
}
