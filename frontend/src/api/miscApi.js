import axiosClient from "./axiosClient";

export const fournisseurApi = {
  obtenirTous: () => axiosClient.get("/fournisseurs").then((r) => r.data),
  creer: (dto) => axiosClient.post("/fournisseurs", dto).then((r) => r.data),
  modifier: (id, dto) => axiosClient.put(`/fournisseurs/${id}`, dto).then((r) => r.data),
  supprimer: (id) => axiosClient.delete(`/fournisseurs/${id}`),
};

export const dashboardApi = {
  obtenir: () => axiosClient.get("/dashboard").then((r) => r.data),
};

export const notificationApi = {
  obtenirNonLues: () => axiosClient.get("/notifications").then((r) => r.data),
  marquerLue: (id) => axiosClient.put(`/notifications/${id}/lue`),
};
