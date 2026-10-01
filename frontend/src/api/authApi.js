import axiosClient from "./axiosClient";

export const authApi = {
  connecter: (dto) => axiosClient.post("/auth/login", dto).then((r) => r.data),
  inscrire: (dto) => axiosClient.post("/auth/register", dto).then((r) => r.data),
  obtenirUtilisateurs: () => axiosClient.get("/auth/utilisateurs").then((r) => r.data),
  modifierUtilisateur: (id, dto) => axiosClient.put(`/auth/utilisateurs/${id}`, dto).then((r) => r.data),
  supprimerUtilisateur: (id) => axiosClient.delete(`/auth/utilisateurs/${id}`),
};
