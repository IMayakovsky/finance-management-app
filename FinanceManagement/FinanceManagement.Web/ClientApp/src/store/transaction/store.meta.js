export const TransactionStoreMeta = {
  getInitialState: () => ({
    transaction: {},
    transactions: [],
    transactionTotalRowCount: 0,
  }),
  Types: {
    Group: 'Group',
    Account: 'Account',
  },
};
