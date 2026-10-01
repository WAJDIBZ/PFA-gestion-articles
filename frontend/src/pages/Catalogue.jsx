import { useEffect, useState } from "react";
import { familleApi, marqueApi, uniteApi } from "../api/catalogueApi";
import { useAuth } from "../context/AuthContext";

function SectionFamilles() {
  const { aRole } = useAuth();
  const estAdmin = aRole("Administrateur");
  const [familles, setFamilles] = useState([]);
  const [nom, setNom] = useState("");
  const [description, setDescription] = useState("");

  const charger = () => familleApi.obtenirToutes().then(setFamilles);
  useEffect(() => {
    charger();
  }, []);

  const creer = async (e) => {
    e.preventDefault();
    if (!nom.trim()) return;
    await familleApi.creer({
      nom,
      description: description || null,
      familleParentId: null,
    });
    setNom("");
    setDescription("");
    charger();
  };

  const supprimer = async (id) => {
    if (!confirm("Supprimer cette famille ?")) return;
    await familleApi.supprimer(id);
    charger();
  };

  return (
    <div className="row g-3">
      <div className="col-md-5">
        {estAdmin && (
          <form onSubmit={creer} className="sg-card p-3 mb-3">
            <h6>Nouvelle famille</h6>
            <input
              className="form-control mb-2"
              placeholder="Nom"
              value={nom}
              onChange={(e) => setNom(e.target.value)}
              required
            />
            <input
              className="form-control mb-2"
              placeholder="Description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
            />
            <button className="btn btn-primary btn-sm">Ajouter</button>
          </form>
        )}
      </div>
      <div className="col-md-7">
        <div className="sg-card overflow-hidden">
          <table className="table table-modern table-sm mb-0">
            <thead>
              <tr>
                <th className="ps-3">Nom</th>
                <th>Description</th>
                <th>Articles</th>
                {estAdmin && <th className="pe-3"></th>}
              </tr>
            </thead>
            <tbody>
              {familles.map((f) => (
                <tr key={f.id}>
                  <td className="ps-3">{f.nom}</td>
                  <td>{f.description}</td>
                  <td>{f.nombreArticles}</td>
                  {estAdmin && (
                    <td className="pe-3">
                      <button
                        className="btn btn-sm btn-outline-danger"
                        onClick={() => supprimer(f.id)}
                      >
                        Suppr.
                      </button>
                    </td>
                  )}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}

function SectionMarques() {
  const { aRole } = useAuth();
  const estAdmin = aRole("Administrateur");
  const [marques, setMarques] = useState([]);
  const [nom, setNom] = useState("");

  const charger = () => marqueApi.obtenirToutes().then(setMarques);
  useEffect(() => {
    charger();
  }, []);

  const creer = async (e) => {
    e.preventDefault();
    if (!nom.trim()) return;
    await marqueApi.creer({ nom });
    setNom("");
    charger();
  };

  const supprimer = async (id) => {
    if (!confirm("Supprimer cette marque ?")) return;
    await marqueApi.supprimer(id);
    charger();
  };

  return (
    <div className="row g-3">
      <div className="col-md-5">
        {estAdmin && (
          <form onSubmit={creer} className="sg-card p-3 mb-3">
            <h6>Nouvelle marque</h6>
            <input
              className="form-control mb-2"
              placeholder="Nom"
              value={nom}
              onChange={(e) => setNom(e.target.value)}
              required
            />
            <button className="btn btn-primary btn-sm">Ajouter</button>
          </form>
        )}
      </div>
      <div className="col-md-7">
        <div className="sg-card overflow-hidden">
          <table className="table table-modern table-sm mb-0">
            <thead>
              <tr>
                <th className="ps-3">Nom</th>
                {estAdmin && <th className="pe-3"></th>}
              </tr>
            </thead>
            <tbody>
              {marques.map((m) => (
                <tr key={m.id}>
                  <td className="ps-3">{m.nom}</td>
                  {estAdmin && (
                    <td className="pe-3">
                      <button
                        className="btn btn-sm btn-outline-danger"
                        onClick={() => supprimer(m.id)}
                      >
                        Suppr.
                      </button>
                    </td>
                  )}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}

function SectionUnites() {
  const { aRole } = useAuth();
  const estAdmin = aRole("Administrateur");
  const [unites, setUnites] = useState([]);
  const [nom, setNom] = useState("");
  const [symbole, setSymbole] = useState("");

  const charger = () => uniteApi.obtenirToutes().then(setUnites);
  useEffect(() => {
    charger();
  }, []);

  const creer = async (e) => {
    e.preventDefault();
    if (!nom.trim() || !symbole.trim()) return;
    await uniteApi.creer({ nom, symbole });
    setNom("");
    setSymbole("");
    charger();
  };

  const supprimer = async (id) => {
    if (!confirm("Supprimer cette unité ?")) return;
    await uniteApi.supprimer(id);
    charger();
  };

  return (
    <div className="row g-3">
      <div className="col-md-5">
        {estAdmin && (
          <form onSubmit={creer} className="sg-card p-3 mb-3">
            <h6>Nouvelle unité</h6>
            <input
              className="form-control mb-2"
              placeholder="Nom (ex: Pièce)"
              value={nom}
              onChange={(e) => setNom(e.target.value)}
              required
            />
            <input
              className="form-control mb-2"
              placeholder="Symbole (ex: pcs)"
              value={symbole}
              onChange={(e) => setSymbole(e.target.value)}
              required
            />
            <button className="btn btn-primary btn-sm">Ajouter</button>
          </form>
        )}
      </div>
      <div className="col-md-7">
        <div className="sg-card overflow-hidden">
          <table className="table table-modern table-sm mb-0">
            <thead>
              <tr>
                <th className="ps-3">Nom</th>
                <th>Symbole</th>
                {estAdmin && <th className="pe-3"></th>}
              </tr>
            </thead>
            <tbody>
              {unites.map((u) => (
                <tr key={u.id}>
                  <td className="ps-3">{u.nom}</td>
                  <td>{u.symbole}</td>
                  {estAdmin && (
                    <td className="pe-3">
                      <button
                        className="btn btn-sm btn-outline-danger"
                        onClick={() => supprimer(u.id)}
                      >
                        Suppr.
                      </button>
                    </td>
                  )}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}

export default function Catalogue() {
  const [onglet, setOnglet] = useState("familles");

  return (
    <div>
      <div className="sg-page-header">
        <div>
          <h3>
            <span className="sg-icon-badge">
              <i className="bi bi-tags"></i>
            </span>
            Familles / Marques / Unités
          </h3>
          <div className="sg-subtitle">
            Organisez votre catalogue d'articles.
          </div>
        </div>
      </div>
      <ul className="nav nav-pills mb-3 gap-2">
        {[
          ["familles", "Familles"],
          ["marques", "Marques"],
          ["unites", "Unités"],
        ].map(([cle, label]) => (
          <li className="nav-item" key={cle}>
            <button
              className={`nav-link ${onglet === cle ? "active" : "bg-white border text-dark"}`}
              onClick={() => setOnglet(cle)}
            >
              {label}
            </button>
          </li>
        ))}
      </ul>
      {onglet === "familles" && <SectionFamilles />}
      {onglet === "marques" && <SectionMarques />}
      {onglet === "unites" && <SectionUnites />}
    </div>
  );
}
