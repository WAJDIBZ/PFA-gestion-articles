namespace StockManagement.Application.DTOs.Fournisseurs;

public record FournisseurDto(Guid Id, string Nom, string? Contact, string? Email, string? Telephone, string? Adresse, bool EstActif);
public record CreerFournisseurDto(string Nom, string? Contact, string? Email, string? Telephone, string? Adresse);
public record ModifierFournisseurDto(string Nom, string? Contact, string? Email, string? Telephone, string? Adresse, bool EstActif);
