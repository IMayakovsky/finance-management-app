import { mapActions, mapGetters } from 'vuex';
import { DebtStoreActions } from 'src/store/debt/actions.meta';
import { DebtStoreGetters } from 'src/store/debt/getters.meta';

export default {
  computed: {
    ...mapGetters({
      debtsGetter: DebtStoreGetters.getDebts,
    }),
  },
  methods: {
    ...mapActions({
      fetchDebtsAction: DebtStoreActions.fetchDebts,
      deleteDebtAction: DebtStoreActions.deleteDebt,
      updateDebtAction: DebtStoreActions.updateDebt,
      createDebtAction: DebtStoreActions.createDebt,
      closeDebtAction: DebtStoreActions.closeDebt,
    }),
  },
};
