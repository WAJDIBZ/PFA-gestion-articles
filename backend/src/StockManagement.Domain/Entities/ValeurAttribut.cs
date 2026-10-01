using StockManagement.Domain.Common;

namespace StockManagement.Domain.Entities;

/// <summary>Valeur possible d'un attribut, ex: Taille=M, Couleur=Noir.</summary>
public class ValeurAttribut : BaseEntity
{
    public Guid AttributVarianteId { get; set; }
    public AttributVariante AttributVariante { get; set; } = null!;

    public string Valeur { get; set; } = string.Empty;

    public ICollection<ArticleVarianteValeur> VariantesAssociees { get; set; } = new List<ArticleVarianteValeur>();
}
