import { GoalStoreMutations as GoalMutations } from 'src/store/goal/mutations.meta';

export const mutations = {
  [GoalMutations.setGoals](state, goals) {
    state.goals = goals;
  },
};
