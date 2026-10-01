namespace StockManagement.Application.DTOs.Catalogue;

public record MarqueDto(Guid Id, string Nom);
public record CreerMarqueDto(string Nom);
public record ModifierMarqueDto(string Nom);

public record UniteDto(Guid Id, string Nom, string Symbole);
public record CreerUniteDto(string Nom, string Symbole);
public record ModifierUniteDto(string Nom, string Symbole);
