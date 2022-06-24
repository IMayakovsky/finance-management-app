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
import goalStoreMixin from 'src/mixins/store/goalStoreMixin';
import CustomSelect from 'components/global/CustomSelect';
import ComponentLoading from 'components/global/ComponentLoading';
import DialogBase from 'components/global/DialogBase';
import accountStoreMixin from 'src/mixins/store/accountStoreMixin';

export default {
  name: 'AddGoalDialog',
  props: {
    goalId: Number,
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
    goalStoreMixin,
    accountStoreMixin,
  ],
  methods: {
    async handleOK() {
      await this.withLocalLoadingAndErrorDialog(
        this.closeGoal,
      );
    },
    async fetchAccountsWithLoading() {
      await this.withLocalLoadingAndErrorDialog(this.fetchAccountsAction);
    },
    async closeGoal() {
      const [goalId, accountId] = [this.goalId, this.selectedAccountId];
      await this.closeGoalAction({ goalId, accountId });
      await this.emitOK();
    },
  },
  computed: {
    accounts() {
      return this.accountsGetter.map((account) => ({
        label: account.name,
        value: account.id,
      }));
    },
  },
  async mounted() {
    await this.fetchAccountsWithLoading();
  },
};
</script>
