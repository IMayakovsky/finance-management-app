<template>
  <DialogBase
    ref="dialog"
    :on-ok-handler="handleOK"
  >
      <q-card-section class="q-pt-none flex justify-center column">
        <q-input
          filled
          type="text"
          v-model="goal.name"
          :label="$t('goal.name')"
          class="q-pb-none q-mt-lg"
          :rules="[ val => val && val.length > 0]"
        ></q-input>
        <DateSelection
          v-model="goal.dateTo"
        />
        <q-input
          filled
          type="text"
          v-model="goal.currentAmount"
          :label="$t('goal.currentAmount')"
          class="q-pb-none q-mt-lg"
        ></q-input>
        <q-input
          filled
          type="text"
          v-model="goal.fullAmount"
          :label="$t('goal.fullAmount')"
          class="q-pb-none q-mt-lg"
        ></q-input>
        <q-input
          filled
          type="text"
          v-model="goal.description"
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
import dialogBaseMixin from 'src/mixins/components/dialogBaseMixin';
import goalStoreMixin from 'src/mixins/store/goalStoreMixin';
import DialogBase from 'components/global/DialogBase';
import ComponentLoading from 'components/global/ComponentLoading';
import DateSelection from 'components/global/DateSelection';
import { convertDateToOnlyDateString } from 'src/utils/common';

export default {
  name: 'AddGoalDialog',
  data() {
    return {
      goal: {
        currentAmount: 0,
        fullAmount: 0,
        dateTo: convertDateToOnlyDateString(new Date()),
        name: 'string',
        description: 'string',
      },
    };
  },
  components: {
    DateSelection, ComponentLoading, DialogBase,
  },
  mixins: [
    withLoadingAndErrorDialogMixin,
    dialogBaseMixin,
    goalStoreMixin,
  ],
  methods: {
    async handleOK() {
      await this.withLocalLoadingAndErrorDialog(
        this.createGoal,
      );
      // await this.okProcedure();
    },
    async createGoal() {
      await this.createGoalAction(this.goalToCreate);
      this.emitOK();
    },
  },
  computed: {
    goalToCreate() {
      return {
        ...this.goal,
      };
    },
  },
};
</script>
