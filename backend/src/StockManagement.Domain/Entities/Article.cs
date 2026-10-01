using StockManagement.Domain.Common;
using StockManagement.Domain.Enums;

namespace StockManagement.Domain.Entities;

public class Article : BaseEntity
{
    public string Reference { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CodeBarre { get; set; }
    public string? ImageUrl { get; set; }

    public Guid FamilleId { get; set; }
    public Famille Famille { get; set; } = null!;

    public Guid? MarqueId { get; set; }
    public Marque? Marque { get; set; }

    public Guid UniteId { get; set; }
    public Unite Unite { get; set; } = null!;

    public ModeSuivi ModeSuivi { get; set; } = ModeSuivi.Simple;
    public bool GereVariantes { get; set; }
    public bool SuiviDatePeremption { get; set; }

    public decimal PrixAchat { get; set; }
    public decimal PrixVente { get; set; }
    public int SeuilMinimum { get; set; }

    public bool EstActif { get; set; } = true;

    public ICollection<ArticleVariante> Variantes { get; set; } = new List<ArticleVariante>();
    public ICollection<AttributVarianteArticle> AttributsVariantes { get; set; } = new List<AttributVarianteArticle>();
    public ICollection<Lot> Lots { get; set; } = new List<Lot>();
    public ICollection<NumeroSerie> NumerosSerie { get; set; } = new List<NumeroSerie>();
    public ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();
    public ICollection<MouvementStock> Mouvements { get; set; } = new List<MouvementStock>();
}
