<template>
  <DialogBase
    ref="dialog"
    :on-ok-handler="handleOK"
  >
    <q-card-section class="q-pt-none flex justify-center column">
      <q-input
        filled
        type="text"
        v-model="transactionForm.name"
        :label="$t('transaction.name')"
        class="q-pb-none q-mt-lg"
        :rules="[ val => val && val.length > 0]"
      ></q-input>
      <q-input
        filled
        type="text"
        v-model="transactionForm.amount"
        :label="$t('transaction.amount')"
        class="q-pb-none q-mt-lg"
      ></q-input>
      <CustomSelect
        v-model="selectedCategoryId"
        :options="categoryOptions"
      />
      <DateSelection
        v-model="transactionForm.date"
      />
      <ComponentLoading
        :is-loading="localLoading"
      />
    </q-card-section>
  </DialogBase>
</template>

<script>

import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import transactionStoreMixin from 'src/mixins/store/transactionStoreMixin';
import accountStoreMixin from 'src/mixins/store/accountStoreMixin';
import DialogBase from 'components/global/DialogBase';
import dialogBaseMixin from 'src/mixins/components/dialogBaseMixin';
import { convertDateToOnlyDateString } from 'src/utils/common';
import DateSelection from 'components/global/DateSelection';
import ComponentLoading from 'components/global/ComponentLoading';
import CustomSelect from 'components/global/CustomSelect';
import categoryStoreMixin from 'src/mixins/store/categoryStoreMixin';
import { OkActionEnum } from 'src/common/enums/components/okActions';
import { TransactionStoreMeta } from 'src/store/transaction/store.meta';

export default {
  name: 'CreateOrUpdateTransactionDialog',
  components: {
    CustomSelect, ComponentLoading, DateSelection, DialogBase,
  },
  props: {
    transaction: {
      type: Object,
      default: () => ({
        name: '',
        amount: 0,
        categoryId: null,
        date: convertDateToOnlyDateString(new Date()),
      }),
    },
    parentId: {
      type: Number,
      default: -1,
    },
    transactionType: String,
    okAction: String,
  },
  data() {
    return {
      transactionForm: {
        ...(this.transaction || this.transaction.default()),
      },
      selectedCategoryId: this.transaction.categoryId,
    };
  },
  mixins: [
    withLoadingAndErrorDialogMixin,
    transactionStoreMixin,
    accountStoreMixin,
    dialogBaseMixin,
    categoryStoreMixin,
  ],
  async mounted() {
    await this.withLocalLoadingAndErrorDialog(this.fetchCategoriesAction);
  },
  methods: {
    async handleOK() {
      await this.withLocalLoadingAndErrorDialog(
        this.actionsMap[this.okAction],
      );
    },
    async updateTransaction() {
      await this.updateTransactionAction(this.transactionToSubmit);
      this.emitOK();
    },
    async createTransaction() {
      await this.createTransactionAction(this.transactionToSubmit);
      this.emitOK();
    },
  },
  computed: {
    categoryOptions() {
      const categories = {
        [TransactionStoreMeta.Types.Account]: this.categoriesGetter,
        [TransactionStoreMeta.Types.Group]: this.defaultCategoriesGetter,
      };

      return (categories[this.transactionType] || []).map((e) => ({
        label: e.name,
        value: e.id,
      }));
    },
    transactionToSubmit() {
      const { parentId, transactionType } = this;
      return {
        transaction: {
          ...this.transactionForm,
          categoryId: this.selectedCategoryId,
        },
        parentId,
        transactionType,
      };
    },
    actionsMap() {
      return {
        [OkActionEnum.Create]: this.createTransaction,
        [OkActionEnum.Update]: this.updateTransaction,
      };
    },
  },
};
</script>
