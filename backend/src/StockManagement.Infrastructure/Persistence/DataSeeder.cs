using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StockManagement.Application.Common.Interfaces;
using StockManagement.Domain.Entities;
using StockManagement.Domain.Enums;

namespace StockManagement.Infrastructure.Persistence;

/// <summary>Alimente la base avec un catalogue de démonstration (familles, marques, dépôts, articles, stock, mouvements).</summary>
public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var uow = services.GetRequiredService<IUnitOfWork>();

        if (await uow.Familles.Query().AnyAsync()) return; // déjà alimenté

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var admin = await userManager.FindByEmailAsync("admin@stockmanagement.local");
        var adminId = admin?.Id ?? Guid.Empty;
        var adminNom = admin?.NomComplet ?? "Administrateur";

        // ---------- Familles ----------
        var familleVetements = new Famille { Nom = "Vêtements", Description = "Articles vestimentaires" };
        var familleElectronique = new Famille { Nom = "Électronique", Description = "Appareils et accessoires électroniques" };
        var familleAlimentation = new Famille { Nom = "Alimentation", Description = "Produits alimentaires" };
        var familleMobilier = new Famille { Nom = "Mobilier & Bureau", Description = "Mobilier et équipements de bureau" };
        var familleAccessoires = new Famille { Nom = "Accessoires", Description = "Accessoires divers" };
        foreach (var f in new[] { familleVetements, familleElectronique, familleAlimentation, familleMobilier, familleAccessoires })
            await uow.Familles.AddAsync(f);

        // ---------- Marques ----------
        var marqueNike = new Marque { Nom = "Nike" };
        var marqueSamsung = new Marque { Nom = "Samsung" };
        var marqueLogitech = new Marque { Nom = "Logitech" };
        var marqueIkea = new Marque { Nom = "IKEA" };
        var marqueGenerique = new Marque { Nom = "Générique" };
        foreach (var m in new[] { marqueNike, marqueSamsung, marqueLogitech, marqueIkea, marqueGenerique })
            await uow.Marques.AddAsync(m);

        // ---------- Unités ----------
        var unitePiece = new Unite { Nom = "Pièce", Symbole = "pcs" };
        var uniteKg = new Unite { Nom = "Kilogramme", Symbole = "kg" };
        var uniteLitre = new Unite { Nom = "Litre", Symbole = "L" };
        var uniteBoite = new Unite { Nom = "Boîte", Symbole = "bte" };
        foreach (var u in new[] { unitePiece, uniteKg, uniteLitre, uniteBoite })
            await uow.Unites.AddAsync(u);

        // ---------- Dépôts ----------
        var depotTunis = new Depot { Nom = "Dépôt Tunis", Adresse = "Zone Industrielle, Tunis", Responsable = "Ahmed Ben Ali" };
        var depotSousse = new Depot { Nom = "Dépôt Sousse", Adresse = "Route de Msaken, Sousse", Responsable = "Fatma Trabelsi" };
        var depotSfax = new Depot { Nom = "Dépôt Sfax", Adresse = "Route de Gabès, Sfax", Responsable = "Karim Gharbi" };
        foreach (var d in new[] { depotTunis, depotSousse, depotSfax })
            await uow.Depots.AddAsync(d);

        // ---------- Fournisseurs ----------
        var fournisseurs = new[]
        {
            new Fournisseur { Nom = "Textile Export SARL", Contact = "Mondher Jaziri", Email = "contact@textileexport.tn", Telephone = "+216 71 123 456", Adresse = "Ariana, Tunisie" },
            new Fournisseur { Nom = "ElectroPlus Distribution", Contact = "Sonia Khemiri", Email = "contact@electroplus.tn", Telephone = "+216 71 456 789", Adresse = "Tunis, Tunisie" },
            new Fournisseur { Nom = "AgroFood Tunisie", Contact = "Walid Nasri", Email = "contact@agrofood.tn", Telephone = "+216 73 222 333", Adresse = "Sousse, Tunisie" },
        };
        foreach (var fr in fournisseurs) await uow.Fournisseurs.AddAsync(fr);

        // ---------- Attributs de variante ----------
        var attributTaille = new AttributVariante { Nom = "Taille" };
        var attributCouleur = new AttributVariante { Nom = "Couleur" };
        attributTaille.Valeurs.Add(new ValeurAttribut { Valeur = "S" });
        attributTaille.Valeurs.Add(new ValeurAttribut { Valeur = "M" });
        attributTaille.Valeurs.Add(new ValeurAttribut { Valeur = "L" });
        attributTaille.Valeurs.Add(new ValeurAttribut { Valeur = "XL" });
        attributCouleur.Valeurs.Add(new ValeurAttribut { Valeur = "Noir" });
        attributCouleur.Valeurs.Add(new ValeurAttribut { Valeur = "Blanc" });
        attributCouleur.Valeurs.Add(new ValeurAttribut { Valeur = "Bleu" });
        await uow.AttributsVariantes.AddAsync(attributTaille);
        await uow.AttributsVariantes.AddAsync(attributCouleur);

        await uow.SaveChangesAsync(); // flush pour obtenir les Id générés avant les relations

        ValeurAttribut Valeur(AttributVariante attr, string val) => attr.Valeurs.First(v => v.Valeur == val);

        // ---------- Articles ----------
        var pull = NouvelArticle("VET-PULL-001", "Pull col rond", "Pull en coton, coupe classique.", familleVetements, marqueNike, unitePiece, ModeSuivi.Simple, 25m, 49.90m, 10, gereVariantes: true, imageSeed: "pull-noir");
        var tshirt = NouvelArticle("VET-TSHIRT-001", "T-shirt basique", "T-shirt 100% coton.", familleVetements, marqueGenerique, unitePiece, ModeSuivi.Simple, 8m, 19.90m, 20, gereVariantes: true, imageSeed: "tshirt-blanc");
        var casquette = NouvelArticle("ACC-CASQUETTE-001", "Casquette logo", "Casquette ajustable.", familleAccessoires, marqueNike, unitePiece, ModeSuivi.Simple, 6m, 14.90m, 10, gereVariantes: true, imageSeed: "casquette");
        var smartphone = NouvelArticle("ELEC-SMART-001", "Smartphone Galaxy A54", "Smartphone 128 Go, écran 6.4\".", familleElectronique, marqueSamsung, unitePiece, ModeSuivi.NumeroSerie, 450m, 699.90m, 5, imageSeed: "smartphone");
        var souris = NouvelArticle("ELEC-SOURIS-001", "Souris sans fil MX Master", "Souris ergonomique sans fil.", familleElectronique, marqueLogitech, unitePiece, ModeSuivi.Simple, 60m, 109.90m, 15, imageSeed: "souris");
        var huile = NouvelArticle("ALIM-HUILE-001", "Huile d'olive extra vierge 1L", "Huile d'olive première pression à froid.", familleAlimentation, marqueGenerique, uniteLitre, ModeSuivi.Lot, 9m, 15.90m, 30, suiviPeremption: true, imageSeed: "huile-olive");
        var pates = NouvelArticle("ALIM-PATES-001", "Pâtes alimentaires 500g", "Pâtes de semoule de blé dur.", familleAlimentation, marqueGenerique, uniteBoite, ModeSuivi.Lot, 1m, 2.50m, 50, suiviPeremption: true, imageSeed: "pates");
        var bureau = NouvelArticle("MOB-BUREAU-001", "Bureau réglable", "Bureau assis-debout électrique.", familleMobilier, marqueIkea, unitePiece, ModeSuivi.Simple, 220m, 399.90m, 3, imageSeed: "bureau-reglable");
        var chaise = NouvelArticle("MOB-CHAISE-001", "Chaise ergonomique", "Chaise de bureau à hauteur réglable.", familleMobilier, marqueIkea, unitePiece, ModeSuivi.Simple, 90m, 169.90m, 6, imageSeed: "chaise-bureau");
        var sac = NouvelArticle("ACC-SAC-001", "Sac à dos urbain", "Sac à dos imperméable avec compartiment ordinateur.", familleAccessoires, marqueGenerique, unitePiece, ModeSuivi.Simple, 20m, 39.90m, 8, imageSeed: "sac-a-dos");

        var tousArticles = new[] { pull, tshirt, casquette, smartphone, souris, huile, pates, bureau, chaise, sac };
        foreach (var a in tousArticles) await uow.Articles.AddAsync(a);

        // Attributs associés aux articles à variantes
        pull.AttributsVariantes.Add(new AttributVarianteArticle { ArticleId = pull.Id, AttributVarianteId = attributTaille.Id });
        pull.AttributsVariantes.Add(new AttributVarianteArticle { ArticleId = pull.Id, AttributVarianteId = attributCouleur.Id });
        tshirt.AttributsVariantes.Add(new AttributVarianteArticle { ArticleId = tshirt.Id, AttributVarianteId = attributTaille.Id });
        tshirt.AttributsVariantes.Add(new AttributVarianteArticle { ArticleId = tshirt.Id, AttributVarianteId = attributCouleur.Id });
        casquette.AttributsVariantes.Add(new AttributVarianteArticle { ArticleId = casquette.Id, AttributVarianteId = attributCouleur.Id });

        // ---------- Variantes concrètes ----------
        var pullNoirM = NouvelleVariante(pull, "VET-PULL-001-NOIR-M", Valeur(attributTaille, "M"), Valeur(attributCouleur, "Noir"));
        var pullNoirL = NouvelleVariante(pull, "VET-PULL-001-NOIR-L", Valeur(attributTaille, "L"), Valeur(attributCouleur, "Noir"));
        var pullBleuM = NouvelleVariante(pull, "VET-PULL-001-BLEU-M", Valeur(attributTaille, "M"), Valeur(attributCouleur, "Bleu"));
        var tshirtBlancS = NouvelleVariante(tshirt, "VET-TSHIRT-001-BLANC-S", Valeur(attributTaille, "S"), Valeur(attributCouleur, "Blanc"));
        var tshirtBlancM = NouvelleVariante(tshirt, "VET-TSHIRT-001-BLANC-M", Valeur(attributTaille, "M"), Valeur(attributCouleur, "Blanc"));
        var casquetteNoire = NouvelleVariante(casquette, "ACC-CASQUETTE-001-NOIR", Valeur(attributCouleur, "Noir"));
        var casquetteBlanche = NouvelleVariante(casquette, "ACC-CASQUETTE-001-BLANC", Valeur(attributCouleur, "Blanc"));

        foreach (var v in new[] { pullNoirM, pullNoirL, pullBleuM, tshirtBlancS, tshirtBlancM, casquetteNoire, casquetteBlanche })
            await uow.ArticleVariantes.AddAsync(v);

        await uow.SaveChangesAsync(); // flush pour obtenir les Id des articles / variantes

        // ---------- Lots (articles à suivi par lot) ----------
        var lotHuile = new Lot { ArticleId = huile.Id, NumeroLot = "LOT-HUILE-2026-01", DateFabrication = DateTime.UtcNow.AddMonths(-2), DateExpiration = DateTime.UtcNow.AddMonths(10) };
        var lotPates = new Lot { ArticleId = pates.Id, NumeroLot = "LOT-PATES-2026-03", DateFabrication = DateTime.UtcNow.AddMonths(-1), DateExpiration = DateTime.UtcNow.AddMonths(17) };
        await uow.Lots.AddAsync(lotHuile);
        await uow.Lots.AddAsync(lotPates);

        // ---------- Numéros de série (article à suivi par numéro de série) ----------
        var numerosSerieSmartphone = new List<NumeroSerie>();
        for (var i = 1; i <= 6; i++)
        {
            var depot = i <= 4 ? depotTunis : depotSousse;
            numerosSerieSmartphone.Add(new NumeroSerie { ArticleId = smartphone.Id, Numero = $"SN-A54-{i:D4}", DepotId = depot.Id, Statut = StatutNumeroSerie.EnStock });
        }
        foreach (var ns in numerosSerieSmartphone) await uow.NumerosSerie.AddAsync(ns);

        await uow.SaveChangesAsync();

        // ---------- Stock (réparti par dépôt) ----------
        var mouvements = new List<MouvementStock>();
        var stockItemsAAjouter = new List<StockItem>();

        void AjouterStock(Article article, ArticleVariante? variante, Depot depot, int quantite, Lot? lot = null)
        {
            var item = new StockItem { ArticleId = article.Id, ArticleVarianteId = variante?.Id, DepotId = depot.Id, LotId = lot?.Id, Quantite = quantite };
            stockItemsAAjouter.Add(item);
            mouvements.Add(new MouvementStock
            {
                Type = TypeMouvement.Entree,
                ArticleId = article.Id,
                ArticleVarianteId = variante?.Id,
                LotId = lot?.Id,
                DepotDestinationId = depot.Id,
                Quantite = quantite,
                Motif = "Stock initial",
                Reference = "SEED-INIT",
                UtilisateurId = adminId,
                UtilisateurNom = adminNom,
                DateMouvement = DateTime.UtcNow.AddDays(-14)
            });
        }

        AjouterStock(pull, pullNoirM, depotTunis, 18);
        AjouterStock(pull, pullNoirM, depotSousse, 4); // sous le seuil (10)
        AjouterStock(pull, pullNoirL, depotTunis, 12);
        AjouterStock(pull, pullBleuM, depotSfax, 9);

        AjouterStock(tshirt, tshirtBlancS, depotTunis, 25);
        AjouterStock(tshirt, tshirtBlancM, depotSousse, 30);

        AjouterStock(casquette, casquetteNoire, depotTunis, 14);
        AjouterStock(casquette, casquetteBlanche, depotSfax, 0); // épuisé

        AjouterStock(smartphone, null, depotTunis, 4); // sous le seuil (5)
        AjouterStock(smartphone, null, depotSousse, 2);

        AjouterStock(souris, null, depotTunis, 40);
        AjouterStock(souris, null, depotSfax, 8);

        AjouterStock(huile, null, depotTunis, 60, lotHuile);
        AjouterStock(huile, null, depotSousse, 15, lotHuile); // sous le seuil (30)

        AjouterStock(pates, null, depotTunis, 80, lotPates);

        AjouterStock(bureau, null, depotTunis, 2); // sous le seuil (3)
        AjouterStock(chaise, null, depotTunis, 10);
        AjouterStock(chaise, null, depotSousse, 3); // sous le seuil (6)

        AjouterStock(sac, null, depotSfax, 22);

        foreach (var item in stockItemsAAjouter) await uow.StockItems.AddAsync(item);
        foreach (var m in mouvements) await uow.Mouvements.AddAsync(m);

        // Quelques mouvements supplémentaires pour illustrer l'historique (sortie + transfert)
        var sortieRecente = new MouvementStock
        {
            Type = TypeMouvement.Sortie,
            ArticleId = souris.Id,
            DepotSourceId = depotTunis.Id,
            Quantite = 5,
            Motif = "Commande client #1042",
            Reference = "CMD-1042",
            UtilisateurId = adminId,
            UtilisateurNom = adminNom,
            DateMouvement = DateTime.UtcNow.AddDays(-2)
        };
        await uow.Mouvements.AddAsync(sortieRecente);

        var transfertId = Guid.NewGuid();
        var transfertSortie = new MouvementStock
        {
            Type = TypeMouvement.TransfertSortie,
            ArticleId = chaise.Id,
            DepotSourceId = depotTunis.Id,
            DepotDestinationId = depotSousse.Id,
            Quantite = 2,
            Motif = "Réassort dépôt Sousse",
            MouvementLieId = transfertId,
            UtilisateurId = adminId,
            UtilisateurNom = adminNom,
            DateMouvement = DateTime.UtcNow.AddDays(-1)
        };
        var transfertEntree = new MouvementStock
        {
            Type = TypeMouvement.TransfertEntree,
            ArticleId = chaise.Id,
            DepotSourceId = depotTunis.Id,
            DepotDestinationId = depotSousse.Id,
            Quantite = 2,
            Motif = "Réassort dépôt Sousse",
            MouvementLieId = transfertId,
            UtilisateurId = adminId,
            UtilisateurNom = adminNom,
            DateMouvement = DateTime.UtcNow.AddDays(-1)
        };
        await uow.Mouvements.AddAsync(transfertSortie);
        await uow.Mouvements.AddAsync(transfertEntree);

        // ---------- Notifications de stock faible ----------
        var alertes = new[]
        {
            new Notification { Type = TypeNotification.StockFaible, Message = "Stock faible pour Pull col rond (Noir/M) au dépôt Sousse.", ArticleId = pull.Id, DepotId = depotSousse.Id },
            new Notification { Type = TypeNotification.StockEpuise, Message = "Casquette logo (Blanc) en rupture au dépôt Sfax.", ArticleId = casquette.Id, DepotId = depotSfax.Id },
            new Notification { Type = TypeNotification.StockFaible, Message = "Stock faible pour Smartphone Galaxy A54 au dépôt Tunis.", ArticleId = smartphone.Id, DepotId = depotTunis.Id },
        };
        foreach (var n in alertes) await uow.Notifications.AddAsync(n);

        await uow.SaveChangesAsync();
    }

    private static Article NouvelArticle(
        string reference, string designation, string description,
        Famille famille, Marque marque, Unite unite, ModeSuivi modeSuivi,
        decimal prixAchat, decimal prixVente, int seuilMinimum,
        bool gereVariantes = false, bool suiviPeremption = false, string? imageSeed = null) => new()
    {
        Reference = reference,
        Designation = designation,
        Description = description,
        ImageUrl = imageSeed is null ? null : ImageParDefaut,
        FamilleId = famille.Id,
        MarqueId = marque.Id,
        UniteId = unite.Id,
        ModeSuivi = modeSuivi,
        GereVariantes = gereVariantes,
        SuiviDatePeremption = suiviPeremption,
        PrixAchat = prixAchat,
        PrixVente = prixVente,
        SeuilMinimum = seuilMinimum
    };

    private const string ImageParDefaut = "https://th.bing.com/th/id/R.c5209bbde8db4c412ed89da0e9425664?rik=uA%2f7OX9tqKMfxQ&pid=ImgRaw&r=0";

    private static ArticleVariante NouvelleVariante(Article article, string referenceVariante, params ValeurAttribut[] valeurs)
    {
        var variante = new ArticleVariante { ArticleId = article.Id, ReferenceVariante = referenceVariante };
        foreach (var valeur in valeurs)
            variante.Valeurs.Add(new ArticleVarianteValeur { ArticleVarianteId = variante.Id, ValeurAttributId = valeur.Id });
        return variante;
    }
}
