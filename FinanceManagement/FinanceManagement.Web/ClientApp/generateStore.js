// usage: node generateStore.js storeName
// Also supports store_name, StoreName
// used to generate a store module based on a boilerplate structure

const fs = require('fs');
const path = require('path');

const convertRawStoreNameToPascalCase = (name) => name
  .split('_')
  .map((word) => word.slice(0, 1).toUpperCase() + word.slice(1))
  .join('');

const convertRawStoreNameToCamelCase = (name) => {
  const tmp = convertRawStoreNameToPascalCase(name);
  return tmp.slice(0, 1).toLowerCase() + tmp.slice(1);
};

const storeNameRaw = process.argv[2];
const storeNamePascal = convertRawStoreNameToPascalCase(storeNameRaw);
const storeNameCamel = convertRawStoreNameToCamelCase(storeNameRaw);

const storePath = path.join(__dirname, 'src', 'store', storeNameCamel);
const actionsJsPath = path.join(storePath, 'actions.js');
const gettersJsPath = path.join(storePath, 'getters.js');
const mutationsJsPath = path.join(storePath, 'mutations.js');
const indexJsPath = path.join(storePath, 'index.js');

const actionsMetaJsPath = path.join(storePath, 'actions.meta.js');
const gettersMetaJsPath = path.join(storePath, 'getters.meta.js');
const mutationsMetaJsPath = path.join(storePath, 'mutations.meta.js');
const storeMetaJsPath = path.join(storePath, 'store.meta.js');
const utilsJsPath = path.join(storePath, 'utils.js');

fs.mkdirSync(storePath, { recursive: true });

const actionsTemplate = 'export const actions = {\n};\n';
fs.writeFileSync(
  actionsJsPath,
  actionsTemplate,
);

const actionsMetaTemplate = `export const ${storeNamePascal}Actions = {\n};\n`;
fs.writeFileSync(
  actionsMetaJsPath,
  actionsMetaTemplate,
);

const gettersTemplate = 'export const getters = {\n};\n';
fs.writeFileSync(
  gettersJsPath,
  gettersTemplate,
);

const gettersMetaTemplate = `export const ${storeNamePascal}Getters = {\n};\n`;
fs.writeFileSync(
  gettersMetaJsPath,
  gettersMetaTemplate,
);

const mutationsTemplate = 'export const mutations = {\n};\n';
fs.writeFileSync(
  mutationsJsPath,
  mutationsTemplate,
);

const mutationsMetaTemplate = `export const ${storeNamePascal}Mutations = {\n};\n`;
fs.writeFileSync(
  mutationsMetaJsPath,
  mutationsMetaTemplate,
);

const indexJsTemplate = `import { ${storeNamePascal}Meta } from 'src/store/${storeNameCamel}/store.meta';
import { getters } from './getters';
import { mutations } from './mutations';
import { actions } from './actions';

const state = {
  ...${storeNamePascal}Meta.getInitialState(),
};

export default {
  state,
  getters,
  mutations,
  actions,
};\n`;
fs.writeFileSync(
  indexJsPath,
  indexJsTemplate,
);

const storeMetaJsTemplate = `export const ${storeNamePascal}Meta = {
  getInitialState: () => ({}),
};\n`;
fs.writeFileSync(
  storeMetaJsPath,
  storeMetaJsTemplate,
);

const utilsJsTemplate = '';
fs.writeFileSync(
  utilsJsPath,
  utilsJsTemplate,
);
