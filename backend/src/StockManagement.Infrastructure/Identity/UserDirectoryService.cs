using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Domain.Entities;

namespace StockManagement.Infrastructure.Identity;

public class UserDirectoryService(UserManager<ApplicationUser> userManager) : IUserDirectoryService
{
    public Task<int> CompterUtilisateursActifsAsync(CancellationToken ct = default)
        => userManager.Users.Where(u => u.EstActif).CountAsync(ct);
}
