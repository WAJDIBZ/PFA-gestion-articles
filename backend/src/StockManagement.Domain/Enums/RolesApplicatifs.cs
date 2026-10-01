namespace StockManagement.Domain.Enums;

public static class RolesApplicatifs
{
    public const string Administrateur = "Administrateur";
    public const string Magasinier = "Magasinier";
    public const string Consultant = "Consultant";

    public static readonly string[] Toutes = [Administrateur, Magasinier, Consultant];
}
