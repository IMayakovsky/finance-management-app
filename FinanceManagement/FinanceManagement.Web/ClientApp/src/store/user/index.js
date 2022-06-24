import { UserStoreMeta } from 'src/store/user/store.meta';
import { getters } from './getters';
import { mutations } from './mutations';
import { actions } from './actions';

const state = {
  ...UserStoreMeta.getInitialState(),
};

export default {
  state,
  getters,
  mutations,
  actions,
};
