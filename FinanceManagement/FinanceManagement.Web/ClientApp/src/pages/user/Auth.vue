<template>
  <q-page>
    <div class="auth">
      <div class="auth-wrapper full-width flex justify-center items-center">
        <SignIn v-if="route.name === ROUTES.signIn" />
        <SignUp v-if="route.name === ROUTES.signUp" />
        <ResetPassword v-if="route.name === ROUTES.passwordReset" />
      </div>
    </div>
  </q-page>
</template>

<script>
import SignIn from 'components/pages/auth/SignIn.vue';
import SignUp from 'components/pages/auth/SignUp.vue';
import { Routes } from 'src/router/routes';
import ResetPassword from 'components/pages/auth/ResetPassword';
import userStoreMixin from 'src/mixins/store/userStoreMixin';

export default {
  name: 'Auth',
  mixins: [userStoreMixin],
  components: {
    ResetPassword,
    SignIn,
    SignUp,
  },
  data() {
    return {
      ROUTES: Routes,
    };
  },
  computed: {
    route() {
      return this.$route;
    },
  },
};
</script>
<style lang="scss">
.auth-wrapper {
  height: calc(100vh - 70px);

  .auth__sign-in,
  .auth__sign-up,
  .password-reset__form {
    width: 397px;
    padding: 48px 48px 96px 48px;
    background-color: #eee;

    .q-separator {
      margin: 0 0 12px 0;
    }

    h2 {
      margin: 0;
    }

    button {
      height: 54px;
    }
  }

  .q-form {
    margin-top: 24px;

    .q-field {
      margin: 0 0 12px 0;
    }

    .q-field:last-child {
      margin: 0;
    }
  }

  .reset-pass {
    text-decoration: underline;
  }

  .sign-up-links a {
    cursor: pointer;
  }
}

</style>
