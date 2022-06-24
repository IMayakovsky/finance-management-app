<template>
  <section>
    <q-drawer
      v-model="leftDrawerOpen"
      show-if-above
      bordered
      :overlay="false"
      :breakpoint="0"
      :width="leftBarWidth"
    >
      <q-splitter
        v-model="splitterModel"
        class="full-height left-menu-splitter"
        separator-class="bg-transparent"
        disable
        unit="px"
      >
        <template v-slot:before>
          <div class="column justify-between full-height left-menu">
            <q-list class="left-menu-upper-list">
              <q-item-label header class="row items-center" style="padding-left: 12px">
                <img :src="logo" alt="">
                <p class="q-pa-none q-ma-none" style="font-size: 16px">FM App</p>
              </q-item-label>
              <MenuLink
                v-for="link in menuLinks"
                :title="link.title"
                :key="link.title"
                v-bind="link"
                class="medium-font"
              />
            </q-list>
            <div>
              <q-list class="left-menu-settings-list">
                <MenuLink
                  :title="$t('menu.logOut')"
                  icon="logout"
                  @click="handleLogOut"
                  class="medium-font flex items-center"
                />
              </q-list>
            </div>
          </div>
        </template>
      </q-splitter>
    </q-drawer>
  </section>
</template>

<script>
import MenuLink from 'components/layouts/mainLayout/MenuLink.vue';
import logo from 'src/assets/img/logo.png';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';

export default {
  name: 'LeftBar',
  components: {
    MenuLink,
  },
  mounted() {},
  data() {
    return {
      logo,
      splitterModel: 200,
      leftDrawerOpen: true,
      isInSettingsMenu: false,
    };
  },
  computed: {
    currentPagePath() {
      return this.$route.path;
    },
    menuLinks() {
      return [
        {
          title: this.$t('menu.dashboard'),
          link: '/',
        },
        {
          title: this.$t('menu.user'),
          link: '/user',
        },
        {
          title: this.$t('menu.accounts'),
          link: '/accounts',
        },
        {
          title: this.$t('menu.groups'),
          link: '/groups',
        },
        {
          title: this.$t('menu.debts'),
          link: '/debts',
        },
        {
          title: this.$t('menu.goals'),
          link: '/goals',
        },
        {
          title: this.$t('menu.categories'),
          link: '/categories',
        },
        {
          title: this.$t('menu.subscriptions'),
          link: '/subscriptions',
        },
      ];
    },
  },
  mixins: [withLoadingAndErrorDialogMixin],
  methods: {
  },
};
</script>

<style lang="scss" scoped>
.menu-settings {
  padding: 68px 0 0 21px;
  overflow: hidden;
}
.slideInLeft {
  animation-duration: 420ms;
}
.slideOutLeft {
  animation-duration: 420ms;
}
</style>
