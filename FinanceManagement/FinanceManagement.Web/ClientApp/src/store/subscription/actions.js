import { SubscriptionStoreActions } from 'src/store/subscription/actions.meta';
import { SubscriptionStoreMutations } from 'src/store/subscription/mutations.meta';
import { SubscriptionService } from 'src/api/SubscriptionService';

export const actions = {
  async [SubscriptionStoreActions.fetchAllSubscriptions](context, {
    page, pageSize,
  }) {
    const { data } = await SubscriptionService.Default().fetchMany(
      { currentPage: page, pageSize },
    );
    context.commit(SubscriptionStoreMutations.setSubscriptions, data);
  },
  async [SubscriptionStoreActions.fetchSubscriptions](context, {
    accountId, page, pageSize,
  }) {
    const { data } = await SubscriptionService.Account(accountId).fetchMany(
      { currentPage: page, pageSize },
    );
    const { totalRowCount, subscriptions } = data;
    context.commit(SubscriptionStoreMutations.setSubscriptions, subscriptions);
    context.commit(SubscriptionStoreMutations.setSubscriptionTotalRowCount, totalRowCount);
  },
  async [SubscriptionStoreActions.fetchSubscription](context, {
    accountId,
    subscriptionId,
  }) {
    const { data: subscription } = await SubscriptionService.Account(accountId).fetchOne(
      subscriptionId,
    );
    context.commit(SubscriptionStoreMutations.setSubscription, subscription);
  },
  async [SubscriptionStoreActions.createSubscription](context, {
    name, amount, accountId, dateFrom, dateTo,
  }) {
    const { data: subscription } = await SubscriptionService.Account(accountId).createOne({
      name, amount, dateFrom, dateTo, accountId,
    });
    return subscription;
  },
  async [SubscriptionStoreActions.deleteSubscription](context, {

    accountId, subscriptionId,
  }) {
    await SubscriptionService.Account(accountId).deleteOne(subscriptionId);
  },
  async [SubscriptionStoreActions.updateSubscription](
    context, subscription,
  ) {
    await SubscriptionService.Account(subscription.accountId).updateOne(
      subscription.id, subscription,
    );
  },
};
