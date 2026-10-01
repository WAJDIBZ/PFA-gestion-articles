using System.Security.Claims;
using StockManagement.Application.Common.Interfaces;

namespace StockManagement.API.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? UtilisateurId
    {
        get
        {
            var id = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var guid) ? guid : null;
        }
    }

    public string? NomComplet => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

    public bool EstDansRole(string role) => httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
}
