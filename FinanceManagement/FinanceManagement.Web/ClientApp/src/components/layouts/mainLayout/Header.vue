<template>
  <q-header>
    <q-toolbar>
      <q-breadcrumbs active-color="white" style="font-size: 16px">
        <q-breadcrumbs-el
          :label="breadcrumb"
          v-for="(breadcrumb, idx) in breadcrumbs"
          :key="idx"
          :class="{
            'cursor-pointer': idx !== breadcrumbs.length - 1,
            'text-underline': idx !== breadcrumbs.length - 1,
          }"
          @click="goToBreadCrumb(idx)"
        />
      </q-breadcrumbs>
      <q-space />
      <div class="row" v-if="user.id > 0">
        <q-btn flat round dense icon="notifications" class="q-mr-xs" >
          <q-badge color="red" floating v-if="unreadNotificationsCount">
            {{ unreadNotificationsCount }}
          </q-badge>
          <q-popup-proxy>
            <NotificationList />
          </q-popup-proxy>
        </q-btn>
        <q-item dense>
          <q-item-section>
            {{ user.email }}
          </q-item-section>
        </q-item>
        <q-btn flat round dense icon="arrow_drop_down" class="q-ml-xs">
          <q-menu cover auto-close>
            <q-list>
              <q-item clickable @click="handleLogOut">
                <q-item-section>{{ $t("menu.logOut") }}</q-item-section>
              </q-item>
            </q-list>
          </q-menu>
        </q-btn>
      </div>
    </q-toolbar>
  </q-header>
</template>

<script>
import logo from 'assets/img/logo.png';
import { mapGetters } from 'vuex';
import { Routes } from 'src/router/routes';
import { AuthStoreActions } from 'src/store/auth/actions.meta';
import { UserStoreGetters } from 'src/store/user/getters.meta';
import NotificationList from 'components/layouts/mainLayout/NotificationList';
import notificationStoreMixin from 'src/mixins/store/notificationStoreMixin';
import userStoreMixin from 'src/mixins/store/userStoreMixin';

export default {
  name: 'Header',
  components: { NotificationList },
  data() {
    return {
      ROUTES: Routes,
      logo,
      username: null,
      isWorkingSpace: true,
    };
  },
  mixins: [notificationStoreMixin, userStoreMixin],
  computed: {
    ...mapGetters({
      user: UserStoreGetters.user,
    }),
    breadcrumbs() {
      const { path } = this.$route;
      return path !== '/' ? path.split('/') : [];
    },
    unreadNotificationsCount() {
      return this.notificationsGetter.filter(({ isRead }) => !isRead).length;
    },
  },
  methods: {
    handleLogOut() {
      this.$store.dispatch(AuthStoreActions.signOut);
      this.$router.push(Routes.signIn);
    },
    goToBreadCrumb(idx) {
      if (idx === this.breadcrumbs.length - 1) return;
      this.$router.push(
        idx ? this.breadcrumbs.slice(0, idx + 1).join('/') : '/',
      );
    },
  },
};
</script>

<style lang="scss">
@import '../../../css/quasar.variables';

.header__menu {
  background-color: #eee;
}

.header {
  height: 70px;
  background-color: transparent;

  &__logo {
    height: 100%;
    button {
      color: $primary;
      margin: 0 0 0 33px;
      text-decoration: none;
      transition: all 0.2s linear;
      border: none;
      background-color: transparent;
      cursor: pointer;

      i {
        font-size: 24px;
        margin: 0 6px 0 0;
      }
    }

    button:hover {
      color: $primary;
      transition: all 0.2s linear;
    }
  }

  &__user {
    height: 100%;
    .username {
      font-size: 20px;
      font-weight: 600;
      color: $primary;
    }
  }
}
</style>
