<template>
  <div
    class="col-4 q-pa-md"
  >
    <q-card
      flat
      bordered
      class="account-card bg-grey-1"
    >
      <q-card-actions align="between">
        <q-btn
          flat
          icon-right="add"
          :label="$t('account.income')"
          color="primary"
          @click="handleAddTransaction"
        />
      </q-card-actions>
      <q-card-section>
        <div class="row items-center no-wrap">
          <div class="col">
            <div class="text-h6">
              {{ account.name }}
              <q-popup-edit auto-save v-slot="scope">
                <q-input
                  v-model="accountForm.name"
                  dense autofocus counter
                  @keyup.enter="handleSaveAccountName(scope)"
                />
              </q-popup-edit>
            </div>
            <div class="text-subtitle2">{{ account.code }}</div>
          </div>

          <div class="col-auto">
            <q-btn color="grey-7" round flat icon="more_vert">
              <q-menu cover auto-close>
                <q-list>
                  <q-item clickable @click="handleDeleteAccount">
                    <q-item-section>{{ $t("account.delete") }}</q-item-section>
                  </q-item>
                </q-list>
              </q-menu>
            </q-btn>
          </div>
        </div>
      </q-card-section>

      <q-card-section>
        {{ account.amount }} {{ account.currency }}
      </q-card-section>

      <q-separator />

      <q-card-actions v-if="goToEnabled">
        <q-btn
          color="primary"
          icon-right="arrow_forward"
          flat @click="handleGoToAccount"
        >{{ $t("account.goToAccount") }}</q-btn>
      </q-card-actions>
      <ComponentLoading
        :is-loading="localLoading"
      />
    </q-card>
  </div>
</template>

<script>
import currenciesNameMapMixin from 'src/mixins/resources/currenciesNameMapMixin';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import accountStoreMixin from 'src/mixins/store/accountStoreMixin';
import transactionStoreMixin from 'src/mixins/store/transactionStoreMixin';
// import AddTransactionPopup from 'components/pages/accounts/AddTransactionPopup';
import ComponentLoading from 'components/global/ComponentLoading';
import CreateOrUpdateTransactionDialog from 'components/pages/account/CreateOrUpdateTransactionDialog';
import { TransactionStoreMeta } from 'src/store/transaction/store.meta';
import { OkActionEnum } from 'src/common/enums/components/okActions';

export default {
  name: 'AccountCard',
  components: { ComponentLoading },
  mixins: [
    currenciesNameMapMixin,
    withLoadingAndErrorDialogMixin,
    accountStoreMixin,
    transactionStoreMixin,
  ],
  props: {
    goToEnabled: {
      default: true,
      type: Boolean,
    },
    account: {
      id: Number,
      code: String,
      currency: Number,
      name: String,
      userId: Number,
      amount: Number,
    },
  },
  data() {
    return {
      accountForm: {
        name: this.account.name,
      },
    };
  },
  methods: {
    async handleAddTransaction() {
      this.$q.dialog({
        component: CreateOrUpdateTransactionDialog,
        componentProps: {
          parentId: this.account.id,
          transactionType: TransactionStoreMeta.Types.Account,
          okAction: OkActionEnum.Create,
        },
      }).onOk(() => {
        this.$emit('add:transaction');
      });
    },
    async handleDeleteAccount() {
      await this.withLocalLoadingAndErrorDialog(this.deleteAccount);
      this.$emit('delete');
    },
    handleGoToAccount() {
      this.$router.push(`${this.$route.path}/${this.account.id}`);
    },
    async handleSaveAccountName(scope) {
      scope.set();
      await this.withLocalLoadingAndErrorDialog(this.updateAccountName);
      this.$emit('edit');
    },
    async deleteAccount() {
      await this.deleteAccountAction(this.account.id);
    },
    async updateAccountName() {
      await this.updateAccountAction({
        ...this.account,
        name: this.accountForm.name,
      });
    },
  },
};
</script>

<style scoped>

</style>
