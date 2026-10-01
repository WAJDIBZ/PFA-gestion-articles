namespace StockManagement.Application.Common.Models;

/// <summary>Enveloppe de résultat pour les opérations de service, évite les exceptions pour les erreurs métier.</summary>
public class ServiceResult<T>
{
    public bool EstReussi { get; private set; }
    public T? Donnees { get; private set; }
    public string? MessageErreur { get; private set; }

    public static ServiceResult<T> Succes(T donnees) => new() { EstReussi = true, Donnees = donnees };
    public static ServiceResult<T> Echec(string message) => new() { EstReussi = false, MessageErreur = message };
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
