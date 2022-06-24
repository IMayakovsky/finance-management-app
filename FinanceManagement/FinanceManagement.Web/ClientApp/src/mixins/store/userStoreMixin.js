import { mapActions, mapGetters } from 'vuex';
import { UserStoreActions } from 'src/store/user/actions.meta';
import { UserStoreGetters } from 'src/store/user/getters.meta';
import { AuthStoreActions } from 'src/store/auth/actions.meta';
import { Routes } from 'src/router/routes';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';

export default {
  computed: {
    ...mapGetters({
      userGetter: UserStoreGetters.user,
    }),
  },
  mixins: [withLoadingAndErrorDialogMixin],
  methods: {
    ...mapActions({
      fetchCurrentUserAction: UserStoreActions.fetchCurrentUser,
      setCurrentUserAction: UserStoreActions.setCurrentUser,
      signInAction: AuthStoreActions.signIn,
      signUpAction: AuthStoreActions.signUp,
      signOutAction: AuthStoreActions.signOut,
      loadSessionFromLocalStorageAction: AuthStoreActions.loadSessionFromLocalStorage,
    }),
    // used for two buttons
    async signOutAndRedirectToSignInPage() {
      await this.signOutAction();
      await this.$router.push(Routes.signIn);
    },
    async handleLogOut() {
      await this.withGlobalLoadingAndErrorDialog(
        this.signOutAndRedirectToSignInPage,
      );
    },
  },
};
