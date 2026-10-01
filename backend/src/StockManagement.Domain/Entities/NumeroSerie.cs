using StockManagement.Domain.Common;

namespace StockManagement.Domain.Entities;

public enum StatutNumeroSerie
{
    EnStock = 0,
    Vendu = 1,
    Retourne = 2,
    HorsService = 3
}

public class NumeroSerie : BaseEntity
{
    public Guid ArticleId { get; set; }
    public Article Article { get; set; } = null!;

    public Guid? ArticleVarianteId { get; set; }
    public ArticleVariante? ArticleVariante { get; set; }

    public string Numero { get; set; } = string.Empty;

    public Guid? DepotId { get; set; }
    public Depot? Depot { get; set; }

    public StatutNumeroSerie Statut { get; set; } = StatutNumeroSerie.EnStock;

    public ICollection<MouvementStock> Mouvements { get; set; } = new List<MouvementStock>();
}
