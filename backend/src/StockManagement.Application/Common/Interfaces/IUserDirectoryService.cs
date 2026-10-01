namespace StockManagement.Application.Common.Interfaces;

/// <summary>Abstraction pour interroger les utilisateurs Identity depuis l'Application sans dépendre d'Infrastructure.</summary>
public interface IUserDirectoryService
{
    Task<int> CompterUtilisateursActifsAsync(CancellationToken ct = default);
}
