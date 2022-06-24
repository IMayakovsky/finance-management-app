import { TransactionStoreMutations } from 'src/store/transaction/mutations.meta';
import { cutTimeFromDateString } from 'src/utils/common';

export const mutations = {
  [TransactionStoreMutations.setTransaction](state, newValue) {
    state.transaction = newValue;
  },
  [TransactionStoreMutations.setTransactions](state, transactions) {
    state.transactions = transactions.map((t) => ({
      ...t,
      date: cutTimeFromDateString(t.date),
    }));
  },
  [TransactionStoreMutations.setTransactionTotalRowCount](state, newValue) {
    state.transactionTotalRowCount = newValue;
  },
};
