import { CategoryStoreActions } from 'src/store/category/actions.meta';
import { CategoryStoreMutations } from 'src/store/category/mutations.meta';
import { CategoryService } from 'src/api/CategoryService';

export const actions = {
  async [CategoryStoreActions.fetchCategories](context) {
    // const { data: categories } = await CategoryService.fetchMany();
    // context.commit(CategoryStoreMutations.setCategories, categories);
    await Promise.all([
      context.dispatch(CategoryStoreActions.fetchDefaultCategories),
      context.dispatch(CategoryStoreActions.fetchUserCategories),
    ]);
  },
  async [CategoryStoreActions.fetchDefaultCategories](context) {
    const { data: categories } = await CategoryService.fetchDefaultCategories();
    context.commit(CategoryStoreMutations.setDefaultCategories, categories);
  },
  async [CategoryStoreActions.fetchUserCategories](context) {
    const { data: categories } = await CategoryService.fetchUserCategories();
    context.commit(CategoryStoreMutations.setUserCategories, categories);
  },
  async [CategoryStoreActions.createCategory](context, {
    name,
  }) {
    const { data: category } = await CategoryService.createOne({
      name,
    });
    return category;
  },
  async [CategoryStoreActions.deleteCategory](context, { categoryId }) {
    await CategoryService.deleteOne(categoryId);
  },
  async [CategoryStoreActions.updateCategory](context, category) {
    await CategoryService.updateOne(category.id, null, { name: category.name });
  },
};
