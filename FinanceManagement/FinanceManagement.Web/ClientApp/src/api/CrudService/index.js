import api from 'src/axios';

export const CrudService = {
  generateCrudRouteFunction: (baseUrl) => (id, patchAttribute) => `${baseUrl}${id || ''}${(patchAttribute && `/${patchAttribute}`) || ''}`,
  get Routes() {
    return { Crud: this.generateCrudRouteFunction('') };
  },
  fetchOne(id, params = {}) {
    return api.get(this.Routes.Crud(id), { params });
  },
  fetchMany(params = {}) {
    return api.get(this.Routes.Crud(), { params });
  },
  deleteOne(id, body, params = {}) {
    return api.delete(this.Routes.Crud(id), { params });
  },
  createOne(body, params = {}) {
    return api.post(this.Routes.Crud(), body, { params });
  },
  updateOne(id, body, params = {}) {
    return api.put(this.Routes.Crud(id), body, { params });
  },
  patchOne(id, attribute, body = null, params = {}) {
    return api.patch(this.Routes.Crud(id, attribute), body, { params });
  },
};
