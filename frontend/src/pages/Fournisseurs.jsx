import { useEffect, useState } from "react";
import { fournisseurApi } from "../api/miscApi";
import { useAuth } from "../context/AuthContext";

const videFormulaire = {
  nom: "",
  contact: "",
  email: "",
  telephone: "",
  adresse: "",
};

export default function Fournisseurs() {
  const { aRole } = useAuth();
  const estAdmin = aRole("Administrateur");
  const [fournisseurs, setFournisseurs] = useState([]);
  const [afficherForm, setAfficherForm] = useState(false);
  const [formulaire, setFormulaire] = useState(videFormulaire);

  const charger = () => fournisseurApi.obtenirTous().then(setFournisseurs);
  useEffect(() => {
    charger();
  }, []);

  const majChamp = (champ, valeur) =>
    setFormulaire((f) => ({ ...f, [champ]: valeur }));

  const creer = async (e) => {
    e.preventDefault();
    await fournisseurApi.creer(formulaire);
    setFormulaire(videFormulaire);
    setAfficherForm(false);
    charger();
  };

  const supprimer = async (id) => {
    if (!confirm("Supprimer ce fournisseur ?")) return;
    await fournisseurApi.supprimer(id);
    charger();
  };

  return (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h3 className="mb-0">Fournisseurs</h3>
        {estAdmin && (
          <button
            className="btn btn-primary"
            onClick={() => setAfficherForm((v) => !v)}
          >
            <i className="bi bi-plus-lg me-1"></i>
            {afficherForm ? "Annuler" : "Nouveau fournisseur"}
          </button>
        )}
      </div>

      {afficherForm && (
        <form onSubmit={creer} className="card card-body mb-4 row g-2">
          <div className="col-md-4">
            <label className="form-label">Nom</label>
            <input
              className="form-control"
              value={formulaire.nom}
              onChange={(e) => majChamp("nom", e.target.value)}
              required
            />
          </div>
          <div className="col-md-4">
            <label className="form-label">Contact</label>
            <input
              className="form-control"
              value={formulaire.contact}
              onChange={(e) => majChamp("contact", e.target.value)}
            />
          </div>
          <div className="col-md-4">
            <label className="form-label">Email</label>
            <input
              type="email"
              className="form-control"
              value={formulaire.email}
              onChange={(e) => majChamp("email", e.target.value)}
            />
          </div>
          <div className="col-md-4">
            <label className="form-label">Téléphone</label>
            <input
              className="form-control"
              value={formulaire.telephone}
              onChange={(e) => majChamp("telephone", e.target.value)}
            />
          </div>
          <div className="col-md-8">
            <label className="form-label">Adresse</label>
            <input
              className="form-control"
              value={formulaire.adresse}
              onChange={(e) => majChamp("adresse", e.target.value)}
            />
          </div>
          <div className="col-12">
            <button className="btn btn-success">Enregistrer</button>
          </div>
        </form>
      )}

      <table className="table table-hover bg-white">
        <thead>
          <tr>
            <th>Nom</th>
            <th>Contact</th>
            <th>Email</th>
            <th>Téléphone</th>
            <th>Adresse</th>
            {estAdmin && <th></th>}
          </tr>
        </thead>
        <tbody>
          {fournisseurs.map((f) => (
            <tr key={f.id}>
              <td>{f.nom}</td>
              <td>{f.contact}</td>
              <td>{f.email}</td>
              <td>{f.telephone}</td>
              <td>{f.adresse}</td>
              {estAdmin && (
                <td>
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
          {fournisseurs.length === 0 && (
            <tr>
              <td colSpan={6} className="text-center text-muted py-3">
                Aucun fournisseur
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
