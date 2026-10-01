using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Application.Common.Models;
using StockManagement.Application.DTOs.Fournisseurs;
using StockManagement.Application.Interfaces;
using StockManagement.Domain.Entities;

namespace StockManagement.Application.Services;

public class FournisseurService(IUnitOfWork uow) : IFournisseurService
{
    public async Task<List<FournisseurDto>> ObtenirTousAsync(CancellationToken ct = default)
    {
        return await uow.Fournisseurs.Query().Where(f => !f.EstSupprime)
            .Select(f => new FournisseurDto(f.Id, f.Nom, f.Contact, f.Email, f.Telephone, f.Adresse, f.EstActif))
            .ToListAsync(ct);
    }

    public async Task<ServiceResult<FournisseurDto>> CreerAsync(CreerFournisseurDto dto, CancellationToken ct = default)
    {
        var fournisseur = new Fournisseur { Nom = dto.Nom, Contact = dto.Contact, Email = dto.Email, Telephone = dto.Telephone, Adresse = dto.Adresse };
        await uow.Fournisseurs.AddAsync(fournisseur, ct);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<FournisseurDto>.Succes(new FournisseurDto(fournisseur.Id, fournisseur.Nom, fournisseur.Contact, fournisseur.Email, fournisseur.Telephone, fournisseur.Adresse, fournisseur.EstActif));
    }

    public async Task<ServiceResult<FournisseurDto>> ModifierAsync(Guid id, ModifierFournisseurDto dto, CancellationToken ct = default)
    {
        var fournisseur = await uow.Fournisseurs.GetByIdAsync(id, ct);
        if (fournisseur is null) return ServiceResult<FournisseurDto>.Echec("Fournisseur introuvable.");

        fournisseur.Nom = dto.Nom;
        fournisseur.Contact = dto.Contact;
        fournisseur.Email = dto.Email;
        fournisseur.Telephone = dto.Telephone;
        fournisseur.Adresse = dto.Adresse;
        fournisseur.EstActif = dto.EstActif;
        fournisseur.DateModification = DateTime.UtcNow;
        uow.Fournisseurs.Update(fournisseur);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<FournisseurDto>.Succes(new FournisseurDto(fournisseur.Id, fournisseur.Nom, fournisseur.Contact, fournisseur.Email, fournisseur.Telephone, fournisseur.Adresse, fournisseur.EstActif));
    }

    public async Task<ServiceResult<bool>> SupprimerAsync(Guid id, CancellationToken ct = default)
    {
        var fournisseur = await uow.Fournisseurs.GetByIdAsync(id, ct);
        if (fournisseur is null) return ServiceResult<bool>.Echec("Fournisseur introuvable.");

        fournisseur.EstSupprime = true;
        uow.Fournisseurs.Update(fournisseur);
        await uow.SaveChangesAsync(ct);
        return ServiceResult<bool>.Succes(true);
    }
}
