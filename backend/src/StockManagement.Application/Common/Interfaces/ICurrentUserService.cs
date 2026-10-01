namespace StockManagement.Application.Common.Interfaces;

/// <summary>Donne accès à l'utilisateur authentifié courant (issu du token JWT) pour la traçabilité.</summary>
public interface ICurrentUserService
{
    Guid? UtilisateurId { get; }
    string? NomComplet { get; }
    bool EstDansRole(string role);
}
