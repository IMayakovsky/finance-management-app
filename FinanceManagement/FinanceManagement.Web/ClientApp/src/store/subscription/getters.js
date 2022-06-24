import { SubscriptionStoreGetters } from 'src/store/subscription/getters.meta';

export const getters = {
  [SubscriptionStoreGetters.getSubscription]: (state) => state.subscription,
  [SubscriptionStoreGetters.getSubscriptions]: (state) => state.subscriptions,
  [SubscriptionStoreGetters.getSubscriptionTotalRowCount]: (state) => (
    state.subscriptionTotalRowCount
  ),
};
