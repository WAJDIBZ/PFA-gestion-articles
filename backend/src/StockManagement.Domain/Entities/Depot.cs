using StockManagement.Domain.Common;

namespace StockManagement.Domain.Entities;

public class Depot : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string? Adresse { get; set; }
    public string? Responsable { get; set; }
    public bool EstActif { get; set; } = true;

    public ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();
    public ICollection<NumeroSerie> NumerosSerie { get; set; } = new List<NumeroSerie>();
}
