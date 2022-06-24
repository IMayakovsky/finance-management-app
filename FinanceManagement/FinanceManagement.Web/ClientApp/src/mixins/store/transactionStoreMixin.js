import { mapActions, mapGetters } from 'vuex';
import { TransactionStoreActions } from 'src/store/transaction/actions.meta';
import { TransactionStoreGetters } from 'src/store/transaction/getters.meta';

export default {
  computed: {
    ...mapGetters({
      transactionsGetter: TransactionStoreGetters.getTransactions,
      transactionGetter: TransactionStoreGetters.getTransaction,
      transactionTotalRowCountGetter: TransactionStoreGetters.getTransactionTotalRowCount,
    }),
  },
  methods: {
    ...mapActions({
      fetchTransactionsAction: TransactionStoreActions.fetchTransactions,
      fetchTransactionAction: TransactionStoreActions.fetchTransaction,
      deleteTransactionAction: TransactionStoreActions.deleteTransaction,
      updateTransactionAction: TransactionStoreActions.updateTransaction,
      createTransactionAction: TransactionStoreActions.createTransaction,
    }),
  },
};
