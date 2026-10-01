using StockManagement.Domain.Common;

namespace StockManagement.Domain.Entities;

/// <summary>Combinaison concrète de valeurs d'attributs pour un article (ex: Pull / Noir / M / Classique).</summary>
public class ArticleVariante : BaseEntity
{
    public Guid ArticleId { get; set; }
    public Article Article { get; set; } = null!;

    public string ReferenceVariante { get; set; } = string.Empty;
    public string? CodeBarre { get; set; }
    public bool EstActif { get; set; } = true;

    public ICollection<ArticleVarianteValeur> Valeurs { get; set; } = new List<ArticleVarianteValeur>();
    public ICollection<Lot> Lots { get; set; } = new List<Lot>();
    public ICollection<NumeroSerie> NumerosSerie { get; set; } = new List<NumeroSerie>();
    public ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();
    public ICollection<MouvementStock> Mouvements { get; set; } = new List<MouvementStock>();
}
