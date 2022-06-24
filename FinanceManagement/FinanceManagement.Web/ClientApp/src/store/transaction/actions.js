import { TransactionStoreActions } from 'src/store/transaction/actions.meta';
import { TransactionStoreMutations } from 'src/store/transaction/mutations.meta';
import { TransactionService } from 'src/api/TransactionService';

export const actions = {
  async [TransactionStoreActions.fetchTransactions](context, {
    transactionType, parentId, page, pageSize,
  }) {
    const { data } = await TransactionService[transactionType](parentId).fetchMany(
      { currentPage: page, pageSize },
    );
    const { totalRowCount, transactions } = data;
    context.commit(TransactionStoreMutations.setTransactions, transactions);
    context.commit(TransactionStoreMutations.setTransactionTotalRowCount, totalRowCount);
  },
  async [TransactionStoreActions.fetchTransaction](context, {
    transactionType,
    parentId,
    transactionId,
  }) {
    const { data: transaction } = await TransactionService[transactionType](parentId).fetchOne(
      transactionId,
    );
    context.commit(TransactionStoreMutations.setTransaction, transaction);
  },
  async [TransactionStoreActions.createTransaction](context, {
    transactionType, parentId, transaction,
  }) {
    const {
      data: newTransaction,
    } = await TransactionService[transactionType](parentId).createOne(transaction);
    return newTransaction;
  },
  async [TransactionStoreActions.deleteTransaction](context, {
    transactionType,
    parentId, transactionId,
  }) {
    await TransactionService[transactionType](parentId).deleteOne(transactionId);
  },
  async [TransactionStoreActions.updateTransaction](
    context, { transactionType, parentId, transaction },
  ) {
    await TransactionService[transactionType](parentId).updateOne(
      transaction.id, transaction,
    );
  },
};
