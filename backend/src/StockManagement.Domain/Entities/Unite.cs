using StockManagement.Domain.Common;

namespace StockManagement.Domain.Entities;

public class Unite : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string Symbole { get; set; } = string.Empty;

    public ICollection<Article> Articles { get; set; } = new List<Article>();
}
