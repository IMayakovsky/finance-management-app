<template>
  <DialogBase
    ref="dialog"
    :on-ok-handler="handleOK"
  >
      <q-card-section class="q-pt-none flex justify-center column">
        <q-input
          filled
          type="text"
          v-model="account.name"
          :label="$t('account.name')"
          class="q-pb-none q-mt-lg"
          :rules="[ val => val && val.length > 0]"
        ></q-input>
        <CurrencySelect
          v-model="selectedCurrency"
        />
        <q-input
          filled
          type="text"
          v-model="account.amount"
          :label="$t('account.amount')"
          class="q-pb-none q-mt-lg"
        ></q-input>
      </q-card-section>
    <ComponentLoading
      :is-loading="localLoading"
    />
  </DialogBase>
</template>

<script>

import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import dialogBaseMixin from 'src/mixins/components/dialogBaseMixin';
import accountStoreMixin from 'src/mixins/store/accountStoreMixin';
import DialogBase from 'components/global/DialogBase';
import ComponentLoading from 'components/global/ComponentLoading';
import CurrencySelect from 'components/global/CurrencySelect';

export default {
  name: 'AddAccountDialog',
  data() {
    return {
      account: {
        currency: 0,
        name: '',
        amount: 0.0,
      },
      selectedCurrency: this.$t('account.selectCurrency'),
    };
  },
  components: { CurrencySelect, ComponentLoading, DialogBase },
  mixins: [
    withLoadingAndErrorDialogMixin,
    dialogBaseMixin,
    accountStoreMixin,
  ],
  methods: {
    async handleOK() {
      await this.withLocalLoadingAndErrorDialog(
        this.createAccount,
      );
      // await this.okProcedure();
    },
    async createAccount() {
      await this.createAccountAction(this.accountToCreate);
      this.emitOK();
    },
  },
  computed: {
    accountToCreate() {
      return {
        ...this.account,
        currency: this.selectedCurrency.value,
      };
    },
  },
};
</script>
