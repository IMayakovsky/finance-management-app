<template>
  <DialogBase
    ref="dialog"
    :on-ok-handler="handleOK"
  >
    <q-card-section class="q-pt-none flex justify-center column">
      <q-input
        filled
        type="text"
        v-model="categoryForm.name"
        :label="$t('category.name')"
        class="q-pb-none q-mt-lg"
        :rules="[ val => val && val.length > 0]"
      ></q-input>
    </q-card-section>
    <ComponentLoading
      :is-loading="localLoading"
    />
  </DialogBase>
</template>

<script>

import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import categoryStoreMixin from 'src/mixins/store/categoryStoreMixin';
import DialogBase from 'components/global/DialogBase';
import dialogBaseMixin from 'src/mixins/components/dialogBaseMixin';
import ComponentLoading from 'components/global/ComponentLoading';

export default {
  name: 'EditCategoryDialog',
  components: { ComponentLoading, DialogBase },
  props: {
    category: {
      name: String,
    },
  },
  data() {
    return {
      categoryForm: {
        ...this.category,
      },
    };
  },
  mixins: [
    withLoadingAndErrorDialogMixin,
    categoryStoreMixin,
    dialogBaseMixin,
  ],
  methods: {
    async handleOK() {
      await this.withGlobalLoadingAndErrorDialog(
        this.updateCategory,
      );
    },
    async updateCategory() {
      await this.updateCategoryAction(this.categoryForm);
      this.emitOK();
    },
  },
};
</script>
