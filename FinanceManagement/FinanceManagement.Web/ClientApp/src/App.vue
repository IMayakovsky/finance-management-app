<template>
  <router-view />
  <ComponentLoading
    :is-loading="globalLoadingIsActive"
  />
</template>
<script>
import { defineComponent } from 'vue';
import { mapActions, mapGetters } from 'vuex';
import { GlobalOperationsStoreGetters } from 'src/store/globalOperations/getters.meta';
import { AuthStoreActions } from 'src/store/auth/actions.meta';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import ComponentLoading from 'components/global/ComponentLoading';
import notificationStoreMixin from 'src/mixins/store/notificationStoreMixin';
import { UserStoreGetters } from 'src/store/user/getters.meta';

export default defineComponent({
  name: 'App',
  components: { ComponentLoading },
  computed: {
    ...mapGetters({
      globalLoadingIsActive: GlobalOperationsStoreGetters.globalLoading,
      user: UserStoreGetters.user,
    }),
  },
  methods: {
    ...mapActions({
      loadSession: AuthStoreActions.loadSessionFromLocalStorage,
    }),
    async initializeHub() {
      await this.$notificationHub.start();
      this.$notificationHub.on('SendNotification', async () => {
        await this.fetchNotificationsAction(this.notificationPagination);
        this.$q.notify({
          message: 'notification.newNotification',
          caption: 'notification.rightNow',
          color: 'positive',
        });
      });
    },
    async initializeApplication() {
      await this.loadSession();
      await this.initializeHub();
      if (this.user?.id > 0) {
        await this.fetchNotificationsAction(this.notificationPagination);
      }
    },
  },
  data() {
    return {
      notificationPagination: {
        page: 1,
        pageSize: 5,
      },
    };
  },
  mixins: [withLoadingAndErrorDialogMixin, notificationStoreMixin],
  async mounted() {
    await this.withGlobalLoadingAndErrorDialog(this.initializeApplication);
  },
});
</script>
