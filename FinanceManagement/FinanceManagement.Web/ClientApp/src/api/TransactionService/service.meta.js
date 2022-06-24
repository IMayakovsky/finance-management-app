import { CrudService } from 'src/api/CrudService';

export const TransactionServiceMeta = {
  Account: (accountId) => ({
    Routes: {
      Crud: CrudService.generateCrudRouteFunction(`/accounts/${accountId}/transactions/`),
    },
  }),
  Group: (groupId) => ({
    Routes: {
      Crud: CrudService.generateCrudRouteFunction(`/groups/${groupId}/transactions/`),
    },
  }),
};
