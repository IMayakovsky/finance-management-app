<template>
<div>
  <section class="auth__sign-up">
    <h2 class="bar-title">Sign up</h2>
    <q-form
      @submit="handleSubmit"
    >

      <q-separator />

      <q-input
        filled
        type="email"
        v-model="email"
        label="Email"
        class="q-pb-none"
        :rules="[ val => val && val.length > 0]"
      ></q-input>

      <q-input
        filled
        v-model="name"
        label="Full name"
        lazy-rules
        class="q-pb-none"
        :rules="[ val => val && val.length > 0]"
      ></q-input>

      <q-separator />

      <q-input
        filled
        type="password"
        v-model="password"
        label="Password"
        lazy-rules
        class="q-pb-none"
        :rules="[
          val => val !== null && val !== '',
        ]"
      ></q-input>

      <q-input
        filled
        type="password"
        v-model="passwordConfirm"
        label="Confirm password"
        lazy-rules
        class="q-pb-none q-mt-md"
        :rules="[ val => val && val.length > 0 && val === this.password]"
      ></q-input>

      <div>
        <q-btn
          label="Create Account"
          type="submit"
          color="primary"
          no-caps
          class="big-text full-width reg-btn"
        ></q-btn>
      </div>

      <p
        class="tiny-text q-mb-none sign-up-links"
      >
        {{ $t("signUp.alreadyAMember") }}
        <router-link :to="Routes.signIn">{{ $t("signUp.signInNow") }}</router-link></p>
    </q-form>
  </section>
</div>
</template>

<script>
import { Routes } from 'src/router/routes';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import userStoreMixin from 'src/mixins/store/userStoreMixin';

export default {
  name: 'SignUp',
  data() {
    return {
      Routes,
      email: '',
      name: '',
      password: '',
      passwordConfirm: '',
    };
  },
  mixins: [withLoadingAndErrorDialogMixin, userStoreMixin],
  computed: {
  },
  methods: {
    async signUpAndShowConfirmationDialog() {
      await this.signUpAction(this);
      this.$q.dialog({
        title: this.$t('confirm'),
        message: this.$t('confirmRegistration'),
        persistent: true,
      }).onOk(() => {
        this.$router.push(Routes.signIn);
      });
    },
    async handleSubmit() {
      await this.withGlobalLoadingAndErrorDialog(this.signUpAndShowConfirmationDialog);
    },
  },
};
</script>

<style lang="scss">
.reg-btn {
  margin: 12px 0 0 0;
}
</style>
