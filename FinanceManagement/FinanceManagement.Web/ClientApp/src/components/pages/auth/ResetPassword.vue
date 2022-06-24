<template>
  <section class="password-reset__form">
    <h2 class="bar-title"> New password</h2>

    <q-form
      @submit="handleSubmit"
    >
      <q-input
        filled
        type="password"
        v-model="newPassword"
        label="Password"
        lazy-rules
        class="q-pb-none q-mt-lg"
        :rules="[ val => val && val.length > 0]"
      ></q-input>

      <q-input
        filled
        type="password"
        v-model="newPasswordConfirm"
        label="Confirm password"
        class="q-pb-none q-mt-md"
        :rules="[ val => val && val.length > 0 && val === this.newPassword]"
      ></q-input>

      <q-card-section align="right" class="q-pa-none">
        <q-btn
          color="primary"
          type="submit"
          no-caps
          class="full-width big-text"
        >Save</q-btn>
      </q-card-section>
    </q-form>
  </section>
</template>

<script>
import { PasswordRestoreService } from 'src/api/PasswordRestoreService';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';

export default {
  name: 'ResetPassword',
  data() {
    return {
      newPassword: '',
      newPasswordConfirm: '',
    };
  },
  computed: {
    userRestoreId() {
      return this.$route.params.resetHash;
    },
  },
  mixins: [withLoadingAndErrorDialogMixin],
  // password restore methods are used there because they don't use store layer at all
  async mounted() {
    await this.withGlobalLoadingAndErrorDialog(
      () => PasswordRestoreService.fetchUserRestoreIdValidity(this),
    );
  },
  methods: {
    async handleSubmit() {
      await this.withGlobalLoadingAndErrorDialog(
        () => PasswordRestoreService.createNewPassword(this),
      );
    },
  },
};
</script>

<style scoped>

</style>
