namespace StockManagement.Application.DTOs.Auth;

public record LoginDto(string Email, string MotDePasse);
public record RegisterDto(string NomComplet, string Email, string MotDePasse, string Role);
public record AuthResponseDto(string Token, DateTime ExpirationToken, UtilisateurDto Utilisateur);
public record UtilisateurDto(Guid Id, string NomComplet, string Email, List<string> Roles, bool EstActif);
public record ModifierUtilisateurDto(string NomComplet, string Role, bool EstActif);
public record ChangerMotDePasseDto(string AncienMotDePasse, string NouveauMotDePasse);
