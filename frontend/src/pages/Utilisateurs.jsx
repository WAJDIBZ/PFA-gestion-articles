import { useEffect, useState } from "react";
import { authApi } from "../api/authApi";

const roles = ["Administrateur", "Magasinier", "Consultant"];
const videFormulaire = {
  nomComplet: "",
  email: "",
  motDePasse: "",
  role: "Magasinier",
};

export default function Utilisateurs() {
  const [utilisateurs, setUtilisateurs] = useState([]);
  const [afficherForm, setAfficherForm] = useState(false);
  const [formulaire, setFormulaire] = useState(videFormulaire);
  const [erreur, setErreur] = useState(null);

  const charger = () => authApi.obtenirUtilisateurs().then(setUtilisateurs);
  useEffect(() => {
    charger();
  }, []);

  const majChamp = (champ, valeur) =>
    setFormulaire((f) => ({ ...f, [champ]: valeur }));

  const creer = async (e) => {
    e.preventDefault();
    setErreur(null);
    try {
      await authApi.inscrire(formulaire);
      setFormulaire(videFormulaire);
      setAfficherForm(false);
      charger();
    } catch (err) {
      setErreur(
        err.response?.data?.message ||
          "Erreur lors de la création de l'utilisateur.",
      );
    }
  };

  const changerStatut = async (u, estActif) => {
    await authApi.modifierUtilisateur(u.id, {
      nomComplet: u.nomComplet,
      role: u.roles[0] ?? "Consultant",
      estActif,
    });
    charger();
  };

  return (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h3 className="mb-0">Utilisateurs</h3>
        <button
          className="btn btn-primary"
          onClick={() => setAfficherForm((v) => !v)}
        >
          <i className="bi bi-plus-lg me-1"></i>
          {afficherForm ? "Annuler" : "Nouvel utilisateur"}
        </button>
      </div>

      {afficherForm && (
        <form onSubmit={creer} className="card card-body mb-4 row g-2">
          {erreur && <div className="alert alert-danger py-2">{erreur}</div>}
          <div className="col-md-3">
            <label className="form-label">Nom complet</label>
            <input
              className="form-control"
              value={formulaire.nomComplet}
              onChange={(e) => majChamp("nomComplet", e.target.value)}
              required
            />
          </div>
          <div className="col-md-3">
            <label className="form-label">Email</label>
            <input
              type="email"
              className="form-control"
              value={formulaire.email}
              onChange={(e) => majChamp("email", e.target.value)}
              required
            />
          </div>
          <div className="col-md-3">
            <label className="form-label">Mot de passe</label>
            <input
              type="password"
              className="form-control"
              value={formulaire.motDePasse}
              onChange={(e) => majChamp("motDePasse", e.target.value)}
              required
            />
          </div>
          <div className="col-md-3">
            <label className="form-label">Rôle</label>
            <select
              className="form-select"
              value={formulaire.role}
              onChange={(e) => majChamp("role", e.target.value)}
            >
              {roles.map((r) => (
                <option key={r} value={r}>
                  {r}
                </option>
              ))}
            </select>
          </div>
          <div className="col-12">
            <button className="btn btn-success">Créer l'utilisateur</button>
          </div>
        </form>
      )}

      <table className="table table-hover bg-white">
        <thead>
          <tr>
            <th>Nom</th>
            <th>Email</th>
            <th>Rôles</th>
            <th>Statut</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {utilisateurs.map((u) => (
            <tr key={u.id}>
              <td>{u.nomComplet}</td>
              <td>{u.email}</td>
              <td>{u.roles.join(", ")}</td>
              <td>
                <span
                  className={`badge ${u.estActif ? "bg-success" : "bg-secondary"}`}
                >
                  {u.estActif ? "Actif" : "Inactif"}
                </span>
              </td>
              <td>
                <button
                  className={`btn btn-sm ${u.estActif ? "btn-outline-danger" : "btn-outline-success"}`}
                  onClick={() => changerStatut(u, !u.estActif)}
                >
                  {u.estActif ? "Désactiver" : "Activer"}
                </button>
              </td>
            </tr>
          ))}
          {utilisateurs.length === 0 && (
            <tr>
              <td colSpan={5} className="text-center text-muted py-3">
                Aucun utilisateur
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
