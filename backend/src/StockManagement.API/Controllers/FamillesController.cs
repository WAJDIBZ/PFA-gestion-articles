using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Application.DTOs.Catalogue;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Enums;

namespace StockManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FamillesController(IFamilleService familleService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObtenirToutes(CancellationToken ct) => Ok(await familleService.ObtenirToutesAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenirParId(Guid id, CancellationToken ct)
    {
        var famille = await familleService.ObtenirParIdAsync(id, ct);
        return famille is null ? NotFound() : Ok(famille);
    }

    [HttpPost]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Creer(CreerFamilleDto dto, CancellationToken ct)
    {
        var resultat = await familleService.CreerAsync(dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Modifier(Guid id, ModifierFamilleDto dto, CancellationToken ct)
    {
        var resultat = await familleService.ModifierAsync(id, dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Supprimer(Guid id, CancellationToken ct)
    {
        var resultat = await familleService.SupprimerAsync(id, ct);
        return resultat.EstReussi ? NoContent() : BadRequest(new { message = resultat.MessageErreur });
    }
}
