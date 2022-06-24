import { mapActions, mapGetters } from 'vuex';
import { NotificationStoreActions } from 'src/store/notification/actions.meta';
import { NotificationStoreGetters } from 'src/store/notification/getters.meta';

export default {
  computed: {
    ...mapGetters({
      notificationsGetter: NotificationStoreGetters.getNotifications,
      notificationGetter: NotificationStoreGetters.getNotification,
    }),
  },
  methods: {
    ...mapActions({
      fetchNotificationsAction: NotificationStoreActions.fetchNotifications,
      fetchUserNotificationsAction: NotificationStoreActions.fetchUserNotifications,
      fetchNotificationAction: NotificationStoreActions.fetchNotification,
      deleteNotificationAction: NotificationStoreActions.deleteNotification,
      updateNotificationAction: NotificationStoreActions.updateNotification,
      createNotificationAction: NotificationStoreActions.createNotification,
    }),
  },
};
