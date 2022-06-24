<template>
  <DialogBase
    ref="dialog"
    :on-ok-handler="handleOK"
    :disable-ok="localLoading"
  >
      <q-card-section class="q-pt-none flex justify-center column">
        <q-input
          filled
          type="text"
          v-model="category.name"
          :label="$t('category.name')"
          name="category-name"
          class="q-pb-none q-mt-lg"
          :rules="[ val => val && val.length > 0]"
        ></q-input>
        <ComponentLoading
          :is-loading="localLoading"
        />
      </q-card-section>
  </DialogBase>
</template>

<script>

import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import dialogBaseMixin from 'src/mixins/components/dialogBaseMixin';
import categoryStoreMixin from 'src/mixins/store/categoryStoreMixin';
import DialogBase from 'components/global/DialogBase';
import ComponentLoading from 'components/global/ComponentLoading';

export default {
  name: 'AddCategoryDialog',
  data() {
    return {
      category: {
        name: 'string',
      },
    };
  },
  components: {
    ComponentLoading, DialogBase,
  },
  mixins: [
    withLoadingAndErrorDialogMixin,
    dialogBaseMixin,
    categoryStoreMixin,
  ],
  methods: {
    async handleOK() {
      await this.withLocalLoadingAndErrorDialog(
        this.createCategory,
      );
      // await this.okProcedure();
    },
    async createCategory() {
      await this.createCategoryAction(this.categoryToCreate);
      this.emitOK();
    },
  },
  computed: {
    categoryToCreate() {
      return {
        ...this.category,
      };
    },
  },
};
</script>
