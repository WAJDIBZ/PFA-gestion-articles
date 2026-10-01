using StockManagement.Domain.Common;

namespace StockManagement.Domain.Entities;

public class Fournisseur : BaseEntity
{
    public string Nom { get; set; } = string.Empty;
    public string? Contact { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public string? Adresse { get; set; }
    public bool EstActif { get; set; } = true;
}
