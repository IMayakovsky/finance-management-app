import { AuthStoreMeta } from 'src/store/auth/store.meta';
import { getters } from './getters';
import { mutations } from './mutations';
import { actions } from './actions';

const state = {
  ...AuthStoreMeta.getInitialState(),
};

export default {
  state,
  getters,
  mutations,
  actions,
};
