import axiosClient from "./axiosClient";

export const articleApi = {
  rechercher: (params) => axiosClient.get("/articles", { params }).then((r) => r.data),
  obtenirParId: (id) => axiosClient.get(`/articles/${id}`).then((r) => r.data),
  creer: (dto) => axiosClient.post("/articles", dto).then((r) => r.data),
  modifier: (id, dto) => axiosClient.put(`/articles/${id}`, dto).then((r) => r.data),
  supprimer: (id) => axiosClient.delete(`/articles/${id}`),
};

export const varianteApi = {
  obtenirAttributs: () => axiosClient.get("/variantes/attributs").then((r) => r.data),
  creerAttribut: (dto) => axiosClient.post("/variantes/attributs", dto).then((r) => r.data),
  creerValeur: (dto) => axiosClient.post("/variantes/attributs/valeurs", dto).then((r) => r.data),
  obtenirParArticle: (articleId) => axiosClient.get(`/variantes/par-article/${articleId}`).then((r) => r.data),
  creer: (dto) => axiosClient.post("/variantes", dto).then((r) => r.data),
  rechercherCombinaison: (dto) => axiosClient.post("/variantes/rechercher-combinaison", dto).then((r) => r.data),
};
