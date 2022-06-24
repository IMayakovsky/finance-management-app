import { DebtStoreMutations as DebtMutations } from 'src/store/debt/mutations.meta';

export const mutations = {
  [DebtMutations.setDebts](state, debts) {
    state.debts = debts;
  },
};
