import { NotificationStoreMutations as NotificationMutations } from 'src/store/notification/mutations.meta';

export const mutations = {
  [NotificationMutations.setNotification](state, newValue) {
    state.notification = newValue;
  },
  [NotificationMutations.setNotifications](state, notifications) {
    state.notifications = notifications;
  },
  [NotificationMutations.setNotificationTotalRowCount](state, newValue) {
    state.notificationTotalRowCount = newValue;
  },
};
