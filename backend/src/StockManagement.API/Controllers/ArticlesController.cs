using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Application.DTOs.Catalogue;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Enums;

namespace StockManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ArticlesController(IArticleService articleService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Rechercher([FromQuery] string? texte, [FromQuery] Guid? familleId, [FromQuery] bool? actifSeulement, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => Ok(await articleService.RechercherAsync(texte, familleId, actifSeulement, page, pageSize, ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenirParId(Guid id, CancellationToken ct)
    {
        var article = await articleService.ObtenirParIdAsync(id, ct);
        return article is null ? NotFound() : Ok(article);
    }

    [HttpPost]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Creer(CreerArticleDto dto, CancellationToken ct)
    {
        var resultat = await articleService.CreerAsync(dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Modifier(Guid id, ModifierArticleDto dto, CancellationToken ct)
    {
        var resultat = await articleService.ModifierAsync(id, dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Supprimer(Guid id, CancellationToken ct)
    {
        var resultat = await articleService.SupprimerAsync(id, ct);
        return resultat.EstReussi ? NoContent() : BadRequest(new { message = resultat.MessageErreur });
    }
}
