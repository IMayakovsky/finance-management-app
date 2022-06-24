import { SubscriptionStoreMutations } from 'src/store/subscription/mutations.meta';

export const mutations = {
  [SubscriptionStoreMutations.setSubscription](state, newValue) {
    state.subscription = newValue;
  },
  [SubscriptionStoreMutations.setSubscriptions](state, newValue) {
    state.subscriptions = newValue;
  },
  [SubscriptionStoreMutations.setSubscriptionTotalRowCount](state, newValue) {
    state.subscriptionTotalRowCount = newValue;
  },
};
