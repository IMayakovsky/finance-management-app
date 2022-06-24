<template>
  <div
      class="col-12 q-pa-md"
  >
    <q-table
        :title="$t('debt.debts')"
        dense
        :rows="formattedDebtRows"
        :columns="debtColumns"
        row-key="name"
        :loading="localLoading"
        v-model:pagination="initialPagination"
        @request="onRequest"
    >
      <template v-slot:body-cell-actions="props">
        <q-td :props="props">
          <q-btn
            dense
            round
            flat
            color="grey"
            @click="handleCloseDebt(props)"
            icon="credit_score"
          ></q-btn>
          <q-btn
              dense
              round
              flat
              color="grey"
              @click="handleEditDebt(props)"
              icon="edit"
          ></q-btn>
          <q-btn
              dense
              round
              flat
              color="grey"
              @click="handleDeleteDebt(props)"
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
import EditDebtDialog from 'components/pages/debts/EditDebtDialog';
import debtStoreMixin from 'src/mixins/store/debtStoreMixin';
import withLoadingMixin from 'src/mixins/decorators/withLoadingMixin';
import ComponentLoading from 'components/global/ComponentLoading';
import { cutTimeFromDateString } from 'src/utils/common';
import CloseDebtDialog from 'components/pages/debts/CloseDebtDialog';

export default {
  name: 'DebtsTable',
  components: { ComponentLoading },
  mixins: [debtStoreMixin, withLoadingMixin],
  props: {
    debts: {
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
    onDebtClose: {
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
    async handleCloseDebt({ row: debt }) {
      this.selectedRow = debt;
      this.$q.dialog({
        component: CloseDebtDialog,
        componentProps: {
          debtId: debt.id,
        },
      }).onOk(this.onDebtClose);
    },
    async handleDeleteDebt({ row: debt }) {
      this.selectedRow = debt;
      this.$q.dialog({
        title: this.$t('confirm'),
        message: this.$t('debt.areYouSureToDelete'),
        cancel: true,
        persistent: true,
      }).onOk(this.deleteDebt);
    },
    async deleteDebt() {
      const debtId = this.selectedRow.id;
      await this.deleteDebtAction({ debtId });
      await this.onDelete();
    },
    handleEditDebt({ row: debt }) {
      this.$q.dialog({
        component: EditDebtDialog,
        componentProps: {
          debt,
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
    formattedDebtRows() {
      return this.debts.map((d) => ({
        ...d,
        created: cutTimeFromDateString(d.created),
        dueTo: cutTimeFromDateString(d.dueTo),
        amount: `${d.amount} ${d.currency}`,
      }));
    },
    debtColumns() {
      return [
        {
          name: 'name',
          field: 'name',
          label: this.$t('debt.name'),
          align: 'left',
        },
        {
          name: 'note',
          field: 'note',
          label: this.$t('debt.note'),
          align: 'left',
        },
        {
          name: 'created',
          field: 'created',
          label: this.$t('debt.created'),
          align: 'left',
        },
        {
          name: 'dueTo',
          field: 'dueTo',
          label: this.$t('debt.dueTo'),
          align: 'left',
        },
        {
          name: 'amount',
          field: 'amount',
          label: this.$t('debt.amount'),
          align: 'left',
        },
        {
          name: 'actions',
          align: 'right',
          label: this.$t('debt.actions'),
          field: 'actions',
        },
      ];
    },
  },
  watch: {
    pagination: {
      handler(newValue) {
        this.initialPagination = {
          ...newValue,
        };
      },
      deep: true,
    },
  },
};
</script>
