<template>
  <q-inner-loading :showing="isLoading">
    <q-spinner-oval size="150px" color="primary" />
  </q-inner-loading>
</template>

<script>

export default {
  name: 'ComponentLoading',
  props: {
    isLoading: {
      type: Boolean,
      default: true,
    },
  },
};
</script>

<style scoped>

</style>
