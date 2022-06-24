import { GoalStoreActions } from 'src/store/goal/actions.meta';
import { GoalStoreMutations } from 'src/store/goal/mutations.meta';
import { GoalService } from 'src/api/GoalService';

export const actions = {
  async [GoalStoreActions.fetchGoals](context) {
    const { data: goals } = await GoalService.fetchMany();
    context.commit(GoalStoreMutations.setGoals, goals);
  },
  async [GoalStoreActions.createGoal](context, {
    name, dateTo, currentAmount, fullAmount, description,
  }) {
    const { data: goal } = await GoalService.createOne({
      name, dateTo, currentAmount, fullAmount, description, isActive: true,
    });
    return goal;
  },
  async [GoalStoreActions.deleteGoal](context, { goalId }) {
    await GoalService.deleteOne(goalId);
  },
  async [GoalStoreActions.updateGoal](context, goal) {
    await GoalService.updateOne(goal.id, goal);
  },
  async [GoalStoreActions.closeGoal](context, { goalId }) {
    await GoalService.closeGoal({ goalId });
  },
  async [GoalStoreActions.updateGoalAmount](context, { goalId, accountId, amount }) {
    await GoalService.changeAmount({ goalId, accountId, amount });
  },
};
