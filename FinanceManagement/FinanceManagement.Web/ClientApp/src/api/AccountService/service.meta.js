import { CrudService } from 'src/api/CrudService';

export const AccountServiceMeta = {
  Routes: {
    Crud: CrudService.generateCrudRouteFunction('/accounts/'),
  },
};
