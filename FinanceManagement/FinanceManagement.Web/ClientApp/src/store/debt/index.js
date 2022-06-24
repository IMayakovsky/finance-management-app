import { DebtStoreMeta } from 'src/store/debt/store.meta';
import { getters } from './getters';
import { mutations } from './mutations';
import { actions } from './actions';

const state = {
  ...DebtStoreMeta.getInitialState(),
};

export default {
  state,
  getters,
  mutations,
  actions,
};
