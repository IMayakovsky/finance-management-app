import { CrudService } from 'src/api/CrudService';
import { TransactionServiceMeta } from 'src/api/TransactionService/service.meta';

export const TransactionService = {
  Account: (accountId) => ({
    ...CrudService,
    ...TransactionServiceMeta.Account(accountId),
  }),
  Group: (accountId) => ({
    ...CrudService,
    ...TransactionServiceMeta.Group(accountId),
  }),
};
