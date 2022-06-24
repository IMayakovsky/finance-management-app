<template>
  <DialogBase
    ref="dialog"
    :on-ok-handler="handleOK"
  >
      <q-card-section class="q-pt-none flex justify-center column">
        <q-input
          filled
          type="text"
          v-model="group.name"
          :label="$t('group.name')"
          class="q-pb-none q-mt-lg"
          :rules="[ val => val && val.length > 0]"
        ></q-input>
        <CurrencySelect
          v-model="selectedCurrency"
        />
      </q-card-section>
    <ComponentLoading
      :is-loading="localLoading"
    />
  </DialogBase>
</template>

<script>

import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import groupStoreMixin from 'src/mixins/store/groupStoreMixin';
import accountStoreMixin from 'src/mixins/store/accountStoreMixin';
import DialogBase from 'components/global/DialogBase';
import dialogBaseMixin from 'src/mixins/components/dialogBaseMixin';
import ComponentLoading from 'components/global/ComponentLoading';
import CurrencySelect from 'components/global/CurrencySelect';

export default {
  name: 'AddGroupDialog',
  data() {
    return {
      group: {
        name: '',
      },
      selectedCurrency: this.$t('account.selectCurrency'),
    };
  },
  components: { CurrencySelect, ComponentLoading, DialogBase },
  mixins: [
    withLoadingAndErrorDialogMixin,
    groupStoreMixin,
    accountStoreMixin,
    dialogBaseMixin,
  ],
  methods: {
    async handleOK() {
      await this.withLocalLoadingAndErrorDialog(
        this.createGroup,
      );
    },
    async createGroup() {
      await this.createGroupAction(this.groupToCreate);
      this.emitOK();
    },
  },
  computed: {
    groupToCreate() {
      return {
        name: this.group.name,
        currency: this.selectedCurrency.value,
      };
    },
  },
};
</script>
