using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Application.DTOs.Stock;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Enums;

namespace StockManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StockController(IStockService stockService) : ControllerBase
{
    [HttpGet("depot/{depotId:guid}")]
    public async Task<IActionResult> ObtenirParDepot(Guid depotId, CancellationToken ct) => Ok(await stockService.ObtenirParDepotAsync(depotId, ct));

    [HttpGet("article/{articleId:guid}")]
    public async Task<IActionResult> ObtenirParArticle(Guid articleId, CancellationToken ct)
    {
        var resultat = await stockService.ObtenirParArticleAsync(articleId, ct);
        return resultat is null ? NotFound() : Ok(resultat);
    }

    [HttpGet("alertes")]
    public async Task<IActionResult> ObtenirAlertes(CancellationToken ct) => Ok(await stockService.ObtenirAlertesAsync(ct));
}

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{RolesApplicatifs.Administrateur},{RolesApplicatifs.Magasinier}")]
public class MouvementsController(IMouvementService mouvementService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Rechercher([FromQuery] FiltreMouvementDto filtre, CancellationToken ct) => Ok(await mouvementService.RechercherAsync(filtre, ct));

    [HttpPost("entree")]
    public async Task<IActionResult> Entree(CreerEntreeStockDto dto, CancellationToken ct)
    {
        var resultat = await mouvementService.EnregistrerEntreeAsync(dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpPost("sortie")]
    public async Task<IActionResult> Sortie(CreerSortieStockDto dto, CancellationToken ct)
    {
        var resultat = await mouvementService.EnregistrerSortieAsync(dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }

    [HttpPost("transfert")]
    public async Task<IActionResult> Transfert(CreerTransfertStockDto dto, CancellationToken ct)
    {
        var resultat = await mouvementService.EnregistrerTransfertAsync(dto, ct);
        return resultat.EstReussi ? Ok(resultat.Donnees) : BadRequest(new { message = resultat.MessageErreur });
    }
}
