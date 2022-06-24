/*
usage: node addNew.js storeName propertyPath getterName mutationName actionName
skip argument: node addNew.js storeName propertyPath - mutationName
example: node addNew.js user userId GetUserId SetUserId LoadUserName

storeName and propertyPath are required parameters!
you must add property to store manually
use it only if you doesn't have changes in current store files!
 */

const fs = require('fs');
const path = require('path');

const convertToPascalCase = (name) => name.split('_').map((word) => word.slice(0, 1).toUpperCase() + word.slice(1)).join('');

function appendToFile(pathToFile, pattern, appendedText) {
  fs.readFile(pathToFile, (err, data) => {
    if (err) throw err;
    const position = data.lastIndexOf(pattern);
    const fileContent = data.toString().substring(position);
    const file = fs.openSync(pathToFile, 'r+');
    const bufferedText = Buffer.from(appendedText + fileContent);
    fs.writeSync(file, bufferedText, 0, bufferedText.length, position);
    fs.close(file);
  });
}

function add(name, type, body) {
  const typePlural = `${type}s`;
  const typePascal = convertToPascalCase(type);
  const typePascalPlural = convertToPascalCase(typePlural);

  const typeTypePath = path.join(storePath, `${typePlural}.meta.js`);
  const typePath = path.join(storePath, `${typePlural}.js`);
  const data = fs.readFileSync(typeTypePath);
  if (data.indexOf(name) >= 0) {
    console.log(`Warning: ${typePascal} ${name} already exists`);
    return;
  }

  // append to {type}.type.js
  let newLine = `  ${name}: '${storeNamePascal}/${name}',\n`;
  appendToFile(typeTypePath, appendPositionPattern, newLine);

  // append to {type}.js
  const fullName = `${storeNamePascal}${typePascalPlural}.${name}`;
  newLine = `  [${fullName}]${body},\n`;
  appendToFile(typePath, appendPositionPattern, newLine);

  console.log(`${typePascal} ${name} was added`);
}

const appendPositionPattern = '};';

// storeName

const storeName = process.argv[2];
const storeNamePascal = convertToPascalCase(storeName);

if (!storeName) {
  console.log('storeName is a required argument');
  return;
}

const storePath = path.join(__dirname, storeName);

if (!fs.existsSync(storePath)) {
  console.log('Store not exists', storePath);
  return;
}

// propertyPath

const propertyPath = process.argv[3];

if (!propertyPath) {
  console.log('propertyPath is a required argument');
  return;
}

// getter

const getterName = process.argv[4];
if (!getterName) return;

if (getterName !== '-') {
  add(getterName, 'getter', `: (state) => state.${propertyPath}`);
}

// mutation

const mutationName = process.argv[5];

if (!mutationName) return;

const mutationFullName = mutationName !== '-' ? `${storeNamePascal}Mutations.${mutationName}` : undefined;

if (mutationName !== '-') {
  add(mutationName, 'mutation', `(state, newValue) {\n    state.${propertyPath} = newValue;\n  }`);
}

// action

const actionName = process.argv[6];

if (!actionName) return;

if (actionName !== '-') {
  const body = mutationFullName
    ? `(context, newValue) {\n    context.commit(${mutationFullName}, newValue);\n  }`
    : '(context, newValue) {\n \n  }';

  add(actionName, 'action', body);
}
