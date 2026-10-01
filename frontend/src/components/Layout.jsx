import { NavLink, Outlet } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

const liensBase = [
  { to: "/dashboard", label: "Tableau de bord", icon: "bi-speedometer2" },
  { to: "/articles", label: "Articles", icon: "bi-box-seam" },
  { to: "/catalogue", label: "Familles / Marques / Unités", icon: "bi-tags" },
  { to: "/depots", label: "Dépôts", icon: "bi-building" },
  { to: "/stock", label: "Stock", icon: "bi-boxes" },
  { to: "/mouvements", label: "Mouvements", icon: "bi-arrow-left-right" },
  { to: "/fournisseurs", label: "Fournisseurs", icon: "bi-truck" },
];

const lienAdmin = {
  to: "/utilisateurs",
  label: "Utilisateurs",
  icon: "bi-people",
};

function initiales(nom) {
  if (!nom) return "?";
  return nom
    .split(" ")
    .map((p) => p[0])
    .slice(0, 2)
    .join("")
    .toUpperCase();
}

export default function Layout() {
  const { utilisateur, deconnecter, aRole } = useAuth();
  const liens = aRole("Administrateur") ? [...liensBase, lienAdmin] : liensBase;

  return (
    <div className="d-flex" style={{ minHeight: "100vh" }}>
      <nav
        className="text-white p-3 d-flex flex-column scrollbar-thin"
        style={{
          width: 264,
          flexShrink: 0,
          background:
            "linear-gradient(180deg, var(--sg-sidebar-from) 0%, var(--sg-sidebar-to) 100%)",
          position: "sticky",
          top: 0,
          height: "100vh",
          overflowY: "auto",
        }}
      >
        <div className="d-flex align-items-center gap-2 mb-4 px-1">
          <span className="sg-icon-badge">
            <i className="bi bi-boxes"></i>
          </span>
          <div>
            <div className="fw-bold">StockManagement</div>
            <div className="small text-white-50">Gestion de stock</div>
          </div>
        </div>

        <ul className="nav nav-pills flex-column gap-1 flex-grow-1">
          {liens.map((lien) => (
            <li className="nav-item" key={lien.to}>
              <NavLink
                to={lien.to}
                className={({ isActive }) =>
                  `nav-link text-white-50 d-flex align-items-center gap-2 rounded-3 ${isActive ? "active text-white" : ""}`
                }
                style={({ isActive }) => ({
                  background: isActive
                    ? "rgba(255,255,255,0.12)"
                    : "transparent",
                  fontWeight: isActive ? 600 : 400,
                })}
              >
                <i className={`bi ${lien.icon}`}></i>
                {lien.label}
              </NavLink>
            </li>
          ))}
        </ul>

        <div className="border-top border-white border-opacity-10 pt-3 mt-3">
          <div className="d-flex align-items-center gap-2 px-1">
            <span
              className="rounded-circle d-flex align-items-center justify-content-center fw-bold"
              style={{
                width: 38,
                height: 38,
                background: "var(--sg-gradient)",
                fontSize: "0.85rem",
              }}
            >
              {initiales(utilisateur?.nomComplet)}
            </span>
            <div className="flex-grow-1" style={{ minWidth: 0 }}>
              <div
                className="fw-semibold text-truncate"
                style={{ fontSize: "0.85rem" }}
              >
                {utilisateur?.nomComplet}
              </div>
              <div className="small text-white-50 text-truncate">
                {utilisateur?.roles?.join(", ")}
              </div>
            </div>
          </div>
        </div>
      </nav>

      <div className="flex-grow-1 d-flex flex-column" style={{ minWidth: 0 }}>
        <header
          className="d-flex justify-content-between align-items-center border-bottom px-4 py-3 bg-white sticky-top"
          style={{ top: 0 }}
        >
          <div>
            <span className="fw-semibold">
              Bienvenue, {utilisateur?.nomComplet?.split(" ")[0]}
            </span>
            <span className="text-muted ms-2 small d-none d-md-inline">
              {new Date().toLocaleDateString("fr-FR", {
                weekday: "long",
                day: "numeric",
                month: "long",
                year: "numeric",
              })}
            </span>
          </div>
          <div className="d-flex align-items-center gap-3">
            <span className="badge rounded-pill text-bg-light border">
              {utilisateur?.roles?.join(", ")}
            </span>
            <button
              className="btn btn-outline-danger btn-sm rounded-pill px-3"
              onClick={deconnecter}
            >
              <i className="bi bi-box-arrow-right me-1"></i>Déconnexion
            </button>
          </div>
        </header>
        <main
          className="p-4 flex-grow-1"
          style={{ background: "var(--sg-bg)" }}
        >
          <Outlet />
        </main>
      </div>
    </div>
  );
}
