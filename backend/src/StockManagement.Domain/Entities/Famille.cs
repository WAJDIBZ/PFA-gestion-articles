using StockManagement.Domain.Common;

namespace StockManagement.Domain.Entities;

public class Famille : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? FamilleParentId { get; set; }
    public Famille? FamilleParent { get; set; }

    public ICollection<Famille> SousFamilles { get; set; } = new List<Famille>();
    public ICollection<Article> Articles { get; set; } = new List<Article>();
}
