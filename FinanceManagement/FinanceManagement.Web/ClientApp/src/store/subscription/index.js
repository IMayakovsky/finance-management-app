import { SubscriptionStoreMeta } from 'src/store/subscription/store.meta';
import { getters } from './getters';
import { mutations } from './mutations';
import { actions } from './actions';

const state = {
  ...SubscriptionStoreMeta.getInitialState(),
};

export default {
  state,
  getters,
  mutations,
  actions,
};
