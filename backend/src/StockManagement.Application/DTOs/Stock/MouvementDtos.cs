using StockManagement.Domain.Enums;

namespace StockManagement.Application.DTOs.Stock;

public record MouvementStockDto(
    Guid Id,
    TypeMouvement Type,
    Guid ArticleId,
    string ArticleDesignation,
    Guid? ArticleVarianteId,
    string? ArticleVarianteReference,
    Guid? DepotSourceId,
    string? DepotSourceNom,
    Guid? DepotDestinationId,
    string? DepotDestinationNom,
    int Quantite,
    DateTime DateMouvement,
    string? Motif,
    string? Reference,
    string UtilisateurNom);

public record CreerEntreeStockDto(Guid ArticleId, Guid? ArticleVarianteId, Guid DepotDestinationId, int Quantite, string? NumeroLot, DateTime? DateExpirationLot, string? NumeroSerie, string? Motif, string? Reference);

public record CreerSortieStockDto(Guid ArticleId, Guid? ArticleVarianteId, Guid DepotSourceId, int Quantite, Guid? LotId, string? NumeroSerie, string? Motif, string? Reference);

public record CreerTransfertStockDto(Guid ArticleId, Guid? ArticleVarianteId, Guid DepotSourceId, Guid DepotDestinationId, int Quantite, Guid? LotId, string? Motif, string? Reference);

public record FiltreMouvementDto(Guid? ArticleId, Guid? DepotId, TypeMouvement? Type, DateTime? DateDebut, DateTime? DateFin, int Page = 1, int PageSize = 20);
