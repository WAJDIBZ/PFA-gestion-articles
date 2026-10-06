import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { articleApi, varianteApi } from "../api/articleApi";
import { familleApi, marqueApi, uniteApi } from "../api/catalogueApi";
import { stockApi } from "../api/stockApi";
import { useAuth } from "../context/AuthContext";

export default function ArticleDetail() {
  const { id } = useParams();
  const { aRole } = useAuth();
  const estAdmin = aRole("Administrateur");

  const [article, setArticle] = useState(null);
  const [variantes, setVariantes] = useState([]);
  const [attributs, setAttributs] = useState([]);
  const [stock, setStock] = useState(null);
  const [selection, setSelection] = useState({});
  const [referenceVariante, setReferenceVariante] = useState("");
  const [erreur, setErreur] = useState(null);
  const [resultatRecherche, setResultatRecherche] = useState(null);

  // --- edition ---
  const [afficherModal, setAfficherModal] = useState(false);
  const [formulaire, setFormulaire] = useState(null);
  const [familles, setFamilles] = useState([]);
  const [marques, setMarques] = useState([]);
  const [unites, setUnites] = useState([]);
  const [erreurEdition, setErreurEdition] = useState(null);
  const [enregistrement, setEnregistrement] = useState(false);

  const charger = () => {
    articleApi.obtenirParId(id).then(setArticle);
    varianteApi.obtenirParArticle(id).then(setVariantes);
    stockApi.obtenirParArticle(id).then(setStock);
  };

  useEffect(() => {
    charger();
    varianteApi.obtenirAttributs().then(setAttributs);
    familleApi.obtenirToutes().then(setFamilles);
    marqueApi.obtenirToutes().then(setMarques);
    uniteApi.obtenirToutes().then(setUnites);
  }, [id]);

  const ouvrirModal = () => {
    if (!article) return;
    setFormulaire({
      reference: article.reference ?? "",
      designation: article.designation ?? "",
      description: article.description ?? "",
      codeBarre: article.codeBarre ?? "",
      imageUrl: article.imageUrl ?? "",
      familleId: article.familleId ?? "",
      marqueId: article.marqueId ?? "",
      uniteId: article.uniteId ?? "",
      modeSuivi: article.modeSuivi ?? "Simple",
      gereVariantes: article.gereVariantes ?? false,
      suiviDatePeremption: article.suiviDatePeremption ?? false,
      prixAchat: article.prixAchat ?? 0,
      prixVente: article.prixVente ?? 0,
      seuilMinimum: article.seuilMinimum ?? 0,
      estActif: article.estActif ?? true,
    });
    setErreurEdition(null);
    setAfficherModal(true);
  };

  const majChamp = (champ, valeur) =>
    setFormulaire((f) => ({ ...f, [champ]: valeur }));

  const enregistrer = async (e) => {
    e.preventDefault();
    setErreurEdition(null);
    setEnregistrement(true);
    try {
      await articleApi.modifier(id, {
        ...formulaire,
        imageUrl: formulaire.imageUrl || null,
        marqueId: formulaire.marqueId || null,
        prixAchat: Number(formulaire.prixAchat),
        prixVente: Number(formulaire.prixVente),
        seuilMinimum: Number(formulaire.seuilMinimum),
      });
      setAfficherModal(false);
      charger();
    } catch (err) {
      setErreurEdition(
        err.response?.data?.message || "Erreur lors de la modification."
      );
    } finally {
      setEnregistrement(false);
    }
  };


  const majSelection = (attributId, valeurId) =>
    setSelection((s) => ({ ...s, [attributId]: valeurId }));

  const creerVariante = async (e) => {
    e.preventDefault();
    setErreur(null);
    const valeurAttributIds = Object.values(selection).filter(Boolean);
    if (valeurAttributIds.length === 0) {
      setErreur("Sélectionnez au moins une valeur d'attribut.");
      return;
    }
    try {
      await varianteApi.creer({
        articleId: id,
        referenceVariante,
        valeurAttributIds,
      });
      setReferenceVariante("");
      setSelection({});
      charger();
    } catch (err) {
      setErreur(
        err.response?.data?.message ||
          "Erreur lors de la création de la variante.",
      );
    }
  };

  const rechercherCombinaison = async () => {
    const valeurAttributIds = Object.values(selection).filter(Boolean);
    if (valeurAttributIds.length === 0) return;
    const r = await varianteApi.rechercherCombinaison({
      articleId: id,
      valeurAttributIds,
    });
    setResultatRecherche(r);
  };

  if (!article) return <div>Chargement...</div>;

  return (
    <div>
      <Link to="/articles" className="btn btn-link ps-0">
        &larr; Retour aux articles
      </Link>

      <div className="sg-card overflow-hidden mb-4">
        <div className="row g-0">
          <div className="col-md-4">
            <div
              className={`sg-article-media ${!article.estActif ? "est-inactif" : ""}`}
              style={{ aspectRatio: "auto", height: "100%", minHeight: 220 }}
            >
              {article.imageUrl ? (
                <img src={article.imageUrl} alt={article.designation} />
              ) : (
                <div className="sg-article-placeholder">
                  <i className="bi bi-image"></i>
                </div>
              )}
              <span className="sg-article-famille">{article.familleNom}</span>
            </div>
          </div>
          <div className="col-md-8 p-4">
            <div className="d-flex justify-content-between align-items-start mb-2">
              <div className="sg-article-ref">{article.reference}</div>
              {estAdmin && (
                <button
                  className="btn btn-sm btn-outline-secondary"
                  onClick={ouvrirModal}
                >
                  <i className="bi bi-pencil me-1"></i>Modifier
                </button>
              )}
            </div>
            <h3 className="fw-bold mb-2">{article.designation}</h3>
            <p className="text-muted mb-2">
              {article.description || "Aucune description."}
            </p>
            <p className="text-muted mb-0">
              Unité : {article.uniteNom}
              {article.marqueNom ? ` \u2022 Marque : ${article.marqueNom}` : ""}
            </p>
          </div>
        </div>
      </div>


      <div className="row g-3 mb-4">
        <div className="col-md-3">
          <div className="sg-card p-3 text-center">
            <small className="text-muted">Quantité totale</small>
            <h4 className="mb-0 mt-1">{article.quantiteTotale}</h4>
          </div>
        </div>
        <div className="col-md-3">
          <div className="sg-card p-3 text-center">
            <small>Seuil minimum</small>
            <h4>{article.seuilMinimum}</h4>
          </div>
        </div>
        <div className="col-md-3">
          <div className="sg-card p-3 text-center">
            <small>Prix d'achat</small>
            <h4>{article.prixAchat} DT</h4>
          </div>
        </div>
        <div className="col-md-3">
          <div className="sg-card p-3 text-center">
            <small>Prix de vente</small>
            <h4>{article.prixVente} DT</h4>
          </div>
        </div>
      </div>

      <div className="card mb-4">
        <div className="card-header fw-semibold">Répartition par dépôt</div>
        <table className="table table-sm mb-0">
          <thead>
            <tr>
              <th>Dépôt</th>
              <th>Variante</th>
              <th>Lot</th>
              <th>Quantité</th>
            </tr>
          </thead>
          <tbody>
            {stock?.repartition.map((s) => (
              <tr key={s.id}>
                <td>{s.depotNom}</td>
                <td>{s.articleVarianteReference ?? "-"}</td>
                <td>{s.numeroLot ?? "-"}</td>
                <td>{s.quantite}</td>
              </tr>
            ))}
            {stock?.repartition.length === 0 && (
              <tr>
                <td colSpan={4} className="text-center text-muted py-3">
                  Aucun stock
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      {article.gereVariantes && (
        <div className="card">
          <div className="card-header fw-semibold">Variantes</div>
          <div className="card-body">
            {erreur && <div className="alert alert-danger py-2">{erreur}</div>}

            <div className="row g-2 mb-3">
              {attributs.map((attr) => (
                <div className="col-md-3" key={attr.id}>
                  <label className="form-label">{attr.nom}</label>
                  <select
                    className="form-select"
                    value={selection[attr.id] || ""}
                    onChange={(e) => majSelection(attr.id, e.target.value)}
                  >
                    <option value="">-</option>
                    {attr.valeurs.map((v) => (
                      <option key={v.id} value={v.id}>
                        {v.valeur}
                      </option>
                    ))}
                  </select>
                </div>
              ))}
              <div className="col-md-3 d-flex align-items-end">
                <button
                  type="button"
                  className="btn btn-outline-secondary"
                  onClick={rechercherCombinaison}
                >
                  Vérifier disponibilité
                </button>
              </div>
            </div>

            {resultatRecherche && (
              <div
                className={`alert ${resultatRecherche.disponible ? "alert-success" : "alert-warning"}`}
              >
                {resultatRecherche.disponible
                  ? `Disponible: ${resultatRecherche.referenceVariante} (${resultatRecherche.quantiteTotale} en stock)`
                  : "Combinaison indisponible ou rupture de stock."}
              </div>
            )}

            {estAdmin && (
              <form
                onSubmit={creerVariante}
                className="d-flex gap-2 align-items-end mb-3"
              >
                <div>
                  <label className="form-label">Référence variante</label>
                  <input
                    className="form-control"
                    value={referenceVariante}
                    onChange={(e) => setReferenceVariante(e.target.value)}
                    required
                  />
                </div>
                <button className="btn btn-primary">
                  Créer cette combinaison
                </button>
              </form>
            )}

            <table className="table table-sm">
              <thead>
                <tr>
                  <th>Référence</th>
                  <th>Attributs</th>
                  <th>Quantité</th>
                </tr>
              </thead>
              <tbody>
                {variantes.map((v) => (
                  <tr key={v.id}>
                    <td>{v.referenceVariante}</td>
                    <td>{v.valeurs.map((val) => val.valeur).join(" / ")}</td>
                    <td>{v.quantiteTotale}</td>
                  </tr>
                ))}
                {variantes.length === 0 && (
                  <tr>
                    <td colSpan={3} className="text-center text-muted py-3">
                      Aucune variante
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {/* ===== Modal de modification ===== */}
      {afficherModal && formulaire && (
        <>
          <div
            className="modal-backdrop fade show"
            style={{ zIndex: 1040 }}
            onClick={() => setAfficherModal(false)}
          />
          <div
            className="modal fade show d-block"
            style={{ zIndex: 1050 }}
            tabIndex={-1}
            role="dialog"
          >
            <div className="modal-dialog modal-lg modal-dialog-scrollable" role="document">
              <div className="modal-content">
                <div className="modal-header">
                  <h5 className="modal-title">
                    <i className="bi bi-pencil-square me-2"></i>
                    Modifier l'article
                  </h5>
                  <button
                    type="button"
                    className="btn-close"
                    onClick={() => setAfficherModal(false)}
                  />
                </div>
                <form onSubmit={enregistrer}>
                  <div className="modal-body">
                    {erreurEdition && (
                      <div className="alert alert-danger py-2">{erreurEdition}</div>
                    )}
                    <div className="row g-3">
                      {/* Référence */}
                      <div className="col-md-4">
                        <label className="form-label">Référence</label>
                        <input
                          className="form-control"
                          value={formulaire.reference}
                          onChange={(e) => majChamp("reference", e.target.value)}
                          required
                        />
                      </div>
                      {/* Désignation */}
                      <div className="col-md-8">
                        <label className="form-label">Désignation</label>
                        <input
                          className="form-control"
                          value={formulaire.designation}
                          onChange={(e) => majChamp("designation", e.target.value)}
                          required
                        />
                      </div>
                      {/* Description */}
                      <div className="col-12">
                        <label className="form-label">Description</label>
                        <textarea
                          className="form-control"
                          rows={2}
                          value={formulaire.description}
                          onChange={(e) => majChamp("description", e.target.value)}
                        />
                      </div>
                      {/* Code-barres */}
                      <div className="col-md-4">
                        <label className="form-label">Code-barres</label>
                        <input
                          className="form-control"
                          value={formulaire.codeBarre}
                          onChange={(e) => majChamp("codeBarre", e.target.value)}
                        />
                      </div>
                      {/* URL image */}
                      <div className="col-md-8">
                        <label className="form-label">URL de l'image</label>
                        <input
                          className="form-control"
                          placeholder="https://..."
                          value={formulaire.imageUrl}
                          onChange={(e) => majChamp("imageUrl", e.target.value)}
                        />
                      </div>
                      {/* Famille */}
                      <div className="col-md-4">
                        <label className="form-label">Famille</label>
                        <select
                          className="form-select"
                          value={formulaire.familleId}
                          onChange={(e) => majChamp("familleId", e.target.value)}
                          required
                        >
                          <option value="">Choisir...</option>
                          {familles.map((f) => (
                            <option key={f.id} value={f.id}>{f.nom}</option>
                          ))}
                        </select>
                      </div>
                      {/* Marque */}
                      <div className="col-md-4">
                        <label className="form-label">Marque</label>
                        <select
                          className="form-select"
                          value={formulaire.marqueId}
                          onChange={(e) => majChamp("marqueId", e.target.value)}
                        >
                          <option value="">Aucune</option>
                          {marques.map((m) => (
                            <option key={m.id} value={m.id}>{m.nom}</option>
                          ))}
                        </select>
                      </div>
                      {/* Unité */}
                      <div className="col-md-4">
                        <label className="form-label">Unité</label>
                        <select
                          className="form-select"
                          value={formulaire.uniteId}
                          onChange={(e) => majChamp("uniteId", e.target.value)}
                          required
                        >
                          <option value="">Choisir...</option>
                          {unites.map((u) => (
                            <option key={u.id} value={u.id}>{u.nom} ({u.symbole})</option>
                          ))}
                        </select>
                      </div>
                      {/* Mode de suivi */}
                      <div className="col-md-4">
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
                      {/* Prix achat */}
                      <div className="col-md-4">
                        <label className="form-label">Prix d'achat (DT)</label>
                        <input
                          type="number"
                          step="0.01"
                          min="0"
                          className="form-control"
                          value={formulaire.prixAchat}
                          onChange={(e) => majChamp("prixAchat", e.target.value)}
                        />
                      </div>
                      {/* Prix vente */}
                      <div className="col-md-4">
                        <label className="form-label">Prix de vente (DT)</label>
                        <input
                          type="number"
                          step="0.01"
                          min="0"
                          className="form-control"
                          value={formulaire.prixVente}
                          onChange={(e) => majChamp("prixVente", e.target.value)}
                        />
                      </div>
                      {/* Seuil minimum */}
                      <div className="col-md-4">
                        <label className="form-label">Seuil minimum</label>
                        <input
                          type="number"
                          min="0"
                          className="form-control"
                          value={formulaire.seuilMinimum}
                          onChange={(e) => majChamp("seuilMinimum", e.target.value)}
                        />
                      </div>
                      {/* Checkboxes */}
                      <div className="col-md-4 d-flex align-items-end">
                        <div className="form-check">
                          <input
                            type="checkbox"
                            className="form-check-input"
                            id="editEstActif"
                            checked={formulaire.estActif}
                            onChange={(e) => majChamp("estActif", e.target.checked)}
                          />
                          <label className="form-check-label" htmlFor="editEstActif">
                            Article actif
                          </label>
                        </div>
                      </div>
                      <div className="col-md-4 d-flex align-items-end">
                        <div className="form-check">
                          <input
                            type="checkbox"
                            className="form-check-input"
                            id="editGereVariantes"
                            checked={formulaire.gereVariantes}
                            onChange={(e) => majChamp("gereVariantes", e.target.checked)}
                          />
                          <label className="form-check-label" htmlFor="editGereVariantes">
                            Gère des variantes
                          </label>
                        </div>
                      </div>
                      <div className="col-md-4 d-flex align-items-end">
                        <div className="form-check">
                          <input
                            type="checkbox"
                            className="form-check-input"
                            id="editSuiviPeremption"
                            checked={formulaire.suiviDatePeremption}
                            onChange={(e) => majChamp("suiviDatePeremption", e.target.checked)}
                          />
                          <label className="form-check-label" htmlFor="editSuiviPeremption">
                            Suivi date d'expiration
                          </label>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div className="modal-footer">
                    <button
                      type="button"
                      className="btn btn-outline-secondary"
                      onClick={() => setAfficherModal(false)}
                    >
                      Annuler
                    </button>
                    <button
                      type="submit"
                      className="btn sg-btn-gradient"
                      disabled={enregistrement}
                    >
                      {enregistrement ? (
                        <>
                          <span className="spinner-border spinner-border-sm me-2" />
                          Enregistrement…
                        </>
                      ) : (
                        <>
                          <i className="bi bi-check2 me-1"></i>Enregistrer
                        </>
                      )}
                    </button>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </>
      )}
    </div>
  );
}
