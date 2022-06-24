export const AccountStoreMeta = {
  getInitialState: () => ({
    account: {
      id: '',
      code: '',
      currency: '',
      name: '',
      userId: 0,
      amount: '',
    },
    accounts: [],
  }),
};
