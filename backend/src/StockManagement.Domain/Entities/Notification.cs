using StockManagement.Domain.Common;

namespace StockManagement.Domain.Entities;

public enum TypeNotification
{
    StockFaible = 0,
    StockEpuise = 1,
    ExpirationProche = 2,
    Information = 3
}

public class Notification : BaseEntity
{
    public TypeNotification Type { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool EstLue { get; set; }

    public Guid? ArticleId { get; set; }
    public Article? Article { get; set; }

    public Guid? DepotId { get; set; }
    public Depot? Depot { get; set; }

    public Guid? UtilisateurId { get; set; }
}
