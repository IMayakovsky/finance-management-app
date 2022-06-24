import { DebtStoreActions } from 'src/store/debt/actions.meta';
import { DebtStoreMutations } from 'src/store/debt/mutations.meta';
import { DebtService } from 'src/api/DebtService';

export const actions = {
  async [DebtStoreActions.fetchDebts](context) {
    const { data: debts } = await DebtService.fetchMany();
    context.commit(DebtStoreMutations.setDebts, debts);
  },
  async [DebtStoreActions.createDebt](context, {
    name, dueTo, amount, note,
  }) {
    const { data: debt } = await DebtService.createOne({
      name, dueTo, amount, note,
    });
    return debt;
  },
  async [DebtStoreActions.deleteDebt](context, { debtId }) {
    await DebtService.deleteOne(debtId);
  },
  async [DebtStoreActions.updateDebt](context, debt) {
    await DebtService.updateOne(debt.id, debt);
  },
  async [DebtStoreActions.closeDebt](context, { debtId, accountId }) {
    await DebtService.closeDebt({ debtId, accountId });
  },
};
