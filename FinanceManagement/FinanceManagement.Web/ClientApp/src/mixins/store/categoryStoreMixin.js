import { mapActions, mapGetters } from 'vuex';
import { CategoryStoreActions } from 'src/store/category/actions.meta';
import { CategoryStoreGetters } from 'src/store/category/getters.meta';

export default {
  computed: {
    ...mapGetters({
      categoriesGetter: CategoryStoreGetters.getCategories,
      userCategoriesGetter: CategoryStoreGetters.getUserCategories,
      defaultCategoriesGetter: CategoryStoreGetters.getDefaultCategories,
    }),
  },
  methods: {
    ...mapActions({
      fetchCategoriesAction: CategoryStoreActions.fetchCategories,
      fetchUserCategoriesAction: CategoryStoreActions.fetchUserCategories,
      fetchDefaultCategoriesAction: CategoryStoreActions.fetchDefaultCategories,
      deleteCategoryAction: CategoryStoreActions.deleteCategory,
      updateCategoryAction: CategoryStoreActions.updateCategory,
      createCategoryAction: CategoryStoreActions.createCategory,
    }),
  },
};
