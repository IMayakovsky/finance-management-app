<template>
  <q-layout view="hHh Lpr fFf">
    <Header />
    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import Header from 'components/layouts/mainLayout/Header';

export default {
  name: 'SignUpLayout',
  components: {
    Header,
  },
};
</script>
