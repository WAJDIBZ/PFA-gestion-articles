using StockManagement.Domain.Common;

namespace StockManagement.Domain.Entities;

/// <summary>Table de liaison indiquant quels attributs (Taille, Couleur, ...) s'appliquent à un article.</summary>
public class AttributVarianteArticle
{
    public Guid ArticleId { get; set; }
    public Article Article { get; set; } = null!;

    public Guid AttributVarianteId { get; set; }
    public AttributVariante AttributVariante { get; set; } = null!;
}
