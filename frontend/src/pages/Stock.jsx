import { useEffect, useState } from "react";
import { depotApi, stockApi } from "../api/stockApi";

export default function Stock() {
  const [depots, setDepots] = useState([]);
  const [depotId, setDepotId] = useState("");
  const [items, setItems] = useState([]);
  const [alertes, setAlertes] = useState([]);
  const [onglet, setOnglet] = useState("parDepot");

  useEffect(() => {
    depotApi.obtenirTous().then((d) => {
      setDepots(d);
      if (d.length > 0) setDepotId(d[0].id);
    });
    stockApi.obtenirAlertes().then(setAlertes);
  }, []);

  useEffect(() => {
    if (depotId) stockApi.obtenirParDepot(depotId).then(setItems);
  }, [depotId]);

  return (
    <div>
      <h3 className="mb-3">Stock</h3>
      <ul className="nav nav-tabs mb-3">
        <li className="nav-item">
          <button
            className={`nav-link ${onglet === "parDepot" ? "active" : ""}`}
            onClick={() => setOnglet("parDepot")}
          >
            Par dépôt
          </button>
        </li>
        <li className="nav-item">
          <button
            className={`nav-link ${onglet === "alertes" ? "active" : ""}`}
            onClick={() => setOnglet("alertes")}
          >
            Alertes de stock faible
          </button>
        </li>
      </ul>

      {onglet === "parDepot" && (
        <>
          <select
            className="form-select mb-3"
            style={{ maxWidth: 300 }}
            value={depotId}
            onChange={(e) => setDepotId(e.target.value)}
          >
            {depots.map((d) => (
              <option key={d.id} value={d.id}>
                {d.nom}
              </option>
            ))}
          </select>
          <table className="table table-hover bg-white">
            <thead>
              <tr>
                <th>Référence</th>
                <th>Article</th>
                <th>Variante</th>
                <th>Lot</th>
                <th>Quantité</th>
                <th>Seuil</th>
              </tr>
            </thead>
            <tbody>
              {items.map((i) => (
                <tr key={i.id} className={i.estEnAlerte ? "table-warning" : ""}>
                  <td>{i.articleReference}</td>
                  <td>{i.articleDesignation}</td>
                  <td>{i.articleVarianteReference ?? "-"}</td>
                  <td>{i.numeroLot ?? "-"}</td>
                  <td>{i.quantite}</td>
                  <td>{i.seuilMinimum}</td>
                </tr>
              ))}
              {items.length === 0 && (
                <tr>
                  <td colSpan={6} className="text-center text-muted py-3">
                    Aucun stock dans ce dépôt
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </>
      )}

      {onglet === "alertes" && (
        <table className="table table-hover bg-white">
          <thead>
            <tr>
              <th>Référence</th>
              <th>Article</th>
              <th>Dépôt</th>
              <th>Quantité</th>
              <th>Seuil</th>
            </tr>
          </thead>
          <tbody>
            {alertes.map((i) => (
              <tr key={i.id} className="table-warning">
                <td>{i.articleReference}</td>
                <td>{i.articleDesignation}</td>
                <td>{i.depotNom}</td>
                <td>{i.quantite}</td>
                <td>{i.seuilMinimum}</td>
              </tr>
            ))}
            {alertes.length === 0 && (
              <tr>
                <td colSpan={5} className="text-center text-muted py-3">
                  Aucune alerte
                </td>
              </tr>
            )}
          </tbody>
        </table>
      )}
    </div>
  );
}
