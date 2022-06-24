export const Routes = {
  root: '/',
  signIn: '/sign-in',
  signUp: '/sign-up',
  userInfo: '/user',
  accountActivation: '/activate/:activationHash/:userId',
  passwordReset: '/restore-password/:resetHash',
  accounts: '/accounts/',
  account: '/accounts/:accountId',
  groups: '/groups/',
  group: '/groups/:groupId',
  debts: '/debts/',
  goals: '/goals/',
  categories: '/categories/',
  subscriptions: '/subscriptions/',
  dashboard: '/dashboard',
};

export const routes = [
  {
    path: Routes.root,
    component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: '', component: () => import('pages/dashboard/Dashboard.vue'), alias: Routes.dashboard },
    ],
  },
  {
    path: Routes.accountActivation,
    component: () => import('layouts/MainLayout'),
    children: [{ path: '', name: Routes.accountActivation, component: () => import('pages/user/UserInfo') }],
  },
  {
    path: Routes.passwordReset,
    component: () => import('layouts/MainLayout'),
    children: [{ path: '', name: Routes.passwordReset, component: () => import('pages/user/Auth.vue') }],
  },
  {
    path: Routes.signIn,
    component: () => import('layouts/SignUpLayout'),
    children: [{ path: '', name: Routes.signIn, component: () => import('pages/user/Auth.vue') }],
  },
  {
    path: Routes.signUp,
    component: () => import('layouts/SignUpLayout'),
    children: [{ path: '', name: Routes.signUp, component: () => import('pages/user/Auth.vue') }],
  },
  {
    path: Routes.userInfo,
    component: () => import('layouts/MainLayout'),
    children: [{ path: '', name: Routes.userInfo, component: () => import('pages/user/UserInfo') }],
  },
  {
    path: Routes.accounts,
    component: () => import('layouts/MainLayout'),
    children: [{ path: '', name: Routes.accounts, component: () => import('pages/account/Accounts') }],
  },
  {
    path: Routes.account,
    component: () => import('layouts/MainLayout'),
    children: [{ path: '', name: Routes.account, component: () => import('pages/account/Account') }],
  },
  {
    path: Routes.groups,
    component: () => import('layouts/MainLayout'),
    children: [{ path: '', name: Routes.groups, component: () => import('pages/group/Groups') }],
  },
  {
    path: Routes.group,
    component: () => import('layouts/MainLayout'),
    children: [{ path: '', name: Routes.group, component: () => import('pages/group/Group') }],
  },
  {
    path: Routes.debts,
    component: () => import('layouts/MainLayout'),
    children: [{ path: '', name: Routes.debts, component: () => import('pages/debt/Debts') }],
  },
  {
    path: Routes.goals,
    component: () => import('layouts/MainLayout'),
    children: [{ path: '', name: Routes.goals, component: () => import('pages/goal/Goals') }],
  },
  {
    path: Routes.categories,
    component: () => import('layouts/MainLayout'),
    children: [{ path: '', name: Routes.categories, component: () => import('pages/category/Categories') }],
  },
  {
    path: Routes.subscriptions,
    component: () => import('layouts/MainLayout'),
    children: [{ path: '', name: Routes.subscriptions, component: () => import('pages/subscription/Subscriptions') }],
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/common/Error404.vue'),
  },
];
