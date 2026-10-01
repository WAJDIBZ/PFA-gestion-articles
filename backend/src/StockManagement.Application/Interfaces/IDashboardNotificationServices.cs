using StockManagement.Application.DTOs.Dashboard;
using StockManagement.Application.DTOs.Notifications;

namespace StockManagement.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> ObtenirAsync(CancellationToken ct = default);
}

public interface INotificationService
{
    Task<List<NotificationDto>> ObtenirNonLuesAsync(CancellationToken ct = default);
    Task MarquerLueAsync(Guid id, CancellationToken ct = default);
    Task CreerAlerteStockFaibleAsync(Guid articleId, Guid depotId, int quantiteActuelle, int seuil, CancellationToken ct = default);
}
