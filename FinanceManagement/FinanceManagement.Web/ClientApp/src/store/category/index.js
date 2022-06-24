import { CategoryStoreMeta } from 'src/store/category/store.meta';
import { getters } from './getters';
import { mutations } from './mutations';
import { actions } from './actions';

const state = {
  ...CategoryStoreMeta.getInitialState(),
};

export default {
  state,
  getters,
  mutations,
  actions,
};
