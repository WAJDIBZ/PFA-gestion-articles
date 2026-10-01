using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Application.DTOs.Notifications;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Entities;

namespace StockManagement.Application.Services;

public class NotificationService(IUnitOfWork uow) : INotificationService
{
    public async Task<List<NotificationDto>> ObtenirNonLuesAsync(CancellationToken ct = default)
    {
        return await uow.Notifications.Query()
            .Where(n => !n.EstLue && !n.EstSupprime)
            .OrderByDescending(n => n.DateCreation)
            .Select(n => new NotificationDto(n.Id, n.Type, n.Message, n.EstLue, n.DateCreation, n.ArticleId, n.DepotId))
            .ToListAsync(ct);
    }

    public async Task MarquerLueAsync(Guid id, CancellationToken ct = default)
    {
        var notification = await uow.Notifications.GetByIdAsync(id, ct);
        if (notification is null) return;

        notification.EstLue = true;
        uow.Notifications.Update(notification);
        await uow.SaveChangesAsync(ct);
    }

    public async Task CreerAlerteStockFaibleAsync(Guid articleId, Guid depotId, int quantiteActuelle, int seuil, CancellationToken ct = default)
    {
        var dejaExistante = await uow.Notifications.Query().AnyAsync(n =>
            n.ArticleId == articleId && n.DepotId == depotId && !n.EstLue &&
            (n.Type == TypeNotification.StockFaible || n.Type == TypeNotification.StockEpuise), ct);
        if (dejaExistante) return;

        var type = quantiteActuelle <= 0 ? TypeNotification.StockEpuise : TypeNotification.StockFaible;
        var message = quantiteActuelle <= 0
            ? "Article en rupture de stock dans ce dépôt."
            : $"Quantité faible ({quantiteActuelle}/{seuil}) pour cet article dans ce dépôt.";

        var notification = new Notification { Type = type, Message = message, ArticleId = articleId, DepotId = depotId };
        await uow.Notifications.AddAsync(notification, ct);
        await uow.SaveChangesAsync(ct);
    }
}
