using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Application.DTOs.Catalogue;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Enums;

namespace StockManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VariantesController(IVarianteService varianteService) : ControllerBase
{
    [HttpGet("attributs")]
    public async Task<IActionResult> ObtenirAttributs(CancellationToken ct) => Ok(await varianteService.ObtenirAttributsAsync(ct));

    [HttpPost("attributs")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> CreerAttribut(CreerAttributVarianteDto dto, CancellationToken ct)
    {
        var resultat = await varianteService.CreerAttributAsync(dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpPost("attributs/valeurs")]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> CreerValeur(CreerValeurAttributDto dto, CancellationToken ct)
    {
        var resultat = await varianteService.CreerValeurAsync(dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpGet("par-article/{articleId:guid}")]
    public async Task<IActionResult> ObtenirParArticle(Guid articleId, CancellationToken ct)
        => Ok(await varianteService.ObtenirParArticleAsync(articleId, ct));

    [HttpPost]
    [Authorize(Roles = RolesApplicatifs.Administrateur)]
    public async Task<IActionResult> Creer(CreerArticleVarianteDto dto, CancellationToken ct)
    {
        var resultat = await varianteService.CreerVarianteAsync(dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpPost("rechercher-combinaison")]
    public async Task<IActionResult> RechercherCombinaison(RechercheCombinaisonDto dto, CancellationToken ct)
        => Ok(await varianteService.RechercherCombinaisonAsync(dto, ct));
}
