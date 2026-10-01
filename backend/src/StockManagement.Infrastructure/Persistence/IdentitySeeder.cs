using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using StockManagement.Domain.Entities;
using StockManagement.Domain.Enums;

namespace StockManagement.Infrastructure.Persistence;

/// <summary>Crée les rôles applicatifs et un compte de démonstration par rôle au premier démarrage.</summary>
public static class IdentitySeeder
{
    private static readonly (string Email, string NomComplet, string Role)[] ComptesDemo =
    [
        ("admin@stockmanagement.local", "Administrateur", RolesApplicatifs.Administrateur),
        ("magasinier@stockmanagement.local", "Magasinier", RolesApplicatifs.Magasinier),
        ("consultant@stockmanagement.local", "Consultant", RolesApplicatifs.Consultant),
    ];

    private const string MotDePasseDemo = "Demo@123";

    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in RolesApplicatifs.Toutes)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new ApplicationRole(role));
        }

        foreach (var (email, nomComplet, role) in ComptesDemo)
        {
            if (await userManager.FindByEmailAsync(email) is not null) continue;

            var utilisateur = new ApplicationUser
            {
                UserName = email,
                Email = email,
                NomComplet = nomComplet,
                EmailConfirmed = true
            };
            var resultat = await userManager.CreateAsync(utilisateur, MotDePasseDemo);
            if (resultat.Succeeded)
                await userManager.AddToRoleAsync(utilisateur, role);
        }
    }
}
