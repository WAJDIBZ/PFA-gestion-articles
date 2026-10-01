import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { articleApi } from "../api/articleApi";
import { familleApi, marqueApi, uniteApi } from "../api/catalogueApi";
import { useAuth } from "../context/AuthContext";

const videFormulaire = {
  reference: "",
  designation: "",
  description: "",
  codeBarre: "",
  familleId: "",
  marqueId: "",
  uniteId: "",
  modeSuivi: "Simple",
  gereVariantes: false,
  suiviDatePeremption: false,
  prixAchat: 0,
  prixVente: 0,
  seuilMinimum: 0,
};

export default function Articles() {
  const { aRole } = useAuth();
  const estAdmin = aRole("Administrateur");

  const [resultat, setResultat] = useState({
    items: [],
    totalCount: 0,
    page: 1,
    pageSize: 20,
  });
  const [texte, setTexte] = useState("");
  const [familles, setFamilles] = useState([]);
  const [marques, setMarques] = useState([]);
  const [unites, setUnites] = useState([]);
  const [afficherForm, setAfficherForm] = useState(false);
  const [formulaire, setFormulaire] = useState(videFormulaire);
  const [erreur, setErreur] = useState(null);

  const charger = (page = 1) =>
    articleApi
      .rechercher({ texte: texte || undefined, page, pageSize: 20 })
      .then(setResultat);

  useEffect(() => {
    charger();
    familleApi.obtenirToutes().then(setFamilles);
    marqueApi.obtenirToutes().then(setMarques);
    uniteApi.obtenirToutes().then(setUnites);
  }, []);

  const rechercher = (e) => {
    e.preventDefault();
    charger(1);
  };

  const majChamp = (champ, valeur) =>
    setFormulaire((f) => ({ ...f, [champ]: valeur }));

  const creer = async (e) => {
    e.preventDefault();
    setErreur(null);
    try {
      await articleApi.creer({
        ...formulaire,
        marqueId: formulaire.marqueId || null,
        prixAchat: Number(formulaire.prixAchat),
        prixVente: Number(formulaire.prixVente),
        seuilMinimum: Number(formulaire.seuilMinimum),
        attributsVarianteIds: [],
      });
      setFormulaire(videFormulaire);
      setAfficherForm(false);
      charger();
    } catch (err) {
      setErreur(err.response?.data?.message || "Erreur lors de la création.");
    }
  };

  return (
    <div>
      <div className="sg-page-header">
        <div>
          <h3>
            <span className="sg-icon-badge">
              <i className="bi bi-box-seam"></i>
            </span>
            Articles
          </h3>
          <div className="sg-subtitle">
            Catalogue des articles et de leurs quantités en stock.
          </div>
        </div>
        {estAdmin && (
          <button
            className="btn sg-btn-gradient"
            onClick={() => setAfficherForm((v) => !v)}
          >
            <i className="bi bi-plus-lg me-1"></i>
            {afficherForm ? "Annuler" : "Nouvel article"}
          </button>
        )}
      </div>

      {afficherForm && (
        <form onSubmit={creer} className="sg-card p-4 mb-4">
          {erreur && <div className="alert alert-danger py-2">{erreur}</div>}
          <div className="row g-2">
            <div className="col-md-3">
              <label className="form-label">Référence</label>
              <input
                className="form-control"
                value={formulaire.reference}
                onChange={(e) => majChamp("reference", e.target.value)}
                required
              />
            </div>
            <div className="col-md-5">
              <label className="form-label">Désignation</label>
              <input
                className="form-control"
                value={formulaire.designation}
                onChange={(e) => majChamp("designation", e.target.value)}
                required
              />
            </div>
            <div className="col-md-4">
              <label className="form-label">Code-barres</label>
              <input
                className="form-control"
                value={formulaire.codeBarre}
                onChange={(e) => majChamp("codeBarre", e.target.value)}
              />
            </div>

            <div className="col-md-3">
              <label className="form-label">Famille</label>
              <select
                className="form-select"
                value={formulaire.familleId}
                onChange={(e) => majChamp("familleId", e.target.value)}
                required
              >
                <option value="">Choisir...</option>
                {familles.map((f) => (
                  <option key={f.id} value={f.id}>
                    {f.nom}
                  </option>
                ))}
              </select>
            </div>
            <div className="col-md-3">
              <label className="form-label">Marque</label>
              <select
                className="form-select"
                value={formulaire.marqueId}
                onChange={(e) => majChamp("marqueId", e.target.value)}
              >
                <option value="">Aucune</option>
                {marques.map((m) => (
                  <option key={m.id} value={m.id}>
                    {m.nom}
                  </option>
                ))}
              </select>
            </div>
            <div className="col-md-3">
              <label className="form-label">Unité</label>
              <select
                className="form-select"
                value={formulaire.uniteId}
                onChange={(e) => majChamp("uniteId", e.target.value)}
                required
              >
                <option value="">Choisir...</option>
                {unites.map((u) => (
                  <option key={u.id} value={u.id}>
                    {u.nom} ({u.symbole})
                  </option>
                ))}
              </select>
            </div>
            <div className="col-md-3">
              <label className="form-label">Mode de suivi</label>
              <select
                className="form-select"
                value={formulaire.modeSuivi}
                onChange={(e) => majChamp("modeSuivi", e.target.value)}
              >
                <option value="Simple">Simple</option>
                <option value="Lot">Par lot</option>
                <option value="NumeroSerie">Numéro de série</option>
              </select>
            </div>

            <div className="col-md-2">
              <label className="form-label">Prix d'achat</label>
              <input
                type="number"
                step="0.01"
                className="form-control"
                value={formulaire.prixAchat}
                onChange={(e) => majChamp("prixAchat", e.target.value)}
              />
            </div>
            <div className="col-md-2">
              <label className="form-label">Prix de vente</label>
              <input
                type="number"
                step="0.01"
                className="form-control"
                value={formulaire.prixVente}
                onChange={(e) => majChamp("prixVente", e.target.value)}
              />
            </div>
            <div className="col-md-2">
              <label className="form-label">Seuil minimum</label>
              <input
                type="number"
                className="form-control"
                value={formulaire.seuilMinimum}
                onChange={(e) => majChamp("seuilMinimum", e.target.value)}
              />
            </div>
            <div className="col-md-3 d-flex align-items-end">
              <div className="form-check">
                <input
                  type="checkbox"
                  className="form-check-input"
                  id="gereVariantes"
                  checked={formulaire.gereVariantes}
                  onChange={(e) => majChamp("gereVariantes", e.target.checked)}
                />
                <label className="form-check-label" htmlFor="gereVariantes">
                  Gère des variantes
                </label>
              </div>
            </div>
            <div className="col-md-3 d-flex align-items-end">
              <div className="form-check">
                <input
                  type="checkbox"
                  className="form-check-input"
                  id="suiviPeremption"
                  checked={formulaire.suiviDatePeremption}
                  onChange={(e) =>
                    majChamp("suiviDatePeremption", e.target.checked)
                  }
                />
                <label className="form-check-label" htmlFor="suiviPeremption">
                  Suivi date d'expiration
                </label>
              </div>
            </div>
          </div>
          <button className="btn sg-btn-gradient mt-3" style={{ width: 200 }}>
            Enregistrer
          </button>
        </form>
      )}

      <form
        onSubmit={rechercher}
        className="input-group mb-3"
        style={{ maxWidth: 400 }}
      >
        <span className="input-group-text bg-white border-end-0">
          <i className="bi bi-search"></i>
        </span>
        <input
          className="form-control border-start-0"
          placeholder="Rechercher un article..."
          value={texte}
          onChange={(e) => setTexte(e.target.value)}
        />
        <button className="btn btn-outline-secondary">Rechercher</button>
      </form>

      <div className="sg-card overflow-hidden">
        <table className="table table-modern table-hover mb-0">
          <thead>
            <tr>
              <th className="ps-3">Référence</th>
              <th>Désignation</th>
              <th>Famille</th>
              <th>Suivi</th>
              <th>Quantité</th>
              <th>Seuil</th>
              <th className="pe-3"></th>
            </tr>
          </thead>
          <tbody>
            {resultat.items.map((a) => (
              <tr
                key={a.id}
                className={
                  a.quantiteTotale <= a.seuilMinimum ? "table-warning" : ""
                }
              >
                <td className="ps-3">{a.reference}</td>
                <td>{a.designation}</td>
                <td>{a.familleNom}</td>
                <td>
                  <span className="badge text-bg-light border">
                    {a.modeSuivi}
                  </span>
                </td>
                <td className="fw-semibold">{a.quantiteTotale}</td>
                <td>{a.seuilMinimum}</td>
                <td className="pe-3">
                  <Link
                    to={`/articles/${a.id}`}
                    className="btn btn-sm btn-outline-primary"
                  >
                    Détails
                  </Link>
                </td>
              </tr>
            ))}
            {resultat.items.length === 0 && (
              <tr>
                <td colSpan={7} className="text-center text-muted py-4">
                  Aucun article
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
