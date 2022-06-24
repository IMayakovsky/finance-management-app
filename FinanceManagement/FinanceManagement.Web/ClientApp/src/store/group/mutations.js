import { GroupStoreMutations as GroupMutations } from 'src/store/group/mutations.meta';

export const mutations = {
  [GroupMutations.setGroup](state, newValue) {
    state.group = newValue;
  },
  [GroupMutations.setGroups](state, groups) {
    state.groups = groups;
  },
};
