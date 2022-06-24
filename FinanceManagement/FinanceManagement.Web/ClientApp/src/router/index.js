import { route } from 'quasar/wrappers';
import {
  createRouter, createMemoryHistory, createWebHistory, createWebHashHistory,
} from 'vue-router';
import LocalStorageApi, { localStorageFields } from 'src/utils/localStorage';
import { Routes, routes } from './routes';

/*
 * If not building with SSR mode, you can
 * directly export the Router instantiation;
 *
 * The function below can be async too; either use
 * async/await or return a Promise which resolves
 * with the Router instance.
 */

export default route((/* { store, ssrContext } */) => {
  const createHistory = process.env.SERVER
    ? createMemoryHistory
    : (process.env.VUE_ROUTER_MODE === 'history' ? createWebHistory : createWebHashHistory);

  const Router = createRouter({
    scrollBehavior: () => ({ left: 0, top: 0 }),
    routes,

    // Leave this as is and make changes in quasar.conf.js instead!
    // quasar.conf.js -> build -> vueRouterMode
    // quasar.conf.js -> build -> publicPath
    history: createHistory(process.env.MODE === 'ssr' ? void 0 : process.env.VUE_ROUTER_BASE),
  });

  // redirect from sign up routes to root path
  const authRoutes = new Set([Routes.signIn, Routes.signUp]);
  Router.beforeEach((to) => {
    const accessToken = LocalStorageApi.getRaw(localStorageFields.accessToken);
    if (authRoutes.has(to.path) && accessToken) {
      return {
        path: Routes.root,
      };
    }
    if (!authRoutes.has(to.path) && !accessToken) {
      return {
        path: Routes.signIn,
      };
    }

    return true;
  });

  return Router;
});
