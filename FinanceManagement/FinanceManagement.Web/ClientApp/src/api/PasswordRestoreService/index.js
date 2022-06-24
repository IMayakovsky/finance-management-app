import api from 'src/axios';
import { PasswordRestoreServiceMeta } from 'src/api/PasswordRestoreService/service.meta';

export const PasswordRestoreService = {
  createRestorePasswordRequest({ email, restorePasswordPageLink }) {
    return api.post(
      PasswordRestoreServiceMeta.ROUTES.CREATE_RESTORE_PASSWORD_REQUEST,
      null,
      { params: { email, link: restorePasswordPageLink } },
    );
  },

  createNewPassword({ newPassword, userRestoreId }) {
    return api.post(
      PasswordRestoreServiceMeta.ROUTES.CREATE_NEW_PASSWORD,
      { newPassword, hashId: userRestoreId },
    );
  },

  fetchUserRestoreIdValidity({ userRestoreId }) {
    return api.get(
      PasswordRestoreServiceMeta.ROUTES.FETCH_USER_RESTORE_ID_VALIDITY(userRestoreId),
    );
  },
};
