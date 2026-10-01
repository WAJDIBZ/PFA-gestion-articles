import { useEffect, useState } from "react";
import { articleApi } from "../api/articleApi";
import { depotApi, mouvementApi } from "../api/stockApi";

export default function Mouvements() {
  const [onglet, setOnglet] = useState("entree");
  const [articles, setArticles] = useState([]);
  const [depots, setDepots] = useState([]);
  const [historique, setHistorique] = useState({ items: [] });
  const [erreur, setErreur] = useState(null);
  const [succes, setSucces] = useState(null);

  const [entree, setEntree] = useState({
    articleId: "",
    depotDestinationId: "",
    quantite: 1,
    numeroLot: "",
    motif: "",
  });
  const [sortie, setSortie] = useState({
    articleId: "",
    depotSourceId: "",
    quantite: 1,
    motif: "",
  });
  const [transfert, setTransfert] = useState({
    articleId: "",
    depotSourceId: "",
    depotDestinationId: "",
    quantite: 1,
    motif: "",
  });

  const chargerHistorique = () =>
    mouvementApi.rechercher({ page: 1, pageSize: 20 }).then(setHistorique);

  useEffect(() => {
    articleApi
      .rechercher({ page: 1, pageSize: 200 })
      .then((r) => setArticles(r.items));
    depotApi.obtenirTous().then(setDepots);
    chargerHistorique();
  }, []);

  const gererErreur = (err) =>
    setErreur(err.response?.data?.message || "Une erreur est survenue.");

  const soumettreEntree = async (e) => {
    e.preventDefault();
    setErreur(null);
    setSucces(null);
    try {
      await mouvementApi.entree({
        ...entree,
        quantite: Number(entree.quantite),
        numeroLot: entree.numeroLot || null,
        motif: entree.motif || null,
      });
      setSucces("Entrée enregistrée.");
      chargerHistorique();
    } catch (err) {
      gererErreur(err);
    }
  };

  const soumettreSortie = async (e) => {
    e.preventDefault();
    setErreur(null);
    setSucces(null);
    try {
      await mouvementApi.sortie({
        ...sortie,
        quantite: Number(sortie.quantite),
        motif: sortie.motif || null,
      });
      setSucces("Sortie enregistrée.");
      chargerHistorique();
    } catch (err) {
      gererErreur(err);
    }
  };

  const soumettreTransfert = async (e) => {
    e.preventDefault();
    setErreur(null);
    setSucces(null);
    try {
      await mouvementApi.transfert({
        ...transfert,
        quantite: Number(transfert.quantite),
        motif: transfert.motif || null,
      });
      setSucces("Transfert enregistré.");
      chargerHistorique();
    } catch (err) {
      gererErreur(err);
    }
  };

  return (
    <div>
      <h3 className="mb-3">Mouvements de stock</h3>
      {erreur && <div className="alert alert-danger py-2">{erreur}</div>}
      {succes && <div className="alert alert-success py-2">{succes}</div>}

      <ul className="nav nav-tabs mb-3">
        {[
          ["entree", "Entrée"],
          ["sortie", "Sortie"],
          ["transfert", "Transfert"],
        ].map(([cle, label]) => (
          <li className="nav-item" key={cle}>
            <button
              className={`nav-link ${onglet === cle ? "active" : ""}`}
              onClick={() => setOnglet(cle)}
            >
              {label}
            </button>
          </li>
        ))}
      </ul>

      {onglet === "entree" && (
        <form
          onSubmit={soumettreEntree}
          className="card card-body mb-4 row g-2"
        >
          <div className="col-md-4">
            <label className="form-label">Article</label>
            <select
              className="form-select"
              value={entree.articleId}
              onChange={(e) =>
                setEntree({ ...entree, articleId: e.target.value })
              }
              required
            >
              <option value="">Choisir...</option>
              {articles.map((a) => (
                <option key={a.id} value={a.id}>
                  {a.reference} - {a.designation}
                </option>
              ))}
            </select>
          </div>
          <div className="col-md-4">
            <label className="form-label">Dépôt destination</label>
            <select
              className="form-select"
              value={entree.depotDestinationId}
              onChange={(e) =>
                setEntree({ ...entree, depotDestinationId: e.target.value })
              }
              required
            >
              <option value="">Choisir...</option>
              {depots.map((d) => (
                <option key={d.id} value={d.id}>
                  {d.nom}
                </option>
              ))}
            </select>
          </div>
          <div className="col-md-2">
            <label className="form-label">Quantité</label>
            <input
              type="number"
              min="1"
              className="form-control"
              value={entree.quantite}
              onChange={(e) =>
                setEntree({ ...entree, quantite: e.target.value })
              }
              required
            />
          </div>
          <div className="col-md-2">
            <label className="form-label">N° de lot</label>
            <input
              className="form-control"
              value={entree.numeroLot}
              onChange={(e) =>
                setEntree({ ...entree, numeroLot: e.target.value })
              }
            />
          </div>
          <div className="col-md-8">
            <label className="form-label">Motif / Référence</label>
            <input
              className="form-control"
              value={entree.motif}
              onChange={(e) => setEntree({ ...entree, motif: e.target.value })}
            />
          </div>
          <div className="col-12">
            <button className="btn btn-success">Enregistrer l'entrée</button>
          </div>
        </form>
      )}

      {onglet === "sortie" && (
        <form
          onSubmit={soumettreSortie}
          className="card card-body mb-4 row g-2"
        >
          <div className="col-md-4">
            <label className="form-label">Article</label>
            <select
              className="form-select"
              value={sortie.articleId}
              onChange={(e) =>
                setSortie({ ...sortie, articleId: e.target.value })
              }
              required
            >
              <option value="">Choisir...</option>
              {articles.map((a) => (
                <option key={a.id} value={a.id}>
                  {a.reference} - {a.designation}
                </option>
              ))}
            </select>
          </div>
          <div className="col-md-4">
            <label className="form-label">Dépôt source</label>
            <select
              className="form-select"
              value={sortie.depotSourceId}
              onChange={(e) =>
                setSortie({ ...sortie, depotSourceId: e.target.value })
              }
              required
            >
              <option value="">Choisir...</option>
              {depots.map((d) => (
                <option key={d.id} value={d.id}>
                  {d.nom}
                </option>
              ))}
            </select>
          </div>
          <div className="col-md-2">
            <label className="form-label">Quantité</label>
            <input
              type="number"
              min="1"
              className="form-control"
              value={sortie.quantite}
              onChange={(e) =>
                setSortie({ ...sortie, quantite: e.target.value })
              }
              required
            />
          </div>
          <div className="col-md-8">
            <label className="form-label">Motif / Référence</label>
            <input
              className="form-control"
              value={sortie.motif}
              onChange={(e) => setSortie({ ...sortie, motif: e.target.value })}
            />
          </div>
          <div className="col-12">
            <button className="btn btn-danger">Enregistrer la sortie</button>
          </div>
        </form>
      )}

      {onglet === "transfert" && (
        <form
          onSubmit={soumettreTransfert}
          className="card card-body mb-4 row g-2"
        >
          <div className="col-md-4">
            <label className="form-label">Article</label>
            <select
              className="form-select"
              value={transfert.articleId}
              onChange={(e) =>
                setTransfert({ ...transfert, articleId: e.target.value })
              }
              required
            >
              <option value="">Choisir...</option>
              {articles.map((a) => (
                <option key={a.id} value={a.id}>
                  {a.reference} - {a.designation}
                </option>
              ))}
            </select>
          </div>
          <div className="col-md-3">
            <label className="form-label">Dépôt source</label>
            <select
              className="form-select"
              value={transfert.depotSourceId}
              onChange={(e) =>
                setTransfert({ ...transfert, depotSourceId: e.target.value })
              }
              required
            >
              <option value="">Choisir...</option>
              {depots.map((d) => (
                <option key={d.id} value={d.id}>
                  {d.nom}
                </option>
              ))}
            </select>
          </div>
          <div className="col-md-3">
            <label className="form-label">Dépôt destination</label>
            <select
              className="form-select"
              value={transfert.depotDestinationId}
              onChange={(e) =>
                setTransfert({
                  ...transfert,
                  depotDestinationId: e.target.value,
                })
              }
              required
            >
              <option value="">Choisir...</option>
              {depots.map((d) => (
                <option key={d.id} value={d.id}>
                  {d.nom}
                </option>
              ))}
            </select>
          </div>
          <div className="col-md-2">
            <label className="form-label">Quantité</label>
            <input
              type="number"
              min="1"
              className="form-control"
              value={transfert.quantite}
              onChange={(e) =>
                setTransfert({ ...transfert, quantite: e.target.value })
              }
              required
            />
          </div>
          <div className="col-md-8">
            <label className="form-label">Motif / Référence</label>
            <input
              className="form-control"
              value={transfert.motif}
              onChange={(e) =>
                setTransfert({ ...transfert, motif: e.target.value })
              }
            />
          </div>
          <div className="col-12">
            <button className="btn btn-primary">
              Enregistrer le transfert
            </button>
          </div>
        </form>
      )}

      <h5>Historique</h5>
      <table className="table table-sm bg-white">
        <thead>
          <tr>
            <th>Date</th>
            <th>Type</th>
            <th>Article</th>
            <th>Source</th>
            <th>Destination</th>
            <th>Quantité</th>
            <th>Utilisateur</th>
          </tr>
        </thead>
        <tbody>
          {historique.items.map((m) => (
            <tr key={m.id}>
              <td>{new Date(m.dateMouvement).toLocaleString()}</td>
              <td>{m.type}</td>
              <td>{m.articleDesignation}</td>
              <td>{m.depotSourceNom ?? "-"}</td>
              <td>{m.depotDestinationNom ?? "-"}</td>
              <td>{m.quantite}</td>
              <td>{m.utilisateurNom}</td>
            </tr>
          ))}
          {historique.items.length === 0 && (
            <tr>
              <td colSpan={7} className="text-center text-muted py-3">
                Aucun mouvement
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
