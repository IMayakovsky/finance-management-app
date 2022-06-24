<template>
  <q-layout view="hHh Lpr fFf">
    <Header />
    <LeftBar />
    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import LeftBar from 'components/layouts/mainLayout/LeftBar';
import Header from 'components/layouts/mainLayout/Header';

export default {
  name: 'MainLayout',
  components: {
    Header,
    LeftBar,
  },
};
</script>
