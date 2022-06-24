import { CrudService } from 'src/api/CrudService';

export const NotificationServiceMeta = {
  Routes: {
    Crud: CrudService.generateCrudRouteFunction('/notifications/'),
  },
};
