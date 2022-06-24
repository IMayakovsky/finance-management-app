import { UserStoreGetters } from 'src/store/user/getters.meta';

export const getters = {
  [UserStoreGetters.user]: (state) => state.user,
};
