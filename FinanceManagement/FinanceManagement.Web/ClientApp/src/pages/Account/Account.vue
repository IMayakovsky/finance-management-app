<template>
  <q-page>
    <AccountCard
      :key="account"
      :account="account"
      :goToEnabled="false"
      @add:transaction="refreshPage"
      @delete="goToAccountsPage"
      @edit="refreshAccount"
      ref="accountCard"
    />
    <TransactionsTable
      :key="{ transactionRows, paginationForTable }"
      :transactions="transactionRows"
      ref="transactionTable"
      :on-delete="refreshPage"
      :on-edit="refreshPage"
      :on-request="fetchTransactionsPageWithTableLoading"
      :pagination="paginationForTable"
      :transaction-type="transactionTypePayload.transactionType"
      :parent-id="accountId"
    />
    <ComponentLoading
      :is-loading="localLoading"
    />
  </q-page>
</template>

<script>
import currenciesNameMapMixin from 'src/mixins/resources/currenciesNameMapMixin';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import accountStoreMixin from 'src/mixins/store/accountStoreMixin';
import transactionStoreMixin from 'src/mixins/store/transactionStoreMixin';
import AccountCard from 'components/pages/accounts/AccountCard';
import TransactionsTable from 'components/pages/account/TransactionsTable';
import { Routes } from 'src/router/routes';
import ComponentLoading from 'components/global/ComponentLoading';
import { TransactionStoreMeta } from 'src/store/transaction/store.meta';
import categoryStoreMixin from 'src/mixins/store/categoryStoreMixin';

export default {
  name: 'Account',
  components: { ComponentLoading, TransactionsTable, AccountCard },
  mixins: [
    currenciesNameMapMixin,
    withLoadingAndErrorDialogMixin,
    accountStoreMixin,
    transactionStoreMixin,
    categoryStoreMixin,
  ],
  data() {
    return {
      transactionPagination: {
        page: 1,
        rowsPerPage: 5,
      },
      propsFromTable: {},
      transactionTypePayload: {
        transactionType: TransactionStoreMeta.Types.Account,
      },
    };
  },
  methods: {
    async fetchTransactionsPageWithTableLoading(props) {
      this.propsFromTable = {
        ...props,
      };
      await this.withComponentLoading(
        this.fetchTransactionsPage,
        this.$refs.transactionTable,
      );
    },
    async fetchTransactionsPage() {
      const { pagination } = this.propsFromTable;
      const { page, rowsPerPage } = pagination;
      this.transactionPagination = {
        page, rowsPerPage,
      };
      await this.fetchTransactions();

      const newPagination = {
        ...this.paginationForTable,
        page,
      };
      this.$refs.transactionTable.setPagination(newPagination);
    },
    async refreshAccount() {
      await this.withComponentLoading(
        this.fetchAccount,
        this.$refs.accountCard,
      );
    },
    async refreshPage() {
      await this.withLocalLoadingAndErrorDialog(this.fetchAccountAndTransactions);
    },
    async fetchAccountAndTransactions() {
      await Promise.all([
        this.fetchAccount(),
        this.fetchTransactions(this.transactionTypePayload),
        this.fetchCategoriesAction(),
      ]);
    },
    async fetchAccount() {
      await this.fetchAccountAction(this.accountId);
    },
    async fetchTransactions() {
      const { accountId } = this;
      await this.fetchTransactionsAction({
        ...this.transactionTypePayload,
        parentId: accountId,
        page: this.transactionPagination.page,
        pageSize: this.transactionPagination.rowsPerPage,
      });
    },
    goToAccountsPage() {
      this.$router.push(Routes.accounts);
    },
  },
  computed: {
    account() {
      return this.accountGetter;
    },
    accountId() {
      return this.$route.params.accountId;
    },
    transactionRows() {
      return this.transactionsGetter.map((t) => ({
        ...t,
        categoryName: this.categoriesGetter.find(({ id }) => id === t.categoryId)?.name,
      }));
    },
    paginationForTable() {
      return {
        // sortBy: 'desc',
        // descending: false,
        ...this.transactionPagination,
        rowsNumber: this.transactionTotalRowCountGetter,
      };
    },
  },
  async mounted() {
    await this.refreshPage();
  },
};
</script>
