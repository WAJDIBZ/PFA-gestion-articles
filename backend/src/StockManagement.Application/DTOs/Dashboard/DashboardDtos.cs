using StockManagement.Application.DTOs.Stock;

namespace StockManagement.Application.DTOs.Dashboard;

public record DashboardDto(
    int NombreArticles,
    int QuantiteTotaleStock,
    int NombreArticlesEnAlerte,
    int NombreArticlesEpuises,
    int NombreDepots,
    int NombreUtilisateurs,
    List<MouvementStockDto> DerniersMouvements,
    List<ArticleAlerteDto> ArticlesEnAlerte);

public record ArticleAlerteDto(Guid ArticleId, string Reference, string Designation, int QuantiteActuelle, int SeuilMinimum);
