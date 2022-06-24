<template>
  <q-page>
    <q-btn @click="handleAdd" class="q-ml-md">{{ $t("debt.add") }}</q-btn>
    <DebtsTable
      :key="{ debtRows, paginationForTable }"
      :debts="debtRows"
      ref="debtTable"
      :on-delete="refreshPage"
      :on-edit="refreshPage"
      :on-request="fetchDebtsPageWithTableLoading"
      :on-debt-close="refreshPage"
      :pagination="{}"
    />
    <ComponentLoading
      :is-loading="localLoading"
    />
  </q-page>
</template>

<script>
import DebtsTable from 'components/pages/debts/DebtTable';
import ComponentLoading from 'components/global/ComponentLoading';
import currenciesNameMapMixin from 'src/mixins/resources/currenciesNameMapMixin';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import accountStoreMixin from 'src/mixins/store/accountStoreMixin';
import debtStoreMixin from 'src/mixins/store/debtStoreMixin';
import AddDebtDialog from 'components/pages/debts/AddDebtDialog';

export default {
  name: 'Debts',
  components: { DebtsTable, ComponentLoading },
  mixins: [
    currenciesNameMapMixin,
    withLoadingAndErrorDialogMixin,
    accountStoreMixin,
    debtStoreMixin,
  ],
  data() {
    return {
      debtPagination: {
        page: 1,
        rowsPerPage: 5,
      },
      propsFromTable: {},
    };
  },
  methods: {
    async handleAdd() {
      this.$q.dialog({
        component: AddDebtDialog,
      }).onOk(this.refreshPage);
    },
    async fetchDebtsPageWithTableLoading(props) {
      this.propsFromTable = {
        ...props,
      };
      await this.withComponentLoading(
        this.fetchDebtsPage,
        this.$refs.debtTable,
      );
    },
    async fetchDebtsPage() {
      const { pagination } = this.propsFromTable;
      const { page, rowsPerPage } = pagination;
      this.debtPagination = {
        page, rowsPerPage,
      };
      await this.fetchDebts();

      const newPagination = {
        ...this.paginationForTable,
        page,
      };
      this.$refs.debtTable.setPagination(newPagination);
    },
    async refreshPage() {
      await this.withLocalLoadingAndErrorDialog(this.fetchDebtsAction);
    },
    async fetchDebts() {
      await this.fetchDebtsAction({
        page: this.debtPagination.page,
        pageSize: this.debtPagination.rowsPerPage,
      });
    },
  },
  computed: {
    debtRows() {
      return this.debtsGetter;
    },
    paginationForTable() {
      return {
        // sortBy: 'desc',
        // descending: false,
        ...this.debtPagination,
        rowsNumber: this.debtsGetter.length,
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
