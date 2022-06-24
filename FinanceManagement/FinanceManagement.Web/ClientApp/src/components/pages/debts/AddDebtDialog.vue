<template>
  <DialogBase
    ref="dialog"
    :on-ok-handler="handleOK"
  >
      <q-card-section class="q-pt-none flex justify-center column">
        <q-input
          filled
          type="text"
          v-model="debt.name"
          :label="$t('debt.name')"
          class="q-pb-none q-mt-lg"
          :rules="[ val => val && val.length > 0]"
        ></q-input>
        <DateSelection
          v-model="debt.dueTo"
        />
        <q-input
          filled
          type="text"
          v-model="debt.amount"
          :label="$t('debt.amount')"
          class="q-pb-none q-mt-lg"
        ></q-input>
        <q-input
          filled
          type="text"
          v-model="debt.note"
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
import dialogBaseMixin from 'src/mixins/components/dialogBaseMixin';
import debtStoreMixin from 'src/mixins/store/debtStoreMixin';
import DialogBase from 'components/global/DialogBase';
import ComponentLoading from 'components/global/ComponentLoading';
import DateSelection from 'components/global/DateSelection';
import { convertDateToOnlyDateString } from 'src/utils/common';

export default {
  name: 'AddDebtDialog',
  data() {
    return {
      debt: {
        amount: 0,
        name: 'string',
        dueTo: convertDateToOnlyDateString(new Date()),
        note: 'string',
      },
    };
  },
  components: {
    DateSelection, ComponentLoading, DialogBase,
  },
  mixins: [
    withLoadingAndErrorDialogMixin,
    dialogBaseMixin,
    debtStoreMixin,
  ],
  methods: {
    async handleOK() {
      await this.withLocalLoadingAndErrorDialog(
        this.createDebt,
      );
      // await this.okProcedure();
    },
    async createDebt() {
      await this.createDebtAction(this.debtToCreate);
      this.emitOK();
    },
  },
  computed: {
    debtToCreate() {
      return {
        ...this.debt,
      };
    },
  },
};
</script>
