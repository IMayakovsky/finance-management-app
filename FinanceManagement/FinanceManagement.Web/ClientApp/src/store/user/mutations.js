import { UserStoreMutations } from 'src/store/user/mutations.meta';

export const mutations = {
  [UserStoreMutations.setUser]: (state, user) => {
    state.user = user;
  },
};
