import { useEffect, useState } from "react";
import { depotApi } from "../api/stockApi";
import { useAuth } from "../context/AuthContext";

export default function Depots() {
  const { aRole } = useAuth();
  const estAdmin = aRole("Administrateur");
  const [depots, setDepots] = useState([]);
  const [afficherForm, setAfficherForm] = useState(false);
  const [nom, setNom] = useState("");
  const [adresse, setAdresse] = useState("");
  const [responsable, setResponsable] = useState("");

  const charger = () => depotApi.obtenirTous().then(setDepots);
  useEffect(() => {
    charger();
  }, []);

  const creer = async (e) => {
    e.preventDefault();
    await depotApi.creer({
      nom,
      adresse: adresse || null,
      responsable: responsable || null,
    });
    setNom("");
    setAdresse("");
    setResponsable("");
    setAfficherForm(false);
    charger();
  };

  const supprimer = async (id) => {
    if (!confirm("Supprimer ce dépôt ?")) return;
    await depotApi.supprimer(id);
    charger();
  };

  return (
    <div>
      <div className="sg-page-header">
        <div>
          <h3>
            <span className="sg-icon-badge">
              <i className="bi bi-building"></i>
            </span>
            Dépôts
          </h3>
          <div className="sg-subtitle">
            Sites de stockage et leurs quantités.
          </div>
        </div>
        {estAdmin && (
          <button
            className="btn sg-btn-gradient"
            onClick={() => setAfficherForm((v) => !v)}
          >
            <i className="bi bi-plus-lg me-1"></i>
            {afficherForm ? "Annuler" : "Nouveau dépôt"}
          </button>
        )}
      </div>

      {afficherForm && (
        <form onSubmit={creer} className="sg-card p-4 mb-4 row g-2">
          <div className="col-md-4">
            <label className="form-label">Nom</label>
            <input
              className="form-control"
              value={nom}
              onChange={(e) => setNom(e.target.value)}
              required
            />
          </div>
          <div className="col-md-4">
            <label className="form-label">Adresse</label>
            <input
              className="form-control"
              value={adresse}
              onChange={(e) => setAdresse(e.target.value)}
            />
          </div>
          <div className="col-md-4">
            <label className="form-label">Responsable</label>
            <input
              className="form-control"
              value={responsable}
              onChange={(e) => setResponsable(e.target.value)}
            />
          </div>
          <div className="col-12">
            <button className="btn sg-btn-gradient">Enregistrer</button>
          </div>
        </form>
      )}

      <div className="row g-3">
        {depots.map((d) => (
          <div className="col-md-4" key={d.id}>
            <div className="sg-card h-100">
              <div className="card-body">
                <div className="d-flex align-items-center gap-2 mb-2">
                  <span
                    className="sg-icon-badge"
                    style={{ width: 34, height: 34, fontSize: "0.9rem" }}
                  >
                    <i className="bi bi-building"></i>
                  </span>
                  <h5 className="card-title mb-0">
                    {d.nom}{" "}
                    {!d.estActif && (
                      <span className="badge bg-secondary">Inactif</span>
                    )}
                  </h5>
                </div>
                <p className="card-text text-muted mb-1">{d.adresse}</p>
                <p className="card-text text-muted">
                  Responsable: {d.responsable ?? "-"}
                </p>
                <div className="d-flex gap-2 mb-3">
                  <span className="badge text-bg-light border">
                    {d.nombreArticlesDistincts} articles
                  </span>
                  <span className="badge text-bg-light border">
                    {d.quantiteTotale} unités
                  </span>
                </div>
                {estAdmin && (
                  <button
                    className="btn btn-sm btn-outline-danger"
                    onClick={() => supprimer(d.id)}
                  >
                    Supprimer
                  </button>
                )}
              </div>
            </div>
          </div>
        ))}
        {depots.length === 0 && <p className="text-muted">Aucun dépôt.</p>}
      </div>
    </div>
  );
}
