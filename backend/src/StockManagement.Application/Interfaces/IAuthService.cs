using StockManagement.Application.Common.Models;
using StockManagement.Application.DTOs.Auth;

namespace StockManagement.Application.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<AuthResponseDto>> ConnecterAsync(LoginDto dto, CancellationToken ct = default);
    Task<ServiceResult<UtilisateurDto>> InscrireAsync(RegisterDto dto, CancellationToken ct = default);
    Task<List<UtilisateurDto>> ObtenirTousAsync(CancellationToken ct = default);
    Task<ServiceResult<UtilisateurDto>> ModifierUtilisateurAsync(Guid id, ModifierUtilisateurDto dto, CancellationToken ct = default);
    Task<ServiceResult<bool>> SupprimerUtilisateurAsync(Guid id, CancellationToken ct = default);
}
