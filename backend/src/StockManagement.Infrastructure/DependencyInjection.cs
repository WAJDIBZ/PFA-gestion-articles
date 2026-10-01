using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Application.Interfaces;
using StockManagement.Infrastructure.Identity;
using StockManagement.Infrastructure.Persistence;
using StockManagement.Infrastructure.Repositories;

namespace StockManagement.Infrastructure;

public static class DependencyInjection
{
    /// <summary>Enregistre le DbContext, les dépôts et les services d'infrastructure.
    /// L'enregistrement d'Identity (AddIdentity) se fait dans le projet API qui référence le framework web complet.</summary>
    public static IServiceCollection AjouterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserDirectoryService, UserDirectoryService>();

        return services;
    }
}
