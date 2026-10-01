using StockManagement.Domain.Common;

namespace StockManagement.Domain.Entities;

public class Marque : BaseEntity
{
    public string Nom { get; set; } = string.Empty;

    public ICollection<Article> Articles { get; set; } = new List<Article>();
}
