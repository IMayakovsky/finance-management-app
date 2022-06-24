import { TransactionStoreMeta } from 'src/store/transaction/store.meta';
import { getters } from './getters';
import { mutations } from './mutations';
import { actions } from './actions';

const state = {
  ...TransactionStoreMeta.getInitialState(),
};

export default {
  state,
  getters,
  mutations,
  actions,
};
