import { NotificationStoreMeta } from 'src/store/notification/store.meta';
import { getters } from './getters';
import { mutations } from './mutations';
import { actions } from './actions';

const state = {
  ...NotificationStoreMeta.getInitialState(),
};

export default {
  state,
  getters,
  mutations,
  actions,
};
