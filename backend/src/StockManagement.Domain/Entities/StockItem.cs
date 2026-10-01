using StockManagement.Domain.Common;

namespace StockManagement.Domain.Entities;

/// <summary>Quantité disponible d'un article (et éventuellement variante/lot) dans un dépôt donné.</summary>
public class StockItem : BaseEntity
{
    public Guid ArticleId { get; set; }
    public Article Article { get; set; } = null!;

    public Guid? ArticleVarianteId { get; set; }
    public ArticleVariante? ArticleVariante { get; set; }

    public Guid DepotId { get; set; }
    public Depot Depot { get; set; } = null!;

    public Guid? LotId { get; set; }
    public Lot? Lot { get; set; }

    public int Quantite { get; set; }
}
