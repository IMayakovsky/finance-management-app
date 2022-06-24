import { mapActions, mapGetters } from 'vuex';
import { GoalStoreActions } from 'src/store/goal/actions.meta';
import { GoalStoreGetters } from 'src/store/goal/getters.meta';

export default {
  computed: {
    ...mapGetters({
      goalsGetter: GoalStoreGetters.getGoals,
    }),
  },
  methods: {
    ...mapActions({
      fetchGoalsAction: GoalStoreActions.fetchGoals,
      deleteGoalAction: GoalStoreActions.deleteGoal,
      updateGoalAction: GoalStoreActions.updateGoal,
      createGoalAction: GoalStoreActions.createGoal,
      closeGoalAction: GoalStoreActions.closeGoal,
      updateGoalAmountAction: GoalStoreActions.updateGoalAmount,
    }),
  },
};
