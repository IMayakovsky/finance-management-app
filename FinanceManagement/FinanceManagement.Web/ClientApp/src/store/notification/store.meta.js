export const NotificationStoreMeta = {
  getInitialState: () => ({
    notification: {
      id: 0,
      code: 'string',
      currency: 0,
      name: 'string',
      userId: 0,
      amount: 0,
    },
    notifications: [],
  }),
};
