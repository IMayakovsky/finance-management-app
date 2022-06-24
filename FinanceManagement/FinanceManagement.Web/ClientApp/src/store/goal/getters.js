import { GoalStoreGetters } from 'src/store/goal/getters.meta';

export const getters = {
  [GoalStoreGetters.getGoals]: (state) => state.goals,
};
