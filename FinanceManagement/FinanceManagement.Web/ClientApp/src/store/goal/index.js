import { GoalStoreMeta } from 'src/store/goal/store.meta';
import { getters } from './getters';
import { mutations } from './mutations';
import { actions } from './actions';

const state = {
  ...GoalStoreMeta.getInitialState(),
};

export default {
  state,
  getters,
  mutations,
  actions,
};
