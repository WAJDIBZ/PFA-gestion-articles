using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Application.Common.Models;
using StockManagement.Application.DTOs.Catalogue;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Entities;

namespace StockManagement.Application.Services;

public class FamilleService(IUnitOfWork uow) : IFamilleService
{
    public async Task<List<FamilleDto>> ObtenirToutesAsync(CancellationToken ct = default)
    {
        return await uow.Familles.Query()
            .Where(f => !f.EstSupprime)
            .Select(f => new FamilleDto(f.Id, f.Nom, f.Description, f.FamilleParentId, f.FamilleParent != null ? f.FamilleParent.Nom : null, f.Articles.Count))
            .ToListAsync(ct);
    }

    public async Task<FamilleDto?> ObtenirParIdAsync(Guid id, CancellationToken ct = default)
    {
        return await uow.Familles.Query()
            .Where(f => f.Id == id && !f.EstSupprime)
            .Select(f => new FamilleDto(f.Id, f.Nom, f.Description, f.FamilleParentId, f.FamilleParent != null ? f.FamilleParent.Nom : null, f.Articles.Count))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<ServiceResult<FamilleDto>> CreerAsync(CreerFamilleDto dto, CancellationToken ct = default)
    {
        var famille = new Famille { Nom = dto.Nom, Description = dto.Description, FamilleParentId = dto.FamilleParentId };
        await uow.Familles.AddAsync(famille, ct);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<FamilleDto>.Succes(new FamilleDto(famille.Id, famille.Nom, famille.Description, famille.FamilleParentId, null, 0));
    }

    public async Task<ServiceResult<FamilleDto>> ModifierAsync(Guid id, ModifierFamilleDto dto, CancellationToken ct = default)
    {
        var famille = await uow.Familles.GetByIdAsync(id, ct);
        if (famille is null) return ServiceResult<FamilleDto>.Echec("Famille introuvable.");

        famille.Nom = dto.Nom;
        famille.Description = dto.Description;
        famille.FamilleParentId = dto.FamilleParentId;
        famille.DateModification = DateTime.UtcNow;
        uow.Familles.Update(famille);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<FamilleDto>.Succes(new FamilleDto(famille.Id, famille.Nom, famille.Description, famille.FamilleParentId, null, 0));
    }

    public async Task<ServiceResult<bool>> SupprimerAsync(Guid id, CancellationToken ct = default)
    {
        var famille = await uow.Familles.GetByIdAsync(id, ct);
        if (famille is null) return ServiceResult<bool>.Echec("Famille introuvable.");

        famille.EstSupprime = true;
        uow.Familles.Update(famille);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<bool>.Succes(true);
    }
}
