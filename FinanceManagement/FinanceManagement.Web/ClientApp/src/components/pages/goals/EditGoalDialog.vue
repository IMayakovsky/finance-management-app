<template>
  <DialogBase
    ref="dialog"
    :on-ok-handler="handleOK"
  >
    <q-card-section class="q-pt-none flex justify-center column">
      <q-input
        filled
        type="text"
        v-model="goalForm.name"
        :label="$t('goal.name')"
        class="q-pb-none q-mt-lg"
        :rules="[ val => val && val.length > 0]"
      ></q-input>
      <DateSelection
        v-model="goalForm.dateTo"
      />
      <q-input
        filled
        type="text"
        v-model="goalForm.currentAmount"
        :label="$t('goal.currentAmount')"
        class="q-pb-none q-mt-lg"
      ></q-input>
      <q-input
        filled
        type="text"
        v-model="goalForm.fullAmount"
        :label="$t('goal.fullAmount')"
        class="q-pb-none q-mt-lg"
      ></q-input>
      <q-input
        filled
        type="text"
        v-model="goalForm.description"
        :label="$t('goal.description')"
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
import goalStoreMixin from 'src/mixins/store/goalStoreMixin';
import DialogBase from 'components/global/DialogBase';
import dialogBaseMixin from 'src/mixins/components/dialogBaseMixin';
import { convertDateToOnlyDateString, cutTimeFromDateString } from 'src/utils/common';
import ComponentLoading from 'components/global/ComponentLoading';
import DateSelection from 'components/global/DateSelection';

export default {
  name: 'EditGoalDialog',
  components: { DateSelection, ComponentLoading, DialogBase },
  props: {
    goal: {
      currentAmount: 0,
      fullAmount: 0,
      dateTo: convertDateToOnlyDateString(new Date()),
      name: 'string',
      description: 'string',
    },
  },
  data() {
    return {
      goalForm: {
        ...this.goal,
        dateTo: cutTimeFromDateString(this.goal.dateTo),
      },
    };
  },
  mixins: [
    withLoadingAndErrorDialogMixin,
    goalStoreMixin,
    dialogBaseMixin,
  ],
  methods: {
    async handleOK() {
      await this.withGlobalLoadingAndErrorDialog(
        this.updateGoal,
      );
    },
    async updateGoal() {
      await this.updateGoalAction(this.goalForm);
      this.emitOK();
    },
  },
};
</script>
