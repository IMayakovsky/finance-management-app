<template>
  <DialogBase
    ref="dialog"
    :on-ok-handler="handleOK"
  >
      <q-card-section class="q-pt-none flex justify-center column">
        <CustomSelect
          v-model="selectedAccountId"
          :options="accounts"
          :label="$t('account.selectAccount')"
        />
        <ComponentLoading
          :is-loading="localLoading"
        />
      </q-card-section>
  </DialogBase>
</template>

<script>

import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import dialogBaseMixin from 'src/mixins/components/dialogBaseMixin';
import debtStoreMixin from 'src/mixins/store/debtStoreMixin';
import CustomSelect from 'components/global/CustomSelect';
import ComponentLoading from 'components/global/ComponentLoading';
import DialogBase from 'components/global/DialogBase';
import accountStoreMixin from 'src/mixins/store/accountStoreMixin';

export default {
  name: 'AddDebtDialog',
  props: {
    debtId: Number,
  },
  data() {
    return {
      selectedAccountId: -1,
    };
  },
  components: {
    DialogBase,
    ComponentLoading,
    CustomSelect,
  },
  mixins: [
    withLoadingAndErrorDialogMixin,
    dialogBaseMixin,
    debtStoreMixin,
    accountStoreMixin,
  ],
  methods: {
    async handleOK() {
      await this.withLocalLoadingAndErrorDialog(
        this.closeDebt,
      );
    },
    async fetchAccountsWithLoading() {
      await this.withLocalLoadingAndErrorDialog(this.fetchAccountsAction);
    },
    async closeDebt() {
      const [debtId, accountId] = [this.debtId, this.selectedAccountId];
      await this.closeDebtAction({ debtId, accountId });
      await this.emitOK();
    },
  },
  computed: {
    accounts() {
      return this.accountsGetter
        .filter((account) => account.currency === this.selectedDebt.currency)
        .map((account) => ({
          label: account.name,
          value: account.id,
        }));
    },
    selectedDebt() {
      return this.debtsGetter.find(({ id }) => id === this.debtId);
    },
  },
  async mounted() {
    await this.fetchAccountsWithLoading();
  },
};
</script>
