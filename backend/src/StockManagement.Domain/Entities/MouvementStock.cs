using StockManagement.Domain.Common;
using StockManagement.Domain.Enums;

namespace StockManagement.Domain.Entities;

/// <summary>Historique d'une opération de stock (entrée, sortie, transfert, ajustement) assurant la traçabilité.</summary>
public class MouvementStock : BaseEntity
{
    public TypeMouvement Type { get; set; }

    public Guid ArticleId { get; set; }
    public Article Article { get; set; } = null!;

    public Guid? ArticleVarianteId { get; set; }
    public ArticleVariante? ArticleVariante { get; set; }

    public Guid? LotId { get; set; }
    public Lot? Lot { get; set; }

    public Guid? NumeroSerieId { get; set; }
    public NumeroSerie? NumeroSerie { get; set; }

    public Guid? DepotSourceId { get; set; }
    public Depot? DepotSource { get; set; }

    public Guid? DepotDestinationId { get; set; }
    public Depot? DepotDestination { get; set; }

    public int Quantite { get; set; }
    public DateTime DateMouvement { get; set; } = DateTime.UtcNow;
    public string? Motif { get; set; }
    public string? Reference { get; set; }

    /// <summary>Permet de relier les deux lignes (sortie/entrée) générées par un même transfert.</summary>
    public Guid? MouvementLieId { get; set; }

    public Guid UtilisateurId { get; set; }
    public string UtilisateurNom { get; set; } = string.Empty;
}
