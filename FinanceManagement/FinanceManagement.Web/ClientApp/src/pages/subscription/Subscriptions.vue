<template>
  <q-page>
    <q-btn @click="handleAdd" class="q-ml-md">{{ $t("subscription.add") }}</q-btn>
    <SubscriptionsTable
      :key="{ subscriptionRows, paginationForTable }"
      :subscriptions="subscriptionRows"
      ref="subscriptionTable"
      :on-delete="refreshPage"
      :on-edit="refreshPage"
      :on-request="fetchSubscriptionsPageWithTableLoading"
      :on-subscription-close="refreshPage"
      :pagination="{}"
    />
    <ComponentLoading
      :is-loading="localLoading"
    />
  </q-page>
</template>

<script>
import SubscriptionsTable from 'components/pages/subscriptions/SubscriptionTable';
import ComponentLoading from 'components/global/ComponentLoading';
import currenciesNameMapMixin from 'src/mixins/resources/currenciesNameMapMixin';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import accountStoreMixin from 'src/mixins/store/accountStoreMixin';
import subscriptionStoreMixin from 'src/mixins/store/subscriptionStoreMixin';
import AddSubscriptionDialog from 'components/pages/subscriptions/AddSubscriptionDialog';

export default {
  name: 'Subscriptions',
  components: { SubscriptionsTable, ComponentLoading },
  mixins: [
    currenciesNameMapMixin,
    withLoadingAndErrorDialogMixin,
    accountStoreMixin,
    subscriptionStoreMixin,
  ],
  data() {
    return {
      subscriptionPagination: {
        page: 1,
        rowsPerPage: 5,
      },
      propsFromTable: {},
    };
  },
  methods: {
    async handleAdd() {
      this.$q.dialog({
        component: AddSubscriptionDialog,
      }).onOk(this.refreshPage);
    },
    async fetchSubscriptionsPageWithTableLoading(props) {
      this.propsFromTable = {
        ...props,
      };
      await this.withComponentLoading(
        this.fetchSubscriptionsPage,
        this.$refs.subscriptionTable,
      );
    },
    async fetchSubscriptionsPage() {
      const { pagination } = this.propsFromTable;
      const { page, rowsPerPage } = pagination;
      this.subscriptionPagination = {
        page, rowsPerPage,
      };
      await this.fetchSubscriptions();

      const newPagination = {
        ...this.paginationForTable,
        page,
      };
      this.$refs.subscriptionTable.setPagination(newPagination);
    },
    async refreshPage() {
      await this.withLocalLoadingAndErrorDialog(this.fetchSubscriptions);
    },
    async fetchSubscriptions() {
      await this.fetchAllSubscriptionsAction({
        page: this.subscriptionPagination.page,
        pageSize: this.subscriptionPagination.rowsPerPage,
      });
    },
  },
  computed: {
    subscriptionRows() {
      return this.subscriptionsGetter;
    },
    paginationForTable() {
      return {
        // sortBy: 'desc',
        // descending: false,
        ...this.subscriptionPagination,
        rowsNumber: this.subscriptionsGetter.length,
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
