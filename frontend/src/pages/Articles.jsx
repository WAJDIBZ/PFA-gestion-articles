import { useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import { articleApi } from "../api/articleApi";
import { familleApi, marqueApi, uniteApi } from "../api/catalogueApi";
import { useAuth } from "../context/AuthContext";

const videFormulaire = {
  reference: "",
  designation: "",
  description: "",
  codeBarre: "",
  imageUrl: "",
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

function statutStock(article) {
  if (article.quantiteTotale <= 0)
    return { cle: "epuise", label: "Épuisé", couleur: "#dc2626" };
  if (article.quantiteTotale <= article.seuilMinimum)
    return { cle: "faible", label: "Stock faible", couleur: "#f59e0b" };
  return { cle: "normal", label: "En stock", couleur: "#16a34a" };
}

export default function Articles() {
  const { aRole } = useAuth();
  const estAdmin = aRole("Administrateur");

  const [resultat, setResultat] = useState({
    items: [],
    totalCount: 0,
    page: 1,
    pageSize: 60,
  });
  const [texte, setTexte] = useState("");
  const [familleFiltre, setFammilleFiltre] = useState("");
  const [statutFiltre, setStatutFiltre] = useState("tous");
  const [familles, setFamilles] = useState([]);
  const [marques, setMarques] = useState([]);
  const [unites, setUnites] = useState([]);
  const [afficherForm, setAfficherForm] = useState(false);
  const [formulaire, setFormulaire] = useState(videFormulaire);
  const [erreur, setErreur] = useState(null);

  const charger = () =>
    articleApi
      .rechercher({
        texte: texte || undefined,
        familleId: familleFiltre || undefined,
        page: 1,
        pageSize: 60,
      })
      .then(setResultat);

  useEffect(() => {
    charger();
    familleApi.obtenirToutes().then(setFamilles);
    marqueApi.obtenirToutes().then(setMarques);
    uniteApi.obtenirToutes().then(setUnites);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const rechercher = (e) => {
    e?.preventDefault();
    charger();
  };

  const articlesAffiches = useMemo(() => {
    if (statutFiltre === "tous") return resultat.items;
    return resultat.items.filter((a) => statutStock(a).cle === statutFiltre);
  }, [resultat.items, statutFiltre]);

  const majChamp = (champ, valeur) =>
    setFormulaire((f) => ({ ...f, [champ]: valeur }));

  const creer = async (e) => {
    e.preventDefault();
    setErreur(null);
    try {
      await articleApi.creer({
        ...formulaire,
        imageUrl: formulaire.imageUrl || null,
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

            <div className="col-md-8">
              <label className="form-label">URL de l'image</label>
              <input
                className="form-control"
                placeholder="https://..."
                value={formulaire.imageUrl}
                onChange={(e) => majChamp("imageUrl", e.target.value)}
              />
            </div>
            <div className="col-md-4 d-flex align-items-end">
              {formulaire.imageUrl && (
                <img
                  src={formulaire.imageUrl}
                  alt="Aperçu"
                  className="rounded-3 border"
                  style={{ width: 70, height: 52, objectFit: "cover" }}
                />
              )}
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

      <div className="sg-filter-bar mb-4">
        <form onSubmit={rechercher} className="row g-2 align-items-end">
          <div className="col-md-4">
            <label className="form-label small fw-semibold text-muted mb-1">
              Recherche
            </label>
            <div className="input-group">
              <span className="input-group-text bg-white">
                <i className="bi bi-search"></i>
              </span>
              <input
                className="form-control"
                placeholder="Référence ou désignation..."
                value={texte}
                onChange={(e) => setTexte(e.target.value)}
              />
            </div>
          </div>
          <div className="col-md-3">
            <label className="form-label small fw-semibold text-muted mb-1">
              Famille
            </label>
            <select
              className="form-select"
              value={familleFiltre}
              onChange={(e) => setFammilleFiltre(e.target.value)}
            >
              <option value="">Toutes les familles</option>
              {familles.map((f) => (
                <option key={f.id} value={f.id}>
                  {f.nom}
                </option>
              ))}
            </select>
          </div>
          <div className="col-md-3">
            <label className="form-label small fw-semibold text-muted mb-1">
              Statut du stock
            </label>
            <select
              className="form-select"
              value={statutFiltre}
              onChange={(e) => setStatutFiltre(e.target.value)}
            >
              <option value="tous">Tous les statuts</option>
              <option value="normal">En stock</option>
              <option value="faible">Stock faible</option>
              <option value="epuise">Épuisé</option>
            </select>
          </div>
          <div className="col-md-2">
            <button className="btn sg-btn-gradient w-100" type="submit">
              Filtrer
            </button>
          </div>
        </form>
      </div>

      <div className="row g-4">
        {articlesAffiches.map((a) => {
          const statut = statutStock(a);
          return (
            <div className="col-sm-6 col-lg-4 col-xl-3" key={a.id}>
              <div className="sg-article-card">
                <div
                  className={`sg-article-media ${!a.estActif ? "est-inactif" : ""}`}
                >
                  {a.imageUrl ? (
                    <img src={a.imageUrl} alt={a.designation} loading="lazy" />
                  ) : (
                    <div className="sg-article-placeholder">
                      <i className="bi bi-image"></i>
                    </div>
                  )}
                  <div className="sg-article-overlay"></div>
                  {statut.cle !== "normal" && (
                    <span
                      className="sg-article-ribbon"
                      style={{ background: statut.couleur }}
                    >
                      {statut.label}
                    </span>
                  )}
                  <span className="sg-article-famille">{a.familleNom}</span>
                </div>
                <div className="sg-article-body">
                  <div className="sg-article-ref">{a.reference}</div>
                  <h6 className="sg-article-title">{a.designation}</h6>
                  <div className="d-flex gap-2 mb-2 flex-wrap">
                    <span className="badge text-bg-light border">
                      {a.modeSuivi}
                    </span>
                    <span className="badge text-bg-light border">
                      {a.quantiteTotale}{" "}
                      {a.quantiteTotale > 1 ? "unités" : "unité"}
                    </span>
                  </div>
                  <div className="sg-article-footer">
                    <span className="sg-article-price">
                      {a.prixVente.toFixed(2)} DT
                    </span>
                    <Link
                      to={`/articles/${a.id}`}
                      className="btn btn-sm btn-outline-primary"
                    >
                      Détails <i className="bi bi-arrow-right ms-1"></i>
                    </Link>
                  </div>
                </div>
              </div>
            </div>
          );
        })}

        {articlesAffiches.length === 0 && (
          <div className="col-12">
            <div className="sg-card text-center text-muted py-5">
              <i className="bi bi-inboxes fs-1 d-block mb-2"></i>
              Aucun article ne correspond à ces critères.
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
