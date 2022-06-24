<template>
  <section class="auth__sign-in">
    <h2 class="bar-title">Sign in</h2>

    <q-form
      @submit="handleSubmit"
    >
      <q-input
        filled
        type="email"
        name="email"
        v-model="email"
        label="Email"
        lazy-rules
        class="q-pb-none"
        :rules="[ val => val && val.length > 0]"
      ></q-input>

      <q-input
        filled
        type="password"
        name="password"
        v-model="password"
        label="Password"
        lazy-rules
        class="q-pb-none"
        :rules="[
          val => val !== null && val !== '',
        ]"
      ></q-input>

      <a
        class="tiny-text reset-pass cursor-pointer"
        @click.prevent="openResetPasswordDialog"
      >
        {{ $t("signUp.forgotPassword") }}
      </a>

      <div>
        <q-btn
          label="Log in"
          type="submit"
          color="primary"
          no-caps
          class="big-text full-width"
        ></q-btn>
      </div>

      <p
        class="tiny-text q-mb-none sign-up-links"
      >
        {{ $t("signUp.notAMember") }}
      <router-link :to="Routes.signUp">{{ $t("signUp.signUpNow") }}</router-link></p>
    </q-form>
  </section>
</template>

<script>
import { Routes } from 'src/router/routes';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import { validateEmailRegex } from 'src/utils/common';
import { PasswordRestoreService } from 'src/api/PasswordRestoreService';
import userStoreMixin from 'src/mixins/store/userStoreMixin';

export default {
  name: 'SignIn',
  data() {
    return {
      Routes,
      email: '',
      password: '',
      // used to generate link on backend, because frontend path view can change
      restorePasswordPageLink: (
        `${window.location.protocol}//${window.location.host}${Routes.passwordReset.split('/').slice(0, -1).join('/')}`
      ),
    };
  },
  mixins: [withLoadingAndErrorDialogMixin, userStoreMixin],
  methods: {
    async handleSubmit() {
      await this.withGlobalLoadingAndErrorDialog(this.signInWithInputData);
    },
    openResetPasswordDialog() {
      this.$q.dialog({
        title: this.$t('signUp.email'),
        message: this.$t('signUp.typeInYourEmail'),
        prompt: {
          model: this.email,
          isValid: validateEmailRegex,
          type: 'text', // optional
        },
        cancel: true,
        persistent: true,
      }).onOk(this.handleReset);
    },
    async signInWithInputData() {
      const { email, password, rememberMe } = this;
      await this.signInAction({ email, password, rememberMe });
      await this.$router.push(Routes.userInfo);
    },
    async handleReset(email) {
      await this.withGlobalLoadingAndErrorDialog(
        () => this.sendRestorePasswordRequest(email),
      );
    },
    async sendRestorePasswordRequest(email) {
      // password restore methods are used there because they don't use store layer at all
      const { restorePasswordPageLink } = this;
      await PasswordRestoreService.createRestorePasswordRequest({ restorePasswordPageLink, email });
    },
  },
};
</script>

<style lang="scss" scoped>
.reset-pass {
  display: block;
  margin: 16px 0 24px 0;
}
</style>
