import { AccountStoreMeta } from 'src/store/account/store.meta';
import { getters } from './getters';
import { mutations } from './mutations';
import { actions } from './actions';

const state = {
  ...AccountStoreMeta.getInitialState(),
};

export default {
  state,
  getters,
  mutations,
  actions,
};
