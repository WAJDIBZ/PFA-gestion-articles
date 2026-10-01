using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Application.Common.Models;
using StockManagement.Application.DTOs.Auth;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Entities;

namespace StockManagement.Infrastructure.Identity;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IJwtTokenGenerator tokenGenerator) : IAuthService
{
    public async Task<ServiceResult<AuthResponseDto>> ConnecterAsync(LoginDto dto, CancellationToken ct = default)
    {
        var utilisateur = await userManager.FindByEmailAsync(dto.Email);
        if (utilisateur is null || !utilisateur.EstActif)
            return ServiceResult<AuthResponseDto>.Echec("Identifiants invalides.");

        var motDePasseValide = await userManager.CheckPasswordAsync(utilisateur, dto.MotDePasse);
        if (!motDePasseValide)
            return ServiceResult<AuthResponseDto>.Echec("Identifiants invalides.");

        var roles = await userManager.GetRolesAsync(utilisateur);
        var token = tokenGenerator.GenererToken(utilisateur, roles);

        var reponse = new AuthResponseDto(
            token,
            DateTime.UtcNow.AddMinutes(480),
            new UtilisateurDto(utilisateur.Id, utilisateur.NomComplet, utilisateur.Email!, roles.ToList(), utilisateur.EstActif));

        return ServiceResult<AuthResponseDto>.Succes(reponse);
    }

    public async Task<ServiceResult<UtilisateurDto>> InscrireAsync(RegisterDto dto, CancellationToken ct = default)
    {
        if (!await roleManager.RoleExistsAsync(dto.Role))
            return ServiceResult<UtilisateurDto>.Echec("Rôle invalide.");

        var utilisateur = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            NomComplet = dto.NomComplet
        };

        var resultat = await userManager.CreateAsync(utilisateur, dto.MotDePasse);
        if (!resultat.Succeeded)
            return ServiceResult<UtilisateurDto>.Echec(string.Join(" ", resultat.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(utilisateur, dto.Role);

        return ServiceResult<UtilisateurDto>.Succes(new UtilisateurDto(utilisateur.Id, utilisateur.NomComplet, utilisateur.Email!, [dto.Role], utilisateur.EstActif));
    }

    public async Task<List<UtilisateurDto>> ObtenirTousAsync(CancellationToken ct = default)
    {
        var utilisateurs = await userManager.Users.ToListAsync(ct);
        var resultat = new List<UtilisateurDto>();
        foreach (var u in utilisateurs)
        {
            var roles = await userManager.GetRolesAsync(u);
            resultat.Add(new UtilisateurDto(u.Id, u.NomComplet, u.Email!, roles.ToList(), u.EstActif));
        }
        return resultat;
    }

    public async Task<ServiceResult<UtilisateurDto>> ModifierUtilisateurAsync(Guid id, ModifierUtilisateurDto dto, CancellationToken ct = default)
    {
        var utilisateur = await userManager.FindByIdAsync(id.ToString());
        if (utilisateur is null) return ServiceResult<UtilisateurDto>.Echec("Utilisateur introuvable.");

        utilisateur.NomComplet = dto.NomComplet;
        utilisateur.EstActif = dto.EstActif;
        await userManager.UpdateAsync(utilisateur);

        var rolesActuels = await userManager.GetRolesAsync(utilisateur);
        if (!rolesActuels.Contains(dto.Role))
        {
            await userManager.RemoveFromRolesAsync(utilisateur, rolesActuels);
            await userManager.AddToRoleAsync(utilisateur, dto.Role);
        }

        return ServiceResult<UtilisateurDto>.Succes(new UtilisateurDto(utilisateur.Id, utilisateur.NomComplet, utilisateur.Email!, [dto.Role], utilisateur.EstActif));
    }

    public async Task<ServiceResult<bool>> SupprimerUtilisateurAsync(Guid id, CancellationToken ct = default)
    {
        var utilisateur = await userManager.FindByIdAsync(id.ToString());
        if (utilisateur is null) return ServiceResult<bool>.Echec("Utilisateur introuvable.");

        utilisateur.EstActif = false;
        await userManager.UpdateAsync(utilisateur);
        return ServiceResult<bool>.Succes(true);
    }
}
