import { CrudService } from 'src/api/CrudService';
import { GroupServiceMeta } from 'src/api/GroupService/service.meta';

export const GroupService = {
  ...CrudService,
  ...GroupServiceMeta,
};
