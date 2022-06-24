import { CrudService } from 'src/api/CrudService';
import { AccountServiceMeta } from 'src/api/AccountService/service.meta';

export const AccountService = {
  ...CrudService,
  ...AccountServiceMeta,
};
