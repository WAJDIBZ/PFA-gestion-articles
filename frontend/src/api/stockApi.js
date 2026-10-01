import axiosClient from "./axiosClient";

export const depotApi = {
  obtenirTous: () => axiosClient.get("/depots").then((r) => r.data),
  obtenirParId: (id) => axiosClient.get(`/depots/${id}`).then((r) => r.data),
  creer: (dto) => axiosClient.post("/depots", dto).then((r) => r.data),
  modifier: (id, dto) => axiosClient.put(`/depots/${id}`, dto).then((r) => r.data),
  supprimer: (id) => axiosClient.delete(`/depots/${id}`),
};

export const stockApi = {
  obtenirParDepot: (depotId) => axiosClient.get(`/stock/depot/${depotId}`).then((r) => r.data),
  obtenirParArticle: (articleId) => axiosClient.get(`/stock/article/${articleId}`).then((r) => r.data),
  obtenirAlertes: () => axiosClient.get("/stock/alertes").then((r) => r.data),
};

export const mouvementApi = {
  rechercher: (params) => axiosClient.get("/mouvements", { params }).then((r) => r.data),
  entree: (dto) => axiosClient.post("/mouvements/entree", dto).then((r) => r.data),
  sortie: (dto) => axiosClient.post("/mouvements/sortie", dto).then((r) => r.data),
  transfert: (dto) => axiosClient.post("/mouvements/transfert", dto).then((r) => r.data),
};
