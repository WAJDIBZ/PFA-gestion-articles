import { Link, Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

const fonctionnalites = [
  {
    icone: "bi-box-seam",
    titre: "Catalogue structuré",
    texte:
      "Organisez vos articles par familles, marques et unités, avec suivi simple, par lot ou par numéro de série.",
  },
  {
    icone: "bi-palette2",
    titre: "Variantes intelligentes",
    texte:
      "Gérez des combinaisons (taille, couleur, coupe...) et vérifiez instantanément leur disponibilité en stock.",
  },
  {
    icone: "bi-building",
    titre: "Multi-dépôts",
    texte:
      "Suivez le stock de chaque dépôt séparément et transférez des quantités entre sites en un clic.",
  },
  {
    icone: "bi-arrow-left-right",
    titre: "Traçabilité complète",
    texte:
      "Chaque entrée, sortie ou transfert est historisé : qui, quand, quel article, quel dépôt.",
  },
  {
    icone: "bi-bell",
    titre: "Alertes de stock",
    texte:
      "Définissez un seuil minimum par article et soyez notifié dès que le stock devient critique.",
  },
  {
    icone: "bi-people",
    titre: "Rôles & permissions",
    texte:
      "Administrateurs, magasiniers et consultants disposent chacun des accès adaptés à leur fonction.",
  },
];

export default function Landing() {
  const { utilisateur } = useAuth();
  if (utilisateur) return <Navigate to="/dashboard" replace />;

  return (
    <div style={{ background: "var(--sg-bg)" }}>
      <nav className="navbar navbar-expand-lg bg-white border-bottom sticky-top">
        <div className="container py-2">
          <Link
            to="/"
            className="navbar-brand d-flex align-items-center gap-2 fw-bold"
          >
            <span className="sg-icon-badge">
              <i className="bi bi-boxes"></i>
            </span>
            StockManagement
          </Link>
          <div className="ms-auto d-flex gap-2">
            <Link to="/login" className="btn btn-outline-primary">
              Se connecter
            </Link>
          </div>
        </div>
      </nav>

      <header
        className="text-white"
        style={{
          background:
            "linear-gradient(135deg,#171a2e 0%, #3730a3 55%, #4f46e5 100%)",
        }}
      >
        <div className="container py-5 py-lg-6">
          <div className="row align-items-center py-5">
            <div className="col-lg-7">
              <span className="badge bg-light text-dark bg-opacity-75 mb-3 px-3 py-2 rounded-pill">
                <i className="bi bi-stars me-1 text-primary"></i>Gestion de
                stock nouvelle génération
              </span>
              <h1 className="display-4 fw-bold mb-3">
                Pilotez vos articles, dépôts et mouvements depuis une seule
                interface
              </h1>
              <p className="fs-5 text-white-50 mb-4">
                Suivez vos quantités, vos variantes, vos lots et vos numéros de
                série dans plusieurs dépôts, avec une traçabilité complète de
                chaque opération et de ses responsables.
              </p>
              <div className="d-flex gap-3 flex-wrap">
                <Link
                  to="/login"
                  className="btn btn-light btn-lg fw-semibold px-4"
                >
                  Accéder à l'application{" "}
                  <i className="bi bi-arrow-right ms-1"></i>
                </Link>
              </div>
            </div>
            <div className="col-lg-5 d-none d-lg-block">
              <div
                className="sg-card p-4 text-dark"
                style={{ transform: "rotate(2deg)" }}
              >
                <div className="d-flex justify-content-between align-items-center mb-3">
                  <strong>Tableau de bord</strong>
                  <i className="bi bi-speedometer2 text-primary"></i>
                </div>
                <div className="row g-2">
                  {["Articles", "En alerte", "Dépôts", "Mouvements"].map(
                    (label, i) => (
                      <div className="col-6" key={label}>
                        <div className="border rounded-3 p-2 text-center">
                          <div className="fw-bold fs-5">
                            {[128, 6, 4, 532][i]}
                          </div>
                          <small className="text-muted">{label}</small>
                        </div>
                      </div>
                    ),
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>
      </header>

      <section className="container py-5">
        <div className="text-center mb-5">
          <h2 className="fw-bold">Tout ce qu'il faut pour gérer votre stock</h2>
          <p className="text-muted">
            Une seule plateforme pour vos articles, vos équipes et vos dépôts.
          </p>
        </div>
        <div className="row g-4">
          {fonctionnalites.map((f) => (
            <div className="col-md-6 col-lg-4" key={f.titre}>
              <div className="sg-card p-4 h-100">
                <span className="sg-icon-badge mb-3">
                  <i className={`bi ${f.icone}`}></i>
                </span>
                <h5 className="fw-semibold">{f.titre}</h5>
                <p className="text-muted mb-0">{f.texte}</p>
              </div>
            </div>
          ))}
        </div>
      </section>

      <section className="py-5" style={{ background: "var(--sg-gradient)" }}>
        <div className="container text-center text-white py-4">
          <h2 className="fw-bold mb-3">
            Prêt à reprendre le contrôle de votre stock ?
          </h2>
          <p className="mb-4 text-white-50">
            Connectez-vous pour accéder au tableau de bord et à vos dépôts.
          </p>
          <Link to="/login" className="btn btn-light btn-lg fw-semibold px-5">
            Se connecter
          </Link>
        </div>
      </section>

      <footer className="bg-white border-top py-4">
        <div className="container text-center text-muted small">
          © {new Date().getFullYear()} StockManagement — Gestion de stock,
          dépôts et mouvements.
        </div>
      </footer>
    </div>
  );
}
