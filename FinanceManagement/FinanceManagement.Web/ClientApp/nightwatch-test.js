describe('Sign in and create an account', () => {
  // test() and specify() is also available
  const addCategoryButtonSelector = '#q-app > div > div > main > button';
  const randomName = (Math.random() + 1).toString(36).substring(2);

  it('test', (browser) => {
    browser
      .url('http://localhost:8080/sign-in')
      .setValue('input[name=email]', 'snusmumrmail@gmail.com')
      .setValue('input[name=password]', '123')
      .click('button[type=submit]')
      .url('http://localhost:8080/categories')
      .click(addCategoryButtonSelector)
      .setValue(
        'input[name=category-name]',
        randomName,
      )
      .click('#dialog-base-modal-ok')
      .assert.containsText('#category-table', randomName)
      .end();
  });
});
