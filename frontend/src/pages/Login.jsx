import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

const comptesDemo = [
  {
    role: "Administrateur",
    email: "admin@stockmanagement.local",
    icone: "bi-shield-lock",
    couleur: "linear-gradient(135deg,#4f46e5,#818cf8)",
  },
  {
    role: "Magasinier",
    email: "magasinier@stockmanagement.local",
    icone: "bi-box-seam",
    couleur: "linear-gradient(135deg,#0891b2,#06b6d4)",
  },
  {
    role: "Consultant",
    email: "consultant@stockmanagement.local",
    icone: "bi-eye",
    couleur: "linear-gradient(135deg,#334155,#64748b)",
  },
];

const statsBrand = [
  { valeur: "Multi-dépôts", label: "Transferts en temps réel" },
  { valeur: "100%", label: "Opérations tracées" },
  { valeur: "3 rôles", label: "Permissions dédiées" },
];

export default function Login() {
  const { connecter } = useAuth();
  const navigate = useNavigate();
  const [email, setEmail] = useState("admin@stockmanagement.local");
  const [motDePasse, setMotDePasse] = useState("");
  const [erreur, setErreur] = useState(null);
  const [chargement, setChargement] = useState(false);

  const soumettre = async (e) => {
    e.preventDefault();
    setErreur(null);
    setChargement(true);
    try {
      await connecter(email, motDePasse);
      navigate("/dashboard");
    } catch {
      setErreur("Identifiants invalides.");
    } finally {
      setChargement(false);
    }
  };

  return (
    <div className="d-flex" style={{ minHeight: "100vh" }}>
      <div
        className="sg-login-brand d-none d-lg-flex flex-column justify-content-between text-white p-5"
        style={{
          width: "46%",
          background:
            "linear-gradient(160deg,#10112a 0%, #2f2a7a 55%, #4f46e5 100%)",
        }}
      >
        <div className="sg-login-grid"></div>
        <div
          className="sg-login-orb"
          style={{
            width: 320,
            height: 320,
            background: "#818cf8",
            top: -80,
            right: -100,
          }}
        ></div>
        <div
          className="sg-login-orb"
          style={{
            width: 260,
            height: 260,
            background: "#06b6d4",
            bottom: -60,
            left: -60,
          }}
        ></div>

        <Link
          to="/"
          className="d-flex align-items-center gap-2 text-white fw-bold fs-5"
        >
          <span
            className="sg-icon-badge"
            style={{ boxShadow: "0 0 0 6px rgba(255,255,255,0.08)" }}
          >
            <i className="bi bi-boxes"></i>
          </span>
          StockManagement
        </Link>

        <div>
          <span
            className="badge rounded-pill px-3 py-2 mb-3"
            style={{ background: "rgba(255,255,255,0.1)" }}
          >
            <i className="bi bi-stars me-1"></i>Plateforme de gestion de stock
          </span>
          <h1
            className="fw-bold mb-3"
            style={{ fontSize: "2.4rem", lineHeight: 1.2 }}
          >
            Gérez votre stock avec précision et sérénité.
          </h1>
          <p className="text-white-50 fs-5 mb-4">
            Articles, variantes, dépôts, mouvements et utilisateurs : toute
            votre chaîne logistique, centralisée et tracée.
          </p>
          <div className="row g-2">
            {statsBrand.map((s) => (
              <div className="col-4" key={s.label}>
                <div className="sg-login-stat">
                  <div className="value">{s.valeur}</div>
                  <div className="label">{s.label}</div>
                </div>
              </div>
            ))}
          </div>
        </div>

        <div className="small text-white-50">
          © {new Date().getFullYear()} StockManagement — Tous droits réservés
        </div>
      </div>

      <div
        className="flex-grow-1 d-flex align-items-center justify-content-center p-3 p-md-4"
        style={{ background: "var(--sg-bg)" }}
      >
        <div
          className="sg-login-card p-4 p-md-5"
          style={{ width: "100%", maxWidth: 440 }}
        >
          <div className="d-lg-none d-flex align-items-center gap-2 mb-4">
            <span className="sg-icon-badge">
              <i className="bi bi-boxes"></i>
            </span>
            <span className="fw-bold fs-5">StockManagement</span>
          </div>

          <h3 className="fw-bold mb-1">Bon retour parmi nous</h3>
          <p className="text-muted mb-4">
            Connectez-vous pour accéder à votre espace de gestion.
          </p>

          {erreur && (
            <div className="alert alert-danger py-2 d-flex align-items-center gap-2">
              <i className="bi bi-exclamation-circle"></i>
              {erreur}
            </div>
          )}

          <form onSubmit={soumettre}>
            <div className="mb-3">
              <label
                className="form-label fw-semibold small text-uppercase text-muted"
                style={{ letterSpacing: "0.04em" }}
              >
                Adresse email
              </label>
              <div className="input-group sg-input-group">
                <span className="input-group-text">
                  <i className="bi bi-envelope"></i>
                </span>
                <input
                  type="email"
                  className="form-control py-2"
                  placeholder="vous@entreprise.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                />
              </div>
            </div>
            <div className="mb-4">
              <label
                className="form-label fw-semibold small text-uppercase text-muted"
                style={{ letterSpacing: "0.04em" }}
              >
                Mot de passe
              </label>
              <div className="input-group sg-input-group">
                <span className="input-group-text">
                  <i className="bi bi-lock"></i>
                </span>
                <input
                  type="password"
                  className="form-control py-2"
                  placeholder="••••••••"
                  value={motDePasse}
                  onChange={(e) => setMotDePasse(e.target.value)}
                  required
                />
              </div>
            </div>
            <button
              type="submit"
              className="btn sg-btn-gradient w-100 py-2 fs-6"
              disabled={chargement}
            >
              {chargement ? (
                <>
                  <span className="spinner-border spinner-border-sm me-2"></span>
                  Connexion...
                </>
              ) : (
                <>
                  Se connecter <i className="bi bi-arrow-right ms-1"></i>
                </>
              )}
            </button>
          </form>

          <div className="sg-divider">Comptes de démonstration</div>

          <div className="d-flex flex-column gap-2">
            {comptesDemo.map((c) => (
              <button
                key={c.email}
                type="button"
                className="sg-demo-account"
                onClick={() => {
                  setEmail(c.email);
                  setMotDePasse("Demo@123");
                }}
              >
                <span
                  className="sg-demo-icon"
                  style={{ background: c.couleur }}
                >
                  <i className={`bi ${c.icone}`}></i>
                </span>
                <span>
                  <span
                    className="d-block fw-semibold"
                    style={{ fontSize: "0.88rem" }}
                  >
                    {c.role}
                  </span>
                  <span
                    className="d-block text-muted"
                    style={{ fontSize: "0.78rem" }}
                  >
                    {c.email}
                  </span>
                </span>
              </button>
            ))}
          </div>
          <p
            className="text-center text-muted mt-3 mb-0"
            style={{ fontSize: "0.78rem" }}
          >
            Mot de passe pour tous les comptes : <code>Demo@123</code>
          </p>

          <div className="text-center mt-4">
            <Link to="/" className="text-decoration-none small text-muted">
              <i className="bi bi-arrow-left me-1"></i>Retour à l'accueil
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
}
