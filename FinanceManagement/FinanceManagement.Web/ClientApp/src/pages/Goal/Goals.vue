<template>
  <q-page>
    <q-btn @click="handleAdd" class="q-ml-md">{{ $t("goal.add") }}</q-btn>
    <GoalsTable
      :key="{ goalRows, paginationForTable }"
      :goals="goalRows"
      ref="goalTable"
      :on-delete="refreshPage"
      :on-edit="refreshPage"
      :on-request="fetchGoalsPageWithTableLoading"
      :on-goal-close="refreshPage"
      :pagination="{}"
    />
    <ComponentLoading
      :is-loading="localLoading"
    />
  </q-page>
</template>

<script>
import GoalsTable from 'components/pages/goals/GoalTable';
import ComponentLoading from 'components/global/ComponentLoading';
import currenciesNameMapMixin from 'src/mixins/resources/currenciesNameMapMixin';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import accountStoreMixin from 'src/mixins/store/accountStoreMixin';
import goalStoreMixin from 'src/mixins/store/goalStoreMixin';
import AddGoalDialog from 'components/pages/goals/AddGoalDialog';

export default {
  name: 'Goals',
  components: { GoalsTable, ComponentLoading },
  mixins: [
    currenciesNameMapMixin,
    withLoadingAndErrorDialogMixin,
    accountStoreMixin,
    goalStoreMixin,
  ],
  data() {
    return {
      goalPagination: {
        page: 1,
        rowsPerPage: 5,
      },
      propsFromTable: {},
    };
  },
  methods: {
    async handleAdd() {
      this.$q.dialog({
        component: AddGoalDialog,
      }).onOk(this.refreshPage);
    },
    async fetchGoalsPageWithTableLoading(props) {
      this.propsFromTable = {
        ...props,
      };
      await this.withComponentLoading(
        this.fetchGoalsPage,
        this.$refs.goalTable,
      );
    },
    async fetchGoalsPage() {
      const { pagination } = this.propsFromTable;
      const { page, rowsPerPage } = pagination;
      this.goalPagination = {
        page, rowsPerPage,
      };
      await this.fetchGoals();

      const newPagination = {
        ...this.paginationForTable,
        page,
      };
      this.$refs.goalTable.setPagination(newPagination);
    },
    async refreshPage() {
      await this.withLocalLoadingAndErrorDialog(this.fetchGoalsAction);
    },
    async fetchGoals() {
      await this.fetchGoalsAction({
        page: this.goalPagination.page,
        pageSize: this.goalPagination.rowsPerPage,
      });
    },
  },
  computed: {
    goalRows() {
      return this.goalsGetter;
    },
    paginationForTable() {
      return {
        // sortBy: 'desc',
        // descending: false,
        ...this.goalPagination,
        rowsNumber: this.goalsGetter.length,
      };
    },
  },
  async mounted() {
    await this.refreshPage();
  },
};
</script>

<style scoped>

</style>
