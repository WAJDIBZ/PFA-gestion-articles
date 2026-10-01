using StockManagement.Domain.Enums;

namespace StockManagement.Application.DTOs.Catalogue;

public record ArticleDto(
    Guid Id,
    string Reference,
    string Designation,
    string? Description,
    string? CodeBarre,
    Guid FamilleId,
    string FamilleNom,
    Guid? MarqueId,
    string? MarqueNom,
    Guid UniteId,
    string UniteNom,
    ModeSuivi ModeSuivi,
    bool GereVariantes,
    bool SuiviDatePeremption,
    decimal PrixAchat,
    decimal PrixVente,
    int SeuilMinimum,
    bool EstActif,
    int QuantiteTotale);

public record CreerArticleDto(
    string Reference,
    string Designation,
    string? Description,
    string? CodeBarre,
    Guid FamilleId,
    Guid? MarqueId,
    Guid UniteId,
    ModeSuivi ModeSuivi,
    bool GereVariantes,
    bool SuiviDatePeremption,
    decimal PrixAchat,
    decimal PrixVente,
    int SeuilMinimum,
    List<Guid>? AttributsVarianteIds);

public record ModifierArticleDto(
    string Designation,
    string? Description,
    string? CodeBarre,
    Guid FamilleId,
    Guid? MarqueId,
    Guid UniteId,
    ModeSuivi ModeSuivi,
    bool SuiviDatePeremption,
    decimal PrixAchat,
    decimal PrixVente,
    int SeuilMinimum,
    bool EstActif);
