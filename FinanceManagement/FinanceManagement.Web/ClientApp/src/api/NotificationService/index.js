import { CrudService } from 'src/api/CrudService';
import { NotificationServiceMeta } from 'src/api/NotificationService/service.meta';
import api from 'src/axios';

export const NotificationService = {
  ...CrudService,
  ...NotificationServiceMeta,
  fetchMany(body, params = {}) {
    return api.post(`${this.Routes.Crud()}getNotifications`, body, params);
  },
  readNotification({ notificationId }) {
    return this.patchOne(notificationId, 'read');
  },
};
