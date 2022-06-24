import { NotificationStoreGetters } from 'src/store/notification/getters.meta';

export const getters = {
  [NotificationStoreGetters.getNotification]: (state) => state.notification,
  [NotificationStoreGetters.getNotifications]: (state) => state.notifications,
  [NotificationStoreGetters.getNotificationTotalRowCount]: (state) => (
    state.notificationTotalRowCount
  ),
};
