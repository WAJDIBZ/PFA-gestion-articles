using StockManagement.Domain.Entities;

namespace StockManagement.Application.DTOs.Notifications;

public record NotificationDto(Guid Id, TypeNotification Type, string Message, bool EstLue, DateTime DateCreation, Guid? ArticleId, Guid? DepotId);
