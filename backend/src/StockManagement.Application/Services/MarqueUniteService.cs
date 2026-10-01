using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Application.Common.Models;
using StockManagement.Application.DTOs.Catalogue;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Entities;

namespace StockManagement.Application.Services;

public class MarqueService(IUnitOfWork uow) : IMarqueService
{
    public async Task<List<MarqueDto>> ObtenirToutesAsync(CancellationToken ct = default)
    {
        return await uow.Marques.Query().Where(m => !m.EstSupprime)
            .Select(m => new MarqueDto(m.Id, m.Nom)).ToListAsync(ct);
    }

    public async Task<ServiceResult<MarqueDto>> CreerAsync(CreerMarqueDto dto, CancellationToken ct = default)
    {
        var marque = new Marque { Nom = dto.Nom };
        await uow.Marques.AddAsync(marque, ct);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<MarqueDto>.Succes(new MarqueDto(marque.Id, marque.Nom));
    }

    public async Task<ServiceResult<MarqueDto>> ModifierAsync(Guid id, ModifierMarqueDto dto, CancellationToken ct = default)
    {
        var marque = await uow.Marques.GetByIdAsync(id, ct);
        if (marque is null) return ServiceResult<MarqueDto>.Echec("Marque introuvable.");

        marque.Nom = dto.Nom;
        marque.DateModification = DateTime.UtcNow;
        uow.Marques.Update(marque);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<MarqueDto>.Succes(new MarqueDto(marque.Id, marque.Nom));
    }

    public async Task<ServiceResult<bool>> SupprimerAsync(Guid id, CancellationToken ct = default)
    {
        var marque = await uow.Marques.GetByIdAsync(id, ct);
        if (marque is null) return ServiceResult<bool>.Echec("Marque introuvable.");

        marque.EstSupprime = true;
        uow.Marques.Update(marque);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<bool>.Succes(true);
    }
}

public class UniteService(IUnitOfWork uow) : IUniteService
{
    public async Task<List<UniteDto>> ObtenirToutesAsync(CancellationToken ct = default)
    {
        return await uow.Unites.Query().Where(u => !u.EstSupprime)
            .Select(u => new UniteDto(u.Id, u.Nom, u.Symbole)).ToListAsync(ct);
    }

    public async Task<ServiceResult<UniteDto>> CreerAsync(CreerUniteDto dto, CancellationToken ct = default)
    {
        var unite = new Unite { Nom = dto.Nom, Symbole = dto.Symbole };
        await uow.Unites.AddAsync(unite, ct);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<UniteDto>.Succes(new UniteDto(unite.Id, unite.Nom, unite.Symbole));
    }

    public async Task<ServiceResult<UniteDto>> ModifierAsync(Guid id, ModifierUniteDto dto, CancellationToken ct = default)
    {
        var unite = await uow.Unites.GetByIdAsync(id, ct);
        if (unite is null) return ServiceResult<UniteDto>.Echec("Unité introuvable.");

        unite.Nom = dto.Nom;
        unite.Symbole = dto.Symbole;
        unite.DateModification = DateTime.UtcNow;
        uow.Unites.Update(unite);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<UniteDto>.Succes(new UniteDto(unite.Id, unite.Nom, unite.Symbole));
    }

    public async Task<ServiceResult<bool>> SupprimerAsync(Guid id, CancellationToken ct = default)
    {
        var unite = await uow.Unites.GetByIdAsync(id, ct);
        if (unite is null) return ServiceResult<bool>.Echec("Unité introuvable.");

        unite.EstSupprime = true;
        uow.Unites.Update(unite);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<bool>.Succes(true);
    }
}
