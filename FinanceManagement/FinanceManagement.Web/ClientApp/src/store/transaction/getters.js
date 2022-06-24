import { TransactionStoreGetters } from 'src/store/transaction/getters.meta';

export const getters = {
  [TransactionStoreGetters.getTransaction]: (state) => state.transaction,
  [TransactionStoreGetters.getTransactions]: (state) => state.transactions,
  [TransactionStoreGetters.getTransactionTotalRowCount]: (state) => state.transactionTotalRowCount,
};
