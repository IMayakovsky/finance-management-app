<template>
  <div
      class="col-12 q-pa-md"
  >
    <q-table
        :title="$t('goal.goals')"
        dense
        :rows="formattedGoalRows"
        :columns="goalColumns"
        row-key="name"
        :loading="localLoading"
        v-model:pagination="initialPagination"
        @request="onRequest"
    >
      <template v-slot:body-cell-progress="props">
        <q-td :props="props">
          <q-linear-progress stripe size="10px" :value="props.row.progress" />
        </q-td>
      </template>
      <template v-slot:body-cell-active="props">
        <q-td :props="props">
          <q-icon name="add" v-if="props.row.isActive"/>
          <q-icon name="remove" v-else />
        </q-td>
      </template>
      <template v-slot:body-cell-actions="props">
        <q-td :props="props">
          <q-btn
            dense
            round
            flat
            color="grey"
            @click="handleCloseGoal(props)"
            v-if="props.row.isActive && props.row.currentAmount === props.row.fullAmount"
            icon="credit_score"
          ></q-btn>
          <q-btn
              dense
              round
              flat
              color="grey"
              @click="handleEditGoal(props)"
              v-if="props.row.isActive"
              icon="edit"
          ></q-btn>
          <q-btn
              dense
              round
              flat
              color="grey"
              @click="handleDeleteGoal(props)"
              icon="delete"
          ></q-btn>
        </q-td>
      </template>
      <template v-slot:loading>
        <ComponentLoading/>
      </template>
    </q-table>
  </div>
</template>
<script>
import EditGoalDialog from 'components/pages/goals/EditGoalDialog';
import goalStoreMixin from 'src/mixins/store/goalStoreMixin';
import withLoadingMixin from 'src/mixins/decorators/withLoadingMixin';
import ComponentLoading from 'components/global/ComponentLoading';
import { cutTimeFromDateString } from 'src/utils/common';

export default {
  name: 'GoalsTable',
  components: { ComponentLoading },
  mixins: [goalStoreMixin, withLoadingMixin],
  props: {
    goals: {
      type: Array,
      default: () => [],
    },
    pagination: {
      type: Object,
      default: () => {},
    },
    onDelete: {
      type: Function,
      default: () => null,
    },
    onEdit: {
      type: Function,
      default: () => null,
    },
    onRequest: {
      type: Function,
      default: () => null,
    },
    onGoalClose: {
      type: Function,
      default: () => null,
    },
    isLoading: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      initialPagination: {
        ...this.pagination,
      },
      selectedRow: {},
    };
  },
  methods: {
    async handleCloseGoal({ row: goal }) {
      this.selectedRow = goal;
      this.$q.dialog({
        title: this.$t('confirm'),
        message: this.$t('goal.areYouSureToClose'),
        cancel: true,
        persistent: true,
      }).onOk(this.closeGoal);
    },
    async closeGoal() {
      const goalId = this.selectedRow.id;
      await this.closeGoalAction({ goalId });
      await this.onGoalClose();
    },
    async handleDeleteGoal({ row: goal }) {
      this.selectedRow = goal;
      this.$q.dialog({
        title: this.$t('confirm'),
        message: this.$t('goal.areYouSureToDelete'),
        cancel: true,
        persistent: true,
      }).onOk(this.deleteGoal);
    },
    async deleteGoal() {
      const goalId = this.selectedRow.id;
      await this.deleteGoalAction({ goalId });
      await this.onDelete();
    },
    handleEditGoal({ row: goal }) {
      this.$q.dialog({
        component: EditGoalDialog,
        componentProps: {
          goal,
        },
      }).onOk(this.onEdit);
    },
    setPagination(pagination) {
      this.initialPagination = {
        ...pagination,
      };
    },
  },
  computed: {
    formattedGoalRows() {
      return this.goals.map((t) => ({
        ...t,
        dateTo: cutTimeFromDateString(t.dateTo),
        progress: t.currentAmount / t.fullAmount,
      }));
    },
    goalColumns() {
      return [
        {
          name: 'name',
          field: 'name',
          label: this.$t('goal.name'),
          align: 'left',
        },
        {
          name: 'description',
          field: 'description',
          label: this.$t('goal.description'),
          align: 'left',
        },
        {
          name: 'dateTo',
          field: 'dateTo',
          label: this.$t('goal.dateTo'),
          align: 'left',
        },
        {
          name: 'progress',
          field: 'progress',
          label: this.$t('goal.progress'),
          align: 'left',
        },
        {
          name: 'active',
          field: 'isActive',
          label: this.$t('goal.active'),
          align: 'left',
        },
        {
          name: 'actions',
          align: 'right',
          label: this.$t('goal.actions'),
          field: 'actions',
        },
      ];
    },
  },
  watch: {
    pagination: {
      handler(newValue) {
        this.setPagination(newValue);
      },
      deep: true,
    },
  },
};
</script>
