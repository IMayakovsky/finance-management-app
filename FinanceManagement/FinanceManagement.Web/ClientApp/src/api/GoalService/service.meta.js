import { CrudService } from 'src/api/CrudService';

export const GoalServiceMeta = {
  Routes: {
    Crud: CrudService.generateCrudRouteFunction('/goals/'),
  },
};
