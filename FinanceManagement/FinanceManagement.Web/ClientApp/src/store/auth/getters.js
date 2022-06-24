import { AuthStoreGetters } from 'src/store/auth/getters.meta';

export const getters = {
  [AuthStoreGetters.accessToken]: (state) => state.accessToken,
  [AuthStoreGetters.graphs]: (state) => state.graphs,
};
