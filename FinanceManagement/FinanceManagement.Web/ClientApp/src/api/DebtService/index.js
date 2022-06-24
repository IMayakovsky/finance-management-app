import { CrudService } from 'src/api/CrudService';
import { DebtServiceMeta } from 'src/api/DebtService/service.meta';

export const DebtService = {
  ...CrudService,
  ...DebtServiceMeta,
  async closeDebt({ debtId, accountId }) {
    await this.patchOne(debtId, '', null, { accountId });
  },
};
