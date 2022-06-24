export const LocalStorageApi = {
  setRaw(key, value) {
    localStorage.setItem(key, value);
  },

  getRaw(key) {
    const value = localStorage.getItem(key);
    return value || null;
  },

  setJson(key, value) {
    localStorage.setItem(key, JSON.stringify(value));
  },

  getJson(key) {
    const value = localStorage.getItem(key);
    return (value && JSON.parse(value)) || null;
  },

  remove(key) {
    localStorage.removeItem(key);
  },
};

export const localStorageFields = {
  accessToken: 'access_token',
};

export const setAccessToken = (accessToken) => {
  LocalStorageApi.setRaw(localStorageFields.accessToken, accessToken);
};
export const unsetAccessToken = () => {
  LocalStorageApi.remove(localStorageFields.accessToken);
};

export default LocalStorageApi;
