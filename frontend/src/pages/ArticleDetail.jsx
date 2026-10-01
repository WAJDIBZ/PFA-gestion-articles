import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { articleApi, varianteApi } from "../api/articleApi";
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

  const charger = () => {
    articleApi.obtenirParId(id).then(setArticle);
    varianteApi.obtenirParArticle(id).then(setVariantes);
    stockApi.obtenirParArticle(id).then(setStock);
  };

  useEffect(() => {
    charger();
    varianteApi.obtenirAttributs().then(setAttributs);
  }, [id]);

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
            <div className={`sg-article-media ${!article.estActif ? "est-inactif" : ""}`} style={{ aspectRatio: "auto", height: "100%", minHeight: 220 }}>
              {article.imageUrl ? (
                <img src={article.imageUrl} alt={article.designation} />
              ) : (
                <div className="sg-article-placeholder"><i className="bi bi-image"></i></div>
              )}
              <span className="sg-article-famille">{article.familleNom}</span>
            </div>
          </div>
          <div className="col-md-8 p-4">
            <div className="sg-article-ref">{article.reference}</div>
            <h3 className="fw-bold mb-2">{article.designation}</h3>
            <p className="text-muted mb-2">{article.description || "Aucune description."}</p>
            <p className="text-muted mb-0">
              Unité : {article.uniteNom}{article.marqueNom ? ` \u2022 Marque : ${article.marqueNom}` : ""}
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
    </div>
  );
}
