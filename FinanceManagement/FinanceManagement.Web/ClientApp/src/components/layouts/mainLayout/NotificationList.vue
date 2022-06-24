<template>
  <q-list padding>
    <q-item-label header>{{ $t('notification.notificationList') }}</q-item-label>
    <q-item v-for="notification in notifications" :key="notification.id">
      <q-item-section avatar>
        <q-badge color="blue" v-if="!notification.isRead">
        </q-badge>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ notification.title }}</q-item-label>
        <component
          :is="notification.component"
          v-bind="notification.payload"
        />
      </q-item-section>

      <q-item-section side top>
        <div class="text-grey-8 q-gutter-xs">
          <q-btn
            class="gt-xs"
            size="12px"
            flat
            dense
            round
            icon="delete"
            @click="deleteNotification(notification.id)"
          />
          <q-btn
            class="gt-xs"
            size="12px"
            flat
            dense
            round
            icon="done"
            @click="readNotification(notification.id)"
          />
        </div>
      </q-item-section>
    </q-item>
    <q-item v-if="!notifications.length">
        <q-item-section>
          {{ $t('notification.noNotifications')}}
        </q-item-section>
    </q-item>
  </q-list>
  <ComponentLoading
    :is-loading="localLoading"
  />
</template>

<script>
import notificationStoreMixin from 'src/mixins/store/notificationStoreMixin';
import NewGroupTransactionNotification
  from 'components/layouts/mainLayout/notifications/NewGroupTransactionNotification';
import groupStoreMixin from 'src/mixins/store/groupStoreMixin';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import ComponentLoading from 'components/global/ComponentLoading';

export default {
  name: 'NotificationList',
  mixins: [notificationStoreMixin, groupStoreMixin, withLoadingAndErrorDialogMixin],
  components: { ComponentLoading, NewGroupTransactionNotification },
  data() {
    return {
      notificationPagination: {
        page: 1,
        pageSize: 5,
      },
    };
  },
  async mounted() {
    await this.refreshNotifications();
  },
  methods: {
    async deleteNotification(id) {
      await this.deleteNotificationAction(id);
      await this.refreshNotifications();
    },
    async readNotification(id) {
      await this.updateNotificationAction(id);
      await this.refreshNotifications();
    },
    async refreshNotifications() {
      await this.withLocalLoadingAndErrorDialog(
        this.fetchRequiredData,
      );
    },
    async fetchRequiredData() {
      await this.fetchNotificationsAction(this.notificationPagination);
      await this.fetchGroupsAction();
    },
    generatePayload(name) {
      const generators = {
        GroupTransaction: (notificationParameters) => ({
          // eslint-disable-next-line max-len
          group: { ...this.groups.find(({ accountId }) => accountId === JSON.parse(notificationParameters).AccountId) },
        }),
      };
      return generators[name];
    },
  },
  computed: {
    notificationTitles() {
      return {
        GroupTransaction: this.$t('notification.newGroupTransaction'),
      };
    },
    notificationComponents() {
      return {
        GroupTransaction: NewGroupTransactionNotification,
      };
    },
    notifications() {
      return this.notificationsGetter.map(({
        id, name, parameters, isRead,
      }) => ({
        id,
        isRead,
        title: this.notificationTitles[name],
        component: this.notificationComponents[name],
        payload: this.generatePayload(name)(parameters),
      }));
    },
    groups() {
      return this.groupsGetter;
    },
  },
};
</script>

<style scoped>

</style>
