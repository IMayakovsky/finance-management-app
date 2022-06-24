import { GroupStoreGetters } from 'src/store/group/getters.meta';

export const getters = {
  [GroupStoreGetters.getGroup]: (state) => state.group,
  [GroupStoreGetters.getGroups]: (state) => state.groups,
};
