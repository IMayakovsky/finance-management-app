<template>
  <div
      class="col-12 q-pa-md"
  >
    <q-table
        :title="$t('category.categories')"
        dense
        id="category-table"
        :rows="formattedCategoryRows"
        :columns="categoryColumns"
        row-key="name"
        :loading="localLoading"
        v-model:pagination="initialPagination"
        @request="onRequest"
    >
      <template v-slot:body-cell-actions="props">
        <q-td :props="props">
          <q-btn
              dense
              round
              flat
              color="grey"
              @click="handleEditCategory(props)"
              icon="edit"
          ></q-btn>
          <q-btn
              dense
              round
              flat
              color="grey"
              @click="handleDeleteCategory(props)"
              icon="delete"
          ></q-btn>
        </q-td>
      </template>
      <template v-slot:loading>
        <ComponentLoading/>
      </template>
    </q-table>
  </div>
</template>
<script>
import EditCategoryDialog from 'components/pages/categories/EditCategoryDialog';
import categoryStoreMixin from 'src/mixins/store/categoryStoreMixin';
import withLoadingMixin from 'src/mixins/decorators/withLoadingMixin';
import ComponentLoading from 'components/global/ComponentLoading';

export default {
  name: 'CategoriesTable',
  components: { ComponentLoading },
  mixins: [categoryStoreMixin, withLoadingMixin],
  props: {
    categories: {
      type: Array,
      default: () => [],
    },
    pagination: {
      type: Object,
      default: () => {},
    },
    onDelete: {
      type: Function,
      default: () => null,
    },
    onEdit: {
      type: Function,
      default: () => null,
    },
    onRequest: {
      type: Function,
      default: () => null,
    },
    isLoading: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      initialPagination: {
        ...this.pagination,
      },
      selectedRow: {},
    };
  },
  methods: {
    async handleDeleteCategory({ row: category }) {
      this.selectedRow = category;
      this.$q.dialog({
        title: this.$t('confirm'),
        message: this.$t('category.areYouSureToDelete'),
        cancel: true,
        persistent: true,
      }).onOk(this.deleteCategory);
    },
    async deleteCategory() {
      const categoryId = this.selectedRow.id;
      await this.deleteCategoryAction({ categoryId });
      await this.onDelete();
    },
    handleEditCategory({ row: category }) {
      this.$q.dialog({
        component: EditCategoryDialog,
        componentProps: {
          category,
        },
      }).onOk(this.onEdit);
    },
    setPagination(pagination) {
      this.initialPagination = {
        ...pagination,
      };
    },
  },
  computed: {
    formattedCategoryRows() {
      return this.categories.map((t) => ({
        ...t,
      }));
    },
    categoryColumns() {
      return [
        {
          name: 'name',
          field: 'name',
          label: this.$t('category.name'),
          align: 'left',
        },
        {
          name: 'actions',
          align: 'right',
          label: this.$t('category.actions'),
          field: 'actions',
        },
      ];
    },
  },
  watch: {
    pagination: {
      handler(newValue) {
        this.initialPagination = {
          ...newValue,
        };
      },
      deep: true,
    },
  },
};
</script>
