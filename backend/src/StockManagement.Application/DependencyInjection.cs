using Microsoft.Extensions.DependencyInjection;
using StockManagement.Application.Interfaces;
using StockManagement.Application.Services;

namespace StockManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AjouterApplication(this IServiceCollection services)
    {
        services.AddScoped<IFamilleService, FamilleService>();
        services.AddScoped<IMarqueService, MarqueService>();
        services.AddScoped<IUniteService, UniteService>();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IVarianteService, VarianteService>();
        services.AddScoped<IDepotService, DepotService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<IMouvementService, MouvementService>();
        services.AddScoped<IFournisseurService, FournisseurService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<INotificationService, NotificationService>();
        return services;
    }
}
