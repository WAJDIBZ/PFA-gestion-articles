using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Application.DTOs.Fournisseurs;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Enums;

namespace StockManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FournisseursController(IFournisseurService fournisseurService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObtenirTous(CancellationToken ct) => Ok(await fournisseurService.ObtenirTousAsync(ct));

    [HttpPost]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Creer(CreerFournisseurDto dto, CancellationToken ct)
    {
        var resultat = await fournisseurService.CreerAsync(dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Modifier(Guid id, ModifierFournisseurDto dto, CancellationToken ct)
    {
        var resultat = await fournisseurService.ModifierAsync(id, dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Supprimer(Guid id, CancellationToken ct)
    {
        var resultat = await fournisseurService.SupprimerAsync(id, ct);
        return resultat.EstReussi ? NoContent() : BadRequest(new { message = resultat.MessageErreur });
    }
}
