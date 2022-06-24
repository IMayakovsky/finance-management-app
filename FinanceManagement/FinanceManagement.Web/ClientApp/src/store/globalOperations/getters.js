import { GlobalOperationsStoreGetters } from 'src/store/globalOperations/getters.meta';

export const getters = {
  [GlobalOperationsStoreGetters.globalLoading]: (state) => state.globalLoading,
};
