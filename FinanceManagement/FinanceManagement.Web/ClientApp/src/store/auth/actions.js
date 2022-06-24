import api from 'src/axios';
import { AuthService } from 'src/api/AuthService';
import LocalStorageApi, {
  localStorageFields,
  setAccessToken,
  unsetAccessToken,
} from 'src/utils/localStorage';
import { AuthStoreActions } from 'src/store/auth/actions.meta';
import { AuthStoreMutations } from 'src/store/auth/mutations.meta';
import { UserStoreActions } from 'src/store/user/actions.meta';
import { UserStoreMeta } from 'src/store/user/store.meta';

export const actions = {
  [AuthStoreActions.loadSessionFromLocalStorage]: async (context) => {
    const accessToken = LocalStorageApi.getRaw(localStorageFields.accessToken);
    if (accessToken) {
      context.commit(AuthStoreMutations.setAccessToken, accessToken);
      api.defaults.headers.common.Authorization = `Bearer ${accessToken}`;
      await context.dispatch(UserStoreActions.fetchCurrentUser);
      return;
    }

    context.commit(AuthStoreMutations.setAccessToken);
    await context.dispatch(
      UserStoreActions.setCurrentUser,
      { ...UserStoreMeta.getInitialState().user },
    );
  },

  [AuthStoreActions.signIn]: async (context, { email, password, rememberMe }) => {
    const { data } = await AuthService.signIn({ email, password, rememberMe });
    setAccessToken(data.accessToken);
    await context.dispatch(AuthStoreActions.loadSessionFromLocalStorage);
  },

  [AuthStoreActions.signOut]: async (context) => {
    unsetAccessToken();
    await context.dispatch(AuthStoreActions.loadSessionFromLocalStorage);
    await AuthService.signOut();
  },

  [AuthStoreActions.signUp]: async (context, { email, password, name }) => {
    await AuthService.register({ email, password, name });
  },
};
