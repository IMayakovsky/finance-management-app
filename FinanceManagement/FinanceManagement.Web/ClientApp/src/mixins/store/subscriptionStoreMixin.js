import { mapActions, mapGetters } from 'vuex';
import { SubscriptionStoreActions } from 'src/store/subscription/actions.meta';
import { SubscriptionStoreGetters } from 'src/store/subscription/getters.meta';

export default {
  computed: {
    ...mapGetters({
      subscriptionsGetter: SubscriptionStoreGetters.getSubscriptions,
      subscriptionGetter: SubscriptionStoreGetters.getSubscription,
      subscriptionTotalRowCountGetter: SubscriptionStoreGetters.getSubscriptionTotalRowCount,
    }),
  },
  methods: {
    ...mapActions({
      fetchSubscriptionsAction: SubscriptionStoreActions.fetchSubscriptions,
      fetchAllSubscriptionsAction: SubscriptionStoreActions.fetchAllSubscriptions,
      fetchSubscriptionAction: SubscriptionStoreActions.fetchSubscription,
      deleteSubscriptionAction: SubscriptionStoreActions.deleteSubscription,
      updateSubscriptionAction: SubscriptionStoreActions.updateSubscription,
      createSubscriptionAction: SubscriptionStoreActions.createSubscription,
    }),
  },
};
