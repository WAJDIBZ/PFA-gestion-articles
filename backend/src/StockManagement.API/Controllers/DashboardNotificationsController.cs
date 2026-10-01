using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManagement.Application.Interfaces;

namespace StockManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController(IDashboardService dashboardService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Obtenir(CancellationToken ct) => Ok(await dashboardService.ObtenirAsync(ct));
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController(INotificationService notificationService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObtenirNonLues(CancellationToken ct) => Ok(await notificationService.ObtenirNonLuesAsync(ct));

    [HttpPut("{id:guid}/lue")]
    public async Task<IActionResult> MarquerLue(Guid id, CancellationToken ct)
    {
        await notificationService.MarquerLueAsync(id, ct);
        return NoContent();
    }
}
