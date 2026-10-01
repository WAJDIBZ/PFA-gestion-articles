namespace StockManagement.Application.DTOs.Stock;

public record StockItemDto(
    Guid Id,
    Guid ArticleId,
    string ArticleDesignation,
    string ArticleReference,
    Guid? ArticleVarianteId,
    string? ArticleVarianteReference,
    Guid DepotId,
    string DepotNom,
    Guid? LotId,
    string? NumeroLot,
    int Quantite,
    int SeuilMinimum,
    bool EstEnAlerte);

public record StockParArticleDto(Guid ArticleId, string ArticleReference, string ArticleDesignation, int QuantiteTotale, int SeuilMinimum, List<StockItemDto> Repartition);
