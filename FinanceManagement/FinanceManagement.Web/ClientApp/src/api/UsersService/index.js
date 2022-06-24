import api from 'src/axios';
import { UsersServiceMeta } from 'src/api/UsersService/service.meta';

export const UsersService = {
  fetchCurrentUser() {
    return api.get(UsersServiceMeta.ROUTES.GET_CURRENT_USER);
  },
};
