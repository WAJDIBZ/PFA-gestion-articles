using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Application.DTOs.Depots;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Enums;

namespace StockManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepotsController(IDepotService depotService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObtenirTous(CancellationToken ct) => Ok(await depotService.ObtenirTousAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenirParId(Guid id, CancellationToken ct)
    {
        var depot = await depotService.ObtenirParIdAsync(id, ct);
        return depot is null ? NotFound() : Ok(depot);
    }

    [HttpPost]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Creer(CreerDepotDto dto, CancellationToken ct)
    {
        var resultat = await depotService.CreerAsync(dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Modifier(Guid id, ModifierDepotDto dto, CancellationToken ct)
    {
        var resultat = await depotService.ModifierAsync(id, dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Supprimer(Guid id, CancellationToken ct)
    {
        var resultat = await depotService.SupprimerAsync(id, ct);
        return resultat.EstReussi ? NoContent() : BadRequest(new { message = resultat.MessageErreur });
    }
}
