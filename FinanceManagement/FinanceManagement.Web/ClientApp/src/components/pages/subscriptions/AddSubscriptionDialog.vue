<template>
  <DialogBase
    ref="dialog"
    :on-ok-handler="handleOK"
  >
      <q-card-section class="q-pt-none flex justify-center column">
        <q-input
          filled
          type="text"
          v-model="subscription.name"
          :label="$t('subscription.name')"
          class="q-pb-none q-mt-lg"
          :rules="[ val => val && val.length > 0]"
        ></q-input>
        <DateSelection
          v-model="subscription.dateFrom"
        />
        <DateSelection
          v-model="subscription.dateTo"
        />
        <q-input
          filled
          type="text"
          v-model="subscription.amount"
          :label="$t('subscription.amount')"
          class="q-pb-none q-mt-lg"
        ></q-input>
        <CustomSelect
          v-model="subscription.accountId"
          :options="accounts"
          :label="$t('account.selectAccount')"
        />
      </q-card-section>
    <ComponentLoading
      :is-loading="localLoading"
    />
  </DialogBase>
</template>

<script>

import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import dialogBaseMixin from 'src/mixins/components/dialogBaseMixin';
import subscriptionStoreMixin from 'src/mixins/store/subscriptionStoreMixin';
import DialogBase from 'components/global/DialogBase';
import ComponentLoading from 'components/global/ComponentLoading';
import DateSelection from 'components/global/DateSelection';
import { convertDateToOnlyDateString } from 'src/utils/common';
import CustomSelect from 'components/global/CustomSelect';
import accountStoreMixin from 'src/mixins/store/accountStoreMixin';

export default {
  name: 'AddSubscriptionDialog',
  data() {
    return {
      subscription: {
        dateTo: convertDateToOnlyDateString(new Date()),
        dateFrom: convertDateToOnlyDateString(new Date()),
      },
    };
  },
  components: {
    CustomSelect,
    DateSelection,
    ComponentLoading,
    DialogBase,
  },
  mixins: [
    withLoadingAndErrorDialogMixin,
    dialogBaseMixin,
    subscriptionStoreMixin,
    accountStoreMixin,
  ],
  methods: {
    async handleOK() {
      await this.withLocalLoadingAndErrorDialog(
        this.createSubscription,
      );
      // await this.okProcedure();
    },
    async createSubscription() {
      await this.createSubscriptionAction(this.subscriptionToCreate);
      this.emitOK();
    },
    async fetchAccountsWithLoading() {
      await this.withLocalLoadingAndErrorDialog(this.fetchAccountsAction);
    },
  },
  async mounted() {
    await this.fetchAccountsWithLoading();
  },
  computed: {
    subscriptionToCreate() {
      return {
        ...this.subscription,
      };
    },
    accounts() {
      return this.accountsGetter.map((account) => ({
        label: account.name,
        value: account.id,
      }));
    },
  },
};
</script>
