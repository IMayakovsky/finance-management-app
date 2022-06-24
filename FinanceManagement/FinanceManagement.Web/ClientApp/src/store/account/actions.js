import { AccountStoreActions } from 'src/store/account/actions.meta';
import { AccountStoreMutations } from 'src/store/account/mutations.meta';
import { AccountService } from 'src/api/AccountService';

export const actions = {
  async [AccountStoreActions.fetchAccounts](context) {
    const { data: accounts } = await AccountService.fetchMany();
    context.commit(AccountStoreMutations.setAccounts, accounts);
  },
  async [AccountStoreActions.fetchAccount](context, accountId) {
    const { data: account } = await AccountService.fetchOne(accountId);
    context.commit(AccountStoreMutations.setAccount, account);
  },
  async [AccountStoreActions.createAccount](context, { name, currency, amount }) {
    const { data: account } = await AccountService.createOne({ name, currency, amount });
    return account;
  },
  async [AccountStoreActions.deleteAccount](context, accountId) {
    await AccountService.deleteOne(accountId);
  },
  async [AccountStoreActions.updateAccount](context, account) {
    await AccountService.updateOne(account.id, account);
  },
};
