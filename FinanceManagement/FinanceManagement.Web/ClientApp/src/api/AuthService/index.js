import { AuthServiceMeta } from 'src/api/AuthService/service.meta';
import api from 'src/axios';

export const AuthService = {
  signIn({ email, password, rememberMe }) {
    return api.post(AuthServiceMeta.ROUTES.SIGN_IN, { email, password, rememberMe });
  },

  signOut() {
    return api.get(AuthServiceMeta.ROUTES.SIGN_OUT);
  },

  register({ email, password, name }) {
    return api.post(AuthServiceMeta.ROUTES.REGISTER, { email, password, name });
  },
};
