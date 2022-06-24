<template>
  <DialogBase
    ref="dialog"
    :on-ok-handler="handleOK"
  >
    <q-card-section class="q-pt-none flex justify-center column">
      <q-input
        filled
        type="text"
        v-model="debtForm.name"
        :label="$t('debt.name')"
        class="q-pb-none q-mt-lg"
        :rules="[ val => val && val.length > 0]"
      ></q-input>
      <DateSelection
        v-model="debtForm.dueTo"
      />
      <q-input
        filled
        type="text"
        v-model="debtForm.amount"
        :label="$t('debt.amount')"
        class="q-pb-none q-mt-lg"
      ></q-input>
      <q-input
        filled
        type="text"
        v-model="debtForm.note"
        :label="$t('debt.note')"
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
import debtStoreMixin from 'src/mixins/store/debtStoreMixin';
import DialogBase from 'components/global/DialogBase';
import dialogBaseMixin from 'src/mixins/components/dialogBaseMixin';
import { cutTimeFromDateString } from 'src/utils/common';
import ComponentLoading from 'components/global/ComponentLoading';
import DateSelection from 'components/global/DateSelection';

export default {
  name: 'EditDebtDialog',
  components: { DateSelection, ComponentLoading, DialogBase },
  props: {
    debt: {
      name: String,
      amount: Number,
      note: String,
      dueTo: String,
    },
  },
  data() {
    return {
      debtForm: {
        ...this.debt,
        dueTo: cutTimeFromDateString(this.debt.dueTo),
      },
    };
  },
  mixins: [
    withLoadingAndErrorDialogMixin,
    debtStoreMixin,
    dialogBaseMixin,
  ],
  methods: {
    async handleOK() {
      await this.withGlobalLoadingAndErrorDialog(
        this.updateDebt,
      );
    },
    async updateDebt() {
      await this.updateDebtAction(this.debtForm);
      this.emitOK();
    },
  },
};
</script>
