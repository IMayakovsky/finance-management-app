import { NotificationStoreActions } from 'src/store/notification/actions.meta';
import { NotificationStoreMutations } from 'src/store/notification/mutations.meta';
import { NotificationService } from 'src/api/NotificationService';

export const actions = {
  async [NotificationStoreActions.fetchNotifications](context, {
    page, pageSize,
  }) {
    const { data } = await NotificationService.fetchMany({
      currentPage: page, pageSize,
    });
    const { totalRowCount, notifications } = data;
    context.commit(NotificationStoreMutations.setNotifications, notifications);
    context.commit(NotificationStoreMutations.setNotificationTotalRowCount, totalRowCount);
  },
  async [NotificationStoreActions.deleteNotification](context, notificationId) {
    await NotificationService.deleteOne(notificationId);
  },
  async [NotificationStoreActions.updateNotification](context, notificationId) {
    await NotificationService.readNotification({ notificationId });
  },
};
