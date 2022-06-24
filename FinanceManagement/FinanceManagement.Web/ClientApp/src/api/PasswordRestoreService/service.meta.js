export const PasswordRestoreServiceMeta = {
  ROUTES: {
    CREATE_NEW_PASSWORD: '/restore/password',
    CREATE_RESTORE_PASSWORD_REQUEST: '/restore/password/request',
    FETCH_USER_RESTORE_ID_VALIDITY: (userRestoreId) => `/restore/password/${userRestoreId}/valid`,
  },
};
