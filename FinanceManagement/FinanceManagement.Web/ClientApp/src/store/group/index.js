import { GroupStoreMeta } from 'src/store/group/store.meta';
import { getters } from './getters';
import { mutations } from './mutations';
import { actions } from './actions';

const state = {
  ...GroupStoreMeta.getInitialState(),
};

export default {
  state,
  getters,
  mutations,
  actions,
};
