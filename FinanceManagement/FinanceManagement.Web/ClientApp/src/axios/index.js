import axios from 'axios';
import store from 'src/store';
import { AuthStoreActions } from 'src/store/auth/actions.meta';
import { AuthServiceMeta } from 'src/api/AuthService/service.meta';

export const baseURL = process.env.VUE_APP_API_BASE_URL;

const api = axios.create({
  baseURL,
});

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const e = error;
    const originalRequest = e.config;
    if (
      e.response && e.response.status === 401
      && originalRequest.url !== AuthServiceMeta.ROUTES.SIGN_OUT
    ) {
      await store.dispatch(AuthStoreActions.signOut);
    }
    return Promise.reject(error);
  },
);

export default api;
