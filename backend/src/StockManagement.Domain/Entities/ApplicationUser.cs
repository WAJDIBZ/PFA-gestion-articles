using Microsoft.AspNetCore.Identity;

namespace StockManagement.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string NomComplet { get; set; } = string.Empty;
    public bool EstActif { get; set; } = true;
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;
}
