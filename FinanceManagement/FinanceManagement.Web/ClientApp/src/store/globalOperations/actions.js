import { GlobalOperationsStoreActions } from 'src/store/globalOperations/actions.meta';
import { GlobalOperationsStoreMutations } from 'src/store/globalOperations/mutations.meta';
import { GlobalOperationsStoreMeta } from 'src/store/globalOperations/store.meta';

export const actions = {
  [GlobalOperationsStoreActions.setGlobalLoading]: (
    context,
    status = GlobalOperationsStoreMeta.getInitialState().globalLoading,
  ) => {
    context.commit(GlobalOperationsStoreMutations.setGlobalLoading, status);
  },
};
