using Microsoft.EntityFrameworkCore;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Infrastructure.Persistence;

namespace StockManagement.Infrastructure.Repositories;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class
{
    protected readonly DbSet<T> Set = context.Set<T>();

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) => await Set.FindAsync([id], ct);

    public async Task<List<T>> GetAllAsync(CancellationToken ct = default) => await Set.ToListAsync(ct);

    public IQueryable<T> Query() => Set.AsQueryable();

    public async Task AddAsync(T entity, CancellationToken ct = default) => await Set.AddAsync(entity, ct);

    /// <summary>Marque l'entité comme modifiée, sauf si elle est déjà suivie comme nouvellement ajoutée
    /// (évite de transformer un INSERT en UPDATE, ce qui provoquerait une DbUpdateConcurrencyException).</summary>
    public void Update(T entity)
    {
        if (context.Entry(entity).State == EntityState.Added) return;
        Set.Update(entity);
    }

    public void Remove(T entity) => Set.Remove(entity);
}
