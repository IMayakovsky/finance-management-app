import { GroupStoreActions } from 'src/store/group/actions.meta';
import { GroupStoreMutations } from 'src/store/group/mutations.meta';
import { GroupService } from 'src/api/GroupService';

export const actions = {
  async [GroupStoreActions.fetchGroups](context) {
    const { data: groups } = await GroupService.fetchMany();
    context.commit(GroupStoreMutations.setGroups, groups);
  },
  async [GroupStoreActions.fetchGroup](context, groupId) {
    const { data: group } = await GroupService.fetchOne(groupId);
    context.commit(GroupStoreMutations.setGroup, group);
  },
  async [GroupStoreActions.createGroup](context, { name, currency }) {
    const { data: group } = await GroupService.createOne({ groupName: name, currency });
    return group;
  },
  async [GroupStoreActions.deleteGroup](context, groupId) {
    await GroupService.deleteOne(groupId);
  },
  async [GroupStoreActions.updateGroup](context, group) {
    await GroupService.updateOne(group.id, group);
  },
};
