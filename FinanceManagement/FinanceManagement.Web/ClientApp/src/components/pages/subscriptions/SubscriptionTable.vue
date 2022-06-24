<template>
  <div
      class="col-12 q-pa-md"
  >
    <q-table
        :title="$t('subscription.subscriptions')"
        dense
        :rows="formattedSubscriptionRows"
        :columns="subscriptionColumns"
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
              @click="handleEditSubscription(props)"
              icon="edit"
          ></q-btn>
          <q-btn
              dense
              round
              flat
              color="grey"
              @click="handleDeleteSubscription(props)"
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
import EditSubscriptionDialog from 'components/pages/subscriptions/EditSubscriptionDialog';
import subscriptionStoreMixin from 'src/mixins/store/subscriptionStoreMixin';
import withLoadingMixin from 'src/mixins/decorators/withLoadingMixin';
import ComponentLoading from 'components/global/ComponentLoading';
import { cutTimeFromDateString } from 'src/utils/common';

export default {
  name: 'SubscriptionsTable',
  components: { ComponentLoading },
  mixins: [subscriptionStoreMixin, withLoadingMixin],
  props: {
    subscriptions: {
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
    onSubscriptionClose: {
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
    async handleDeleteSubscription({ row: subscription }) {
      this.selectedRow = subscription;
      this.$q.dialog({
        title: this.$t('confirm'),
        message: this.$t('subscription.areYouSureToDelete'),
        cancel: true,
        persistent: true,
      }).onOk(this.deleteSubscription);
    },
    async deleteSubscription() {
      const subscriptionId = this.selectedRow.id;
      const { accountId } = this.selectedRow;
      await this.deleteSubscriptionAction({ accountId, subscriptionId });
      await this.onDelete();
    },
    handleEditSubscription({ row: subscription }) {
      this.$q.dialog({
        component: EditSubscriptionDialog,
        componentProps: {
          subscription,
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
    formattedSubscriptionRows() {
      return this.subscriptions.map((t) => ({
        ...t,
        dateTo: cutTimeFromDateString(t.dateTo),
        dateFrom: cutTimeFromDateString(t.dateFrom),
      }));
    },
    subscriptionColumns() {
      return [
        {
          name: 'name',
          field: 'name',
          label: this.$t('subscription.name'),
          align: 'left',
        },
        {
          name: 'dateFrom',
          field: 'dateFrom',
          label: this.$t('subscription.dateFrom'),
          align: 'left',
        },
        {
          name: 'dateTo',
          field: 'dateTo',
          label: this.$t('subscription.dateTo'),
          align: 'left',
        },
        {
          name: 'nextBilling',
          field: 'nextBilling',
          label: this.$t('subscription.nextBilling'),
          align: 'left',
        },
        {
          name: 'amount',
          field: 'amount',
          label: this.$t('subscription.amount'),
          align: 'left',
        },
        {
          name: 'linkedAccount',
          field: 'accountId',
          label: this.$t('subscription.linkedAccount'),
          align: 'left',
        },
        {
          name: 'active',
          field: 'isActive',
          label: this.$t('subscription.active'),
          align: 'left',
        },
        {
          name: 'actions',
          align: 'right',
          label: this.$t('subscription.actions'),
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
