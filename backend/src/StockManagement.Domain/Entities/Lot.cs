using StockManagement.Domain.Common;

namespace StockManagement.Domain.Entities;

public class Lot : BaseEntity
{
    public Guid ArticleId { get; set; }
    public Article Article { get; set; } = null!;

    public Guid? ArticleVarianteId { get; set; }
    public ArticleVariante? ArticleVariante { get; set; }

    public string NumeroLot { get; set; } = string.Empty;
    public DateTime? DateFabrication { get; set; }
    public DateTime? DateExpiration { get; set; }

    public ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();
    public ICollection<MouvementStock> Mouvements { get; set; } = new List<MouvementStock>();
}
