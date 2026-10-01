import { useEffect, useState } from "react";
import { dashboardApi } from "../api/miscApi";

export default function Dashboard() {
  const [donnees, setDonnees] = useState(null);
  const [chargement, setChargement] = useState(true);

  useEffect(() => {
    dashboardApi
      .obtenir()
      .then(setDonnees)
      .finally(() => setChargement(false));
  }, []);

  if (chargement) {
    return (
      <div className="d-flex justify-content-center py-5">
        <div className="spinner-border text-primary" role="status"></div>
      </div>
    );
  }
  if (!donnees)
    return (
      <div className="alert alert-danger">
        Impossible de charger le tableau de bord.
      </div>
    );

  const cartes = [
    {
      titre: "Articles actifs",
      valeur: donnees.nombreArticles,
      icone: "bi-box-seam",
      degrade: "linear-gradient(135deg,#4f46e5,#818cf8)",
    },
    {
      titre: "Quantité totale en stock",
      valeur: donnees.quantiteTotaleStock,
      icone: "bi-boxes",
      degrade: "linear-gradient(135deg,#0891b2,#06b6d4)",
    },
    {
      titre: "Articles en alerte",
      valeur: donnees.nombreArticlesEnAlerte,
      icone: "bi-exclamation-triangle",
      degrade: "linear-gradient(135deg,#d97706,#f59e0b)",
    },
    {
      titre: "Articles épuisés",
      valeur: donnees.nombreArticlesEpuises,
      icone: "bi-x-octagon",
      degrade: "linear-gradient(135deg,#b91c1c,#dc2626)",
    },
    {
      titre: "Dépôts",
      valeur: donnees.nombreDepots,
      icone: "bi-building",
      degrade: "linear-gradient(135deg,#334155,#64748b)",
    },
    {
      titre: "Utilisateurs actifs",
      valeur: donnees.nombreUtilisateurs,
      icone: "bi-people",
      degrade: "linear-gradient(135deg,#171a2e,#3730a3)",
    },
  ];

  return (
    <div>
      <div className="sg-page-header">
        <div>
          <h3>
            <span className="sg-icon-badge">
              <i className="bi bi-speedometer2"></i>
            </span>
            Tableau de bord
          </h3>
          <div className="sg-subtitle">
            Vue d'ensemble de votre activité de stock.
          </div>
        </div>
      </div>

      <div className="row g-3 mb-4">
        {cartes.map((c) => (
          <div className="col-md-4 col-lg-2" key={c.titre}>
            <div className="sg-stat-card" style={{ background: c.degrade }}>
              <i className={`bi ${c.icone} sg-stat-icon`}></i>
              <div className="sg-stat-label">{c.titre}</div>
              <div className="sg-stat-value">{c.valeur}</div>
            </div>
          </div>
        ))}
      </div>

      <div className="row g-3">
        <div className="col-lg-7">
          <div className="sg-card h-100">
            <div className="card-header bg-white fw-semibold border-0 pt-3 px-3">
              <i className="bi bi-clock-history me-2 text-primary"></i>Derniers
              mouvements
            </div>
            <div className="table-responsive">
              <table className="table table-modern table-sm mb-0 align-middle">
                <thead>
                  <tr>
                    <th className="ps-3">Date</th>
                    <th>Type</th>
                    <th>Article</th>
                    <th>Quantité</th>
                    <th className="pe-3">Utilisateur</th>
                  </tr>
                </thead>
                <tbody>
                  {donnees.derniersMouvements.map((m) => (
                    <tr key={m.id}>
                      <td className="ps-3">
                        {new Date(m.dateMouvement).toLocaleString()}
                      </td>
                      <td>
                        <span className="badge text-bg-light border">
                          {m.type}
                        </span>
                      </td>
                      <td>{m.articleDesignation}</td>
                      <td className="fw-semibold">{m.quantite}</td>
                      <td className="pe-3">{m.utilisateurNom}</td>
                    </tr>
                  ))}
                  {donnees.derniersMouvements.length === 0 && (
                    <tr>
                      <td colSpan={5} className="text-center text-muted py-4">
                        Aucun mouvement
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </div>
        </div>

        <div className="col-lg-5">
          <div className="sg-card h-100">
            <div className="card-header bg-white fw-semibold border-0 pt-3 px-3">
              <i className="bi bi-exclamation-triangle me-2 text-warning"></i>
              Articles en alerte de stock
            </div>
            <ul className="list-group list-group-flush">
              {donnees.articlesEnAlerte.map((a) => (
                <li
                  className="list-group-item d-flex justify-content-between align-items-center px-3"
                  key={a.articleId}
                >
                  <span>
                    {a.reference} - {a.designation}
                  </span>
                  <span
                    className={`badge rounded-pill ${a.quantiteActuelle <= 0 ? "bg-danger" : "bg-warning text-dark"}`}
                  >
                    {a.quantiteActuelle} / {a.seuilMinimum}
                  </span>
                </li>
              ))}
              {donnees.articlesEnAlerte.length === 0 && (
                <li className="list-group-item text-center text-muted py-4">
                  Aucune alerte
                </li>
              )}
            </ul>
          </div>
        </div>
      </div>
    </div>
  );
}
