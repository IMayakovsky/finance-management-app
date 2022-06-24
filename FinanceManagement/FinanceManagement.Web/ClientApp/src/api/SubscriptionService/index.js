import { CrudService } from 'src/api/CrudService';
import { SubscriptionServiceMeta } from 'src/api/SubscriptionService/service.meta';

export const SubscriptionService = {
  Account: (accountId) => ({
    ...CrudService,
    ...SubscriptionServiceMeta.Account(accountId),
  }),
  Default: () => ({
    ...CrudService,
    ...SubscriptionServiceMeta.Default(),
  }),
};
