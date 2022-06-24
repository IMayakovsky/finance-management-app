import { CrudService } from 'src/api/CrudService';

export const GroupServiceMeta = {
  Routes: {
    Crud: CrudService.generateCrudRouteFunction('/groups/'),
  },
};
