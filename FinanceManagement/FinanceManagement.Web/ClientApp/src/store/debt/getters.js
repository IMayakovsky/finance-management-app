import { DebtStoreGetters } from 'src/store/debt/getters.meta';

export const getters = {
  [DebtStoreGetters.getDebts]: (state) => state.debts,
};
