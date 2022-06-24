import { CrudService } from 'src/api/CrudService';
import { GoalServiceMeta } from 'src/api/GoalService/service.meta';

export const GoalService = {
  ...CrudService,
  ...GoalServiceMeta,
  async closeGoal({ goalId }) {
    await this.patchOne(goalId, 'close');
  },
  async changeAmount({ goalId, accountId, amount }) {
    await this.patchOne(goalId, 'amount', null, { accountId, amount });
  },
};
