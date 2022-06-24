<template>
  <q-page>
    <h3 class="q-mt-none">Hello, {{ userGetter.name }}!</h3>
  </q-page>
</template>

<script>
import userStoreMixin from 'src/mixins/store/userStoreMixin';

export default {
  name: 'UserInfo',
  mixins: [userStoreMixin],
  async mounted() {
    this.$log.debug(this.userGetter);
  },
};
</script>
