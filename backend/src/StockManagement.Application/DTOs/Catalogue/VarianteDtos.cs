namespace StockManagement.Application.DTOs.Catalogue;

public record AttributVarianteDto(Guid Id, string Nom, List<ValeurAttributDto> Valeurs);
public record CreerAttributVarianteDto(string Nom);
public record ValeurAttributDto(Guid Id, string Valeur);
public record CreerValeurAttributDto(Guid AttributVarianteId, string Valeur);

public record ArticleVarianteDto(
    Guid Id,
    Guid ArticleId,
    string ReferenceVariante,
    string? CodeBarre,
    bool EstActif,
    int QuantiteTotale,
    List<ValeurAttributDto> Valeurs);

public record CreerArticleVarianteDto(Guid ArticleId, string ReferenceVariante, string? CodeBarre, List<Guid> ValeurAttributIds);

public record RechercheCombinaisonDto(Guid ArticleId, List<Guid> ValeurAttributIds);
public record ResultatCombinaisonDto(bool Disponible, Guid? ArticleVarianteId, string? ReferenceVariante, int QuantiteTotale);
