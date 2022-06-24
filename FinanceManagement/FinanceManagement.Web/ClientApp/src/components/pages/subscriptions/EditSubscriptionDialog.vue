<template>
  <DialogBase
    ref="dialog"
    :on-ok-handler="handleOK"
  >
    <q-card-section class="q-pt-none flex justify-center column">
      <q-input
        filled
        type="text"
        v-model="subscriptionForm.name"
        :label="$t('subscription.name')"
        class="q-pb-none q-mt-lg"
        :rules="[ val => val && val.length > 0]"
      ></q-input>
      <DateSelection
        v-model="subscriptionForm.dateFrom"
      />
      <DateSelection
        v-model="subscriptionForm.dateTo"
      />
      <q-input
        filled
        type="text"
        v-model="subscriptionForm.amount"
        :label="$t('subscription.amount')"
        class="q-pb-none q-mt-lg"
      ></q-input>
      <CustomSelect
        v-model="subscriptionForm.accountId"
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
import subscriptionStoreMixin from 'src/mixins/store/subscriptionStoreMixin';
import DialogBase from 'components/global/DialogBase';
import dialogBaseMixin from 'src/mixins/components/dialogBaseMixin';
import { convertDateToOnlyDateString, cutTimeFromDateString } from 'src/utils/common';
import ComponentLoading from 'components/global/ComponentLoading';
import DateSelection from 'components/global/DateSelection';
import accountStoreMixin from 'src/mixins/store/accountStoreMixin';
import CustomSelect from 'components/global/CustomSelect';

export default {
  name: 'EditSubscriptionDialog',
  components: {
    CustomSelect, DateSelection, ComponentLoading, DialogBase,
  },
  props: {
    subscription: {
      accountId: -1,
      dateTo: convertDateToOnlyDateString(new Date()),
      dateFrom: convertDateToOnlyDateString(new Date()),
    },
  },
  data() {
    return {
      subscriptionForm: {
        ...this.subscription,
        dateTo: cutTimeFromDateString(this.subscription.dateTo),
        dateFrom: cutTimeFromDateString(this.subscription.dateFrom),
      },
    };
  },
  mixins: [
    withLoadingAndErrorDialogMixin,
    subscriptionStoreMixin,
    dialogBaseMixin,
    accountStoreMixin,
  ],
  methods: {
    async handleOK() {
      await this.withGlobalLoadingAndErrorDialog(
        this.updateSubscription,
      );
    },
    async updateSubscription() {
      await this.updateSubscriptionAction(this.subscriptionForm);
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
    accounts() {
      return this.accountsGetter.map((account) => ({
        label: account.name,
        value: account.id,
      }));
    },
  },
};
</script>
