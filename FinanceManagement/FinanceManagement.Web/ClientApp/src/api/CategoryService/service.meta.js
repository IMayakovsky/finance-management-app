import { CrudService } from 'src/api/CrudService';

export const CategoryServiceMeta = {
  Routes: {
    Crud: CrudService.generateCrudRouteFunction('/categories/'),
  },
};
