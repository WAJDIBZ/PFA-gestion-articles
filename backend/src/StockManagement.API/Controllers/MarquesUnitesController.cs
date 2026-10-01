using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Application.DTOs.Catalogue;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Enums;

namespace StockManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MarquesController(IMarqueService marqueService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObtenirToutes(CancellationToken ct) => Ok(await marqueService.ObtenirToutesAsync(ct));

    [HttpPost]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Creer(CreerMarqueDto dto, CancellationToken ct)
    {
        var resultat = await marqueService.CreerAsync(dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Modifier(Guid id, ModifierMarqueDto dto, CancellationToken ct)
    {
        var resultat = await marqueService.ModifierAsync(id, dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Supprimer(Guid id, CancellationToken ct)
    {
        var resultat = await marqueService.SupprimerAsync(id, ct);
        return resultat.EstReussi ? NoContent() : BadRequest(new { message = resultat.MessageErreur });
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UnitesController(IUniteService uniteService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObtenirToutes(CancellationToken ct) => Ok(await uniteService.ObtenirToutesAsync(ct));

    [HttpPost]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Creer(CreerUniteDto dto, CancellationToken ct)
    {
        var resultat = await uniteService.CreerAsync(dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Modifier(Guid id, ModifierUniteDto dto, CancellationToken ct)
    {
        var resultat = await uniteService.ModifierAsync(id, dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Supprimer(Guid id, CancellationToken ct)
    {
        var resultat = await uniteService.SupprimerAsync(id, ct);
        return resultat.EstReussi ? NoContent() : BadRequest(new { message = resultat.MessageErreur });
    }
}
