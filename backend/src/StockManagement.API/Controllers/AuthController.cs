using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Application.DTOs.Auth;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Enums;

namespace StockManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Connecter(LoginDto dto, CancellationToken ct)
    {
        var resultat = await authService.ConnecterAsync(dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : Unauthorized(new { message = resultat.MessageErreur });
    }

    [HttpPost("register")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Inscrire(RegisterDto dto, CancellationToken ct)
    {
        var resultat = await authService.InscrireAsync(dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpGet("utilisateurs")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> ObtenirUtilisateurs(CancellationToken ct)
        => Ok(await authService.ObtenirTousAsync(ct));

    [HttpPut("utilisateurs/{id:guid}")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> ModifierUtilisateur(Guid id, ModifierUtilisateurDto dto, CancellationToken ct)
    {
        var resultat = await authService.ModifierUtilisateurAsync(id, dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpDelete("utilisateurs/{id:guid}")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> SupprimerUtilisateur(Guid id, CancellationToken ct)
    {
        var resultat = await authService.SupprimerUtilisateurAsync(id, ct);
        return resultat.EstReussi ? NoContent() : BadRequest(new { message = resultat.MessageErreur });
    }
}
