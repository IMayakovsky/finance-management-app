import { GlobalOperationsStoreMutations } from 'src/store/globalOperations/mutations.meta';

export const mutations = {
  [GlobalOperationsStoreMutations.setGlobalLoading]: (state, value) => {
    state.globalLoading = value;
  },
};
