using StockManagement.Domain.Common;

namespace StockManagement.Domain.Entities;

/// <summary>Attribut de variante, ex: Taille, Couleur, Coupe.</summary>
public class AttributVariante : BaseEntity
{
    public string Nom { get; set; } = string.Empty;

    public ICollection<ValeurAttribut> Valeurs { get; set; } = new List<ValeurAttribut>();
    public ICollection<AttributVarianteArticle> ArticlesAssocies { get; set; } = new List<AttributVarianteArticle>();
}
