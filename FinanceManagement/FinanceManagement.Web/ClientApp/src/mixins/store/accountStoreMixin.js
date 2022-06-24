import { mapActions, mapGetters } from 'vuex';
import { AccountStoreActions } from 'src/store/account/actions.meta';
import { AccountStoreGetters } from 'src/store/account/getters.meta';

export default {
  computed: {
    ...mapGetters({
      accountsGetter: AccountStoreGetters.getAccounts,
      accountGetter: AccountStoreGetters.getAccount,
    }),
  },
  methods: {
    ...mapActions({
      fetchAccountsAction: AccountStoreActions.fetchAccounts,
      fetchAccountAction: AccountStoreActions.fetchAccount,
      deleteAccountAction: AccountStoreActions.deleteAccount,
      updateAccountAction: AccountStoreActions.updateAccount,
      createAccountAction: AccountStoreActions.createAccount,
    }),
  },
};
