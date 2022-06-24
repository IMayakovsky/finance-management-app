import { createStore } from 'vuex';
import auth from './auth';
import user from './user';
import globalOperations from './globalOperations';
import account from './account';
import transaction from './transaction';
import group from './group';
import debt from './debt';
import goal from './goal';
import category from './category';
import notification from './notification';
import subscription from './subscription';

// import example from './module-example'

/*
 * If not building with SSR mode, you can
 * directly export the Store instantiation;
 *
 * The function below can be async too; either use
 * async/await or return a Promise which resolves
 * with the Store instance.
 */

const Store = createStore({
  modules: {
    auth,
    globalOperations,
    user,
    account,
    transaction,
    group,
    debt,
    goal,
    category,
    notification,
    subscription,
  },

  // enable strict mode (adds overhead!)
  // for dev mode and --debug builds only
  strict: process.env.DEBUGGING,
});

export default Store;
