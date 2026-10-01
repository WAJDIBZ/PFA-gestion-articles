import axiosClient from "./axiosClient";

export const familleApi = {
  obtenirToutes: () => axiosClient.get("/familles").then((r) => r.data),
  obtenirParId: (id) => axiosClient.get(`/familles/${id}`).then((r) => r.data),
  creer: (dto) => axiosClient.post("/familles", dto).then((r) => r.data),
  modifier: (id, dto) => axiosClient.put(`/familles/${id}`, dto).then((r) => r.data),
  supprimer: (id) => axiosClient.delete(`/familles/${id}`),
};

export const marqueApi = {
  obtenirToutes: () => axiosClient.get("/marques").then((r) => r.data),
  creer: (dto) => axiosClient.post("/marques", dto).then((r) => r.data),
  modifier: (id, dto) => axiosClient.put(`/marques/${id}`, dto).then((r) => r.data),
  supprimer: (id) => axiosClient.delete(`/marques/${id}`),
};

export const uniteApi = {
  obtenirToutes: () => axiosClient.get("/unites").then((r) => r.data),
  creer: (dto) => axiosClient.post("/unites", dto).then((r) => r.data),
  modifier: (id, dto) => axiosClient.put(`/unites/${id}`, dto).then((r) => r.data),
  supprimer: (id) => axiosClient.delete(`/unites/${id}`),
};
