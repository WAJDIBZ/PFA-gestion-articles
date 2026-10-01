namespace StockManagement.Application.DTOs.Catalogue;

public record FamilleDto(Guid Id, string Nom, string? Description, Guid? FamilleParentId, string? FamilleParentNom, int NombreArticles);
public record CreerFamilleDto(string Nom, string? Description, Guid? FamilleParentId);
public record ModifierFamilleDto(string Nom, string? Description, Guid? FamilleParentId);
