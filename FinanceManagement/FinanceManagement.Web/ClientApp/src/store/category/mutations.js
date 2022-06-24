import { CategoryStoreMutations as CategoryMutations } from 'src/store/category/mutations.meta';

export const mutations = {
  [CategoryMutations.setCategories](state, categories) {
    state.categories = categories;
  },
  [CategoryMutations.setDefaultCategories](state, newValue) {
    state.defaultCategories = newValue;
  },
  [CategoryMutations.setUserCategories](state, newValue) {
    state.userCategories = newValue;
  },
};
