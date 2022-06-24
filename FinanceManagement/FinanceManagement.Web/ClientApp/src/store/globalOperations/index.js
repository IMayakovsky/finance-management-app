import { GlobalOperationsStoreMeta } from 'src/store/globalOperations/store.meta';
import { getters } from './getters';
import { mutations } from './mutations';
import { actions } from './actions';

const state = {
  ...GlobalOperationsStoreMeta.getInitialState(),
};

export default {
  state,
  getters,
  mutations,
  actions,
};
