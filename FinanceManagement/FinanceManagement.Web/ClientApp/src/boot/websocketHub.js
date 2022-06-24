import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import LocalStorageApi, { localStorageFields } from 'src/utils/localStorage';

export default ({ app }) => {
  const buildHubConnection = (endpoint) => new HubConnectionBuilder()
    .withUrl(endpoint, {
      accessTokenFactory: () => `${LocalStorageApi.getRaw(localStorageFields.accessToken)}`,
    })
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Information)
    .build();

  app.config.globalProperties.$notificationHub = buildHubConnection('hub/notification');
};
