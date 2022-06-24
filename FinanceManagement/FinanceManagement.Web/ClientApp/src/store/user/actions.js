import { UsersService } from 'src/api/UsersService';
import { UserStoreActions } from 'src/store/user/actions.meta';
import { UserStoreMutations } from 'src/store/user/mutations.meta';

export const actions = {
  [UserStoreActions.fetchCurrentUser]: async (context) => {
    const { data: user } = await UsersService.fetchCurrentUser();
    context.commit(UserStoreMutations.setUser, user);
  },
  [UserStoreActions.setCurrentUser]: async (context, user) => {
    context.commit(UserStoreMutations.setUser, user);
  },
};
