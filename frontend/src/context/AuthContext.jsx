import { createContext, useContext, useState } from "react";
import { authApi } from "../api/authApi";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [utilisateur, setUtilisateur] = useState(() => {
    const stocke = localStorage.getItem("utilisateur");
    return stocke ? JSON.parse(stocke) : null;
  });

  const connecter = async (email, motDePasse) => {
    const reponse = await authApi.connecter({ email, motDePasse });
    localStorage.setItem("token", reponse.token);
    localStorage.setItem("utilisateur", JSON.stringify(reponse.utilisateur));
    setUtilisateur(reponse.utilisateur);
    return reponse.utilisateur;
  };

  const deconnecter = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("utilisateur");
    setUtilisateur(null);
  };

  const aRole = (role) => utilisateur?.roles?.includes(role) ?? false;

  return (
    <AuthContext.Provider
      value={{ utilisateur, connecter, deconnecter, aRole }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const contexte = useContext(AuthContext);
  if (!contexte) throw new Error("useAuth doit être utilisé dans AuthProvider");
  return contexte;
}
