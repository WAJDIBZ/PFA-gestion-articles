using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Application.Common.Models;
using StockManagement.Application.DTOs.Depots;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Entities;

namespace StockManagement.Application.Services;

public class DepotService(IUnitOfWork uow) : IDepotService
{
    public async Task<List<DepotDto>> ObtenirTousAsync(CancellationToken ct = default)
    {
        return await uow.Depots.Query().Where(d => !d.EstSupprime)
            .Select(d => new DepotDto(
                d.Id, d.Nom, d.Adresse, d.Responsable, d.EstActif,
                d.StockItems.Select(s => s.ArticleId).Distinct().Count(),
                d.StockItems.Sum(s => s.Quantite)))
            .ToListAsync(ct);
    }

    public async Task<DepotDto?> ObtenirParIdAsync(Guid id, CancellationToken ct = default)
    {
        return await uow.Depots.Query().Where(d => d.Id == id && !d.EstSupprime)
            .Select(d => new DepotDto(
                d.Id, d.Nom, d.Adresse, d.Responsable, d.EstActif,
                d.StockItems.Select(s => s.ArticleId).Distinct().Count(),
                d.StockItems.Sum(s => s.Quantite)))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<ServiceResult<DepotDto>> CreerAsync(CreerDepotDto dto, CancellationToken ct = default)
    {
        var depot = new Depot { Nom = dto.Nom, Adresse = dto.Adresse, Responsable = dto.Responsable };
        await uow.Depots.AddAsync(depot, ct);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<DepotDto>.Succes(new DepotDto(depot.Id, depot.Nom, depot.Adresse, depot.Responsable, depot.EstActif, 0, 0));
    }

    public async Task<ServiceResult<DepotDto>> ModifierAsync(Guid id, ModifierDepotDto dto, CancellationToken ct = default)
    {
        var depot = await uow.Depots.GetByIdAsync(id, ct);
        if (depot is null) return ServiceResult<DepotDto>.Echec("Dépôt introuvable.");

        depot.Nom = dto.Nom;
        depot.Adresse = dto.Adresse;
        depot.Responsable = dto.Responsable;
        depot.EstActif = dto.EstActif;
        depot.DateModification = DateTime.UtcNow;
        uow.Depots.Update(depot);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<DepotDto>.Succes(new DepotDto(depot.Id, depot.Nom, depot.Adresse, depot.Responsable, depot.EstActif, 0, 0));
    }

    public async Task<ServiceResult<bool>> SupprimerAsync(Guid id, CancellationToken ct = default)
    {
        var depot = await uow.Depots.GetByIdAsync(id, ct);
        if (depot is null) return ServiceResult<bool>.Echec("Dépôt introuvable.");

        depot.EstSupprime = true;
        uow.Depots.Update(depot);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<bool>.Succes(true);
    }
}
