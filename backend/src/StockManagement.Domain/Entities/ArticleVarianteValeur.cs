namespace StockManagement.Domain.Entities;

/// <summary>Table de liaison entre une variante concrète et les valeurs d'attributs qui la composent.</summary>
public class ArticleVarianteValeur
{
    public Guid ArticleVarianteId { get; set; }
    public ArticleVariante ArticleVariante { get; set; } = null!;

    public Guid ValeurAttributId { get; set; }
    public ValeurAttribut ValeurAttribut { get; set; } = null!;
}
