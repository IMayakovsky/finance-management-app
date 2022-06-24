<template>
  <q-item-label caption>
    {{ $t('notification.newTransactionAppearedInGroup')}}
    <a @click="goToGroup" class="cursor-pointer text-primary">
      {{ group.name }}
    </a>
  </q-item-label>
</template>

<script>
export default {
  name: 'NewGroupTransactionNotification',
  props: {
    group: {
      type: Object,
      default: () => ({
        name: '',
        id: -1,
      }),
    },
  },
  methods: {
    async goToGroup() {
      if (this.$route.path !== this.groupLink) await this.$router.push(this.groupLink);
    },
  },
  computed: {
    groupLink() {
      return `/groups/${this.group.id}`;
    },
  },
};
</script>

<style scoped>

</style>
