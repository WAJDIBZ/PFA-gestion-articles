namespace StockManagement.Application.DTOs.Depots;

public record DepotDto(Guid Id, string Nom, string? Adresse, string? Responsable, bool EstActif, int NombreArticlesDistincts, int QuantiteTotale);
public record CreerDepotDto(string Nom, string? Adresse, string? Responsable);
public record ModifierDepotDto(string Nom, string? Adresse, string? Responsable, bool EstActif);
