import { AccountStoreGetters } from 'src/store/account/getters.meta';

export const getters = {
  [AccountStoreGetters.getAccount]: (state) => state.account,
  [AccountStoreGetters.getAccounts]: (state) => state.accounts,
};
