import { AccountStoreMutations as AccountMutations } from 'src/store/account/mutations.meta';

export const mutations = {
  [AccountMutations.setAccount](state, newValue) {
    state.account = newValue;
  },
  [AccountMutations.setAccounts](state, accounts) {
    state.accounts = accounts;
  },
};
