import { CrudService } from 'src/api/CrudService';
import { CategoryServiceMeta } from 'src/api/CategoryService/service.meta';
import api from 'src/axios';

export const CategoryService = {
  ...CrudService,
  ...CategoryServiceMeta,
  fetchUserCategories() {
    return api.get(`${this.Routes.Crud()}userCategories`);
  },
  fetchDefaultCategories() {
    return api.get(`${this.Routes.Crud()}defaultCategories`);
  },
};
