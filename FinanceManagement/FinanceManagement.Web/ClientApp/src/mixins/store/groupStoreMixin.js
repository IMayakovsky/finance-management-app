import { mapActions, mapGetters } from 'vuex';
import { GroupStoreActions } from 'src/store/group/actions.meta';
import { GroupStoreGetters } from 'src/store/group/getters.meta';

export default {
  computed: {
    ...mapGetters({
      groupsGetter: GroupStoreGetters.getGroups,
      groupGetter: GroupStoreGetters.getGroup,
    }),
  },
  methods: {
    ...mapActions({
      fetchGroupsAction: GroupStoreActions.fetchGroups,
      fetchGroupAction: GroupStoreActions.fetchGroup,
      deleteGroupAction: GroupStoreActions.deleteGroup,
      updateGroupAction: GroupStoreActions.updateGroup,
      createGroupAction: GroupStoreActions.createGroup,
    }),
  },
};
