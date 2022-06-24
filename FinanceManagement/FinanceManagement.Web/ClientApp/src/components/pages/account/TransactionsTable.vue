<template>
  <div
      class="col-12 q-pa-md"
  >
    <q-table
        :title="$t('transaction.transactions')"
        dense
        :rows="formattedTransactionRows"
        :columns="transactionColumns"
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
              @click="handleEditTransaction(props)"
              icon="edit"
          ></q-btn>
          <q-btn
              dense
              round
              flat
              color="grey"
              @click="handleDeleteTransaction(props)"
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
import CreateOrUpdateTransactionDialog from 'components/pages/account/CreateOrUpdateTransactionDialog';
import transactionStoreMixin from 'src/mixins/store/transactionStoreMixin';
import withLoadingMixin from 'src/mixins/decorators/withLoadingMixin';
import ComponentLoading from 'components/global/ComponentLoading';
import { cutTimeFromDateString } from 'src/utils/common';
import { OkActionEnum } from 'src/common/enums/components/okActions';

export default {
  name: 'TransactionsTable',
  components: { ComponentLoading },
  mixins: [transactionStoreMixin, withLoadingMixin],
  props: {
    transactions: {
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
    isLoading: {
      type: Boolean,
      default: false,
    },
    transactionType: String,
    parentId: Number,
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
    async handleDeleteTransaction({ row: transaction }) {
      this.selectedRow = transaction;
      this.$q.dialog({
        title: this.$t('confirm'),
        message: this.$t('transaction.areYouSureToDelete'),
        cancel: true,
        persistent: true,
      }).onOk(this.deleteTransaction);
    },
    handleEditTransaction({ row: transaction }) {
      this.$q.dialog({
        component: CreateOrUpdateTransactionDialog,
        componentProps: {
          transaction,
          parentId: this.parentId,
          transactionType: this.transactionType,
          okAction: OkActionEnum.Update,
        },
      }).onOk(this.onEdit);
    },
    setPagination(pagination) {
      this.initialPagination = {
        ...pagination,
      };
    },
    async deleteTransaction() {
      const [parentId, transactionId] = [this.parentId, this.selectedRow.id];
      await this.deleteTransactionAction({
        transactionType: this.transactionType,
        parentId,
        transactionId,
      });
      await this.onDelete();
    },
  },
  computed: {
    formattedTransactionRows() {
      return this.transactions.map((t) => ({
        ...t,
        date: cutTimeFromDateString(t.date),
      }));
    },
    transactionColumns() {
      return [
        {
          name: 'name',
          field: 'name',
          label: this.$t('transaction.name'),
          align: 'left',
        },
        {
          name: 'categoryName',
          field: 'categoryName',
          label: this.$t('transaction.category'),
          align: 'left',
        },
        {
          name: 'date',
          field: 'date',
          label: this.$t('transaction.date'),
          align: 'left',
        },
        {
          name: 'amount',
          field: 'amount',
          label: this.$t('transaction.amount'),
          align: 'left',
        },
        {
          name: 'actions',
          align: 'right',
          label: this.$t('transaction.actions'),
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
