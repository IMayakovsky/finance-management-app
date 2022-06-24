import { CrudService } from 'src/api/CrudService';

export const DebtServiceMeta = {
  Routes: {
    Crud: CrudService.generateCrudRouteFunction('/debts/'),
  },
};
