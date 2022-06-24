import { CrudService } from 'src/api/CrudService';

export const SubscriptionServiceMeta = {
  Account: (accountId) => ({
    Routes: {
      Crud: CrudService.generateCrudRouteFunction(`/accounts/${accountId}/subscriptions/`),
    },
  }),
  Default: () => ({
    Routes: {
      Crud: CrudService.generateCrudRouteFunction('/subscriptions/'),
    },
  }),
};
