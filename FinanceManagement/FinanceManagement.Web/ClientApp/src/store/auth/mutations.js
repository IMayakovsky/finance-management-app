import { AuthStoreMeta } from 'src/store/auth/store.meta';
import { AuthStoreMutations } from 'src/store/auth/mutations.meta';

export const mutations = {
  [AuthStoreMutations.setAccessToken]: (
    state,
    accessToken = AuthStoreMeta.getInitialState().accessToken,
  ) => {
    state.accessToken = accessToken;
  },
};
