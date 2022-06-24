<template>
  <q-page class="q-px-md">
    <q-btn @click="handleAdd" class="q-ml-md">{{ $t("group.add") }}</q-btn>
    <div class="row">
      <GroupCard
        v-for="group in groupsGetter"
        :key="group.id"
        :group="group"
        @add:transaction="refreshPage"
        @edit="refreshPage"
        @delete="refreshPage"
      />
    </div>
    <ComponentLoading
      :is-loading="localLoading"
    />
  </q-page>
</template>

<script>
import AddGroupDialog from 'components/pages/groups/AddGroupDialog';
import GroupCard from 'components/pages/groups/GroupCard';
import groupStoreMixin from 'src/mixins/store/groupStoreMixin';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import ComponentLoading from 'components/global/ComponentLoading';

export default {
  name: 'Groups',
  components: { ComponentLoading, GroupCard },
  methods: {
    async handleAdd() {
      this.$q.dialog({
        component: AddGroupDialog,
      }).onOk(this.refreshPage);
    },
    async refreshPage() {
      await this.withGlobalLoadingAndErrorDialog(this.fetchGroupsAction);
    },
  },
  mixins: [groupStoreMixin, withLoadingAndErrorDialogMixin],
  async mounted() {
    await this.refreshPage();
  },
};
</script>

<style scoped>

</style>
