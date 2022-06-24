<template>
  <q-page>
    <q-btn @click="handleAdd" class="q-ml-md">{{ $t("category.add") }}</q-btn>
    <CategoriesTable
      :key="{ categoryRows, paginationForTable }"
      :categories="categoryRows"
      ref="categoryTable"
      :on-delete="refreshPage"
      :on-edit="refreshPage"
      :on-request="fetchCategoriesPageWithTableLoading"
      :on-category-close="refreshPage"
      :pagination="{}"
    />
    <ComponentLoading
      :is-loading="localLoading"
    />
  </q-page>
</template>

<script>
import CategoriesTable from 'components/pages/categories/CategoryTable';
import ComponentLoading from 'components/global/ComponentLoading';
import currenciesNameMapMixin from 'src/mixins/resources/currenciesNameMapMixin';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import categoryStoreMixin from 'src/mixins/store/categoryStoreMixin';
import AddCategoryDialog from 'components/pages/categories/AddCategoryDialog';

export default {
  name: 'Categories',
  components: { CategoriesTable, ComponentLoading },
  mixins: [
    currenciesNameMapMixin,
    withLoadingAndErrorDialogMixin,
    categoryStoreMixin,
  ],
  data() {
    return {
      categoryPagination: {
        page: 1,
        rowsPerPage: 5,
      },
      propsFromTable: {},
    };
  },
  methods: {
    async handleAdd() {
      this.$q.dialog({
        component: AddCategoryDialog,
      }).onOk(this.refreshPage);
    },
    async fetchCategoriesPageWithTableLoading(props) {
      this.propsFromTable = {
        ...props,
      };
      await this.withComponentLoading(
        this.fetchCategoriesPage,
        this.$refs.categoryTable,
      );
    },
    async fetchCategoriesPage() {
      const { pagination } = this.propsFromTable;
      const { page, rowsPerPage } = pagination;
      this.categoryPagination = {
        page, rowsPerPage,
      };
      await this.fetchCategories();

      const newPagination = {
        ...this.paginationForTable,
        page,
      };
      this.$refs.categoryTable.setPagination(newPagination);
    },
    async refreshPage() {
      await this.withLocalLoadingAndErrorDialog(this.fetchCategoriesAction);
    },
    async fetchCategories() {
      await this.fetchCategoriesAction({
        page: this.categoryPagination.page,
        pageSize: this.categoryPagination.rowsPerPage,
      });
    },
  },
  computed: {
    categoryRows() {
      return this.categoriesGetter;
    },
    paginationForTable() {
      return {
        // sortBy: 'desc',
        // descending: false,
        ...this.categoryPagination,
        rowsNumber: this.categoriesGetter.length,
      };
    },
  },
  async mounted() {
    await this.refreshPage();
  },
};
</script>

<style scoped>

</style>
