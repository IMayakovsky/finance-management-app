import { CategoryStoreGetters } from 'src/store/category/getters.meta';

export const getters = {
  [CategoryStoreGetters.getCategories]: (state) => [
    ...state.defaultCategories, ...state.userCategories,
  ],
  [CategoryStoreGetters.getDefaultCategories]: (state) => state.defaultCategories,
  [CategoryStoreGetters.getUserCategories]: (state) => state.userCategories,
};
