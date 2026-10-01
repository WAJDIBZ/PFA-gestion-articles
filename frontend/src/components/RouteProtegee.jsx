import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export function RouteProtegee({ rolesAutorises }) {
  const { utilisateur } = useAuth();

  if (!utilisateur) return <Navigate to="/login" replace />;

  if (
    rolesAutorises &&
    !rolesAutorises.some((r) => utilisateur.roles?.includes(r))
  ) {
    return <Navigate to="/dashboard" replace />;
  }

  return <Outlet />;
}
