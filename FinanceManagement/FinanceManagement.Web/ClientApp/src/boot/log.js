import { LogLevelEnum } from 'src/common/enums/logLevelEnum';
import { boot } from 'quasar/wrappers';

const log = {
  install(app) {
    app.config.globalProperties.$log = this;
  },
  minLogLevel: LogLevelEnum.Debug,
  debug(data, logType) {
    if (this.canLog(LogLevelEnum.Debug)) {
      // eslint-disable-next-line no-console
      console.debug(logType ?? 'LOG', data);
    }
  },
  trace(data, logType) {
    if (this.canLog(LogLevelEnum.Trace)) {
      // eslint-disable-next-line no-console
      console.trace(logType ?? 'TRACE', data);
    }
  },
  info(data, logType) {
    if (this.canLog(LogLevelEnum.Info)) {
      // eslint-disable-next-line no-console
      console.info(logType ?? 'INFO', data);
    }
  },
  warn(data, logType) {
    if (this.canLog(LogLevelEnum.Warning)) {
      // eslint-disable-next-line no-console
      console.warn(logType ?? 'WARNING', data);
    }
  },
  error(data, logType) {
    if (this.canLog(LogLevelEnum.Error)) {
      // eslint-disable-next-line no-console
      console.error(logType ?? 'ERROR', data, data.response);
    }
  },
  canLog(logLevel) {
    return logLevel >= this.minLogLevel;
  },
};

export default boot(({ app }) => {
  // Set log instance on app
  app.use(log);
});

export { log };
