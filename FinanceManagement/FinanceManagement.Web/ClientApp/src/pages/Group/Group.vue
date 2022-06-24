<template>
  <q-page>
    <q-tabs
      v-model="tab"
      dense
      class="text-grey"
      active-color="primary"
      indicator-color="primary"
      align="justify"
      narrow-indicator
    >
      <q-tab name="transactions" :label="$t('transaction.transactions')" />
      <q-tab name="users" :label="$t('group.groupUsers')" />
    </q-tabs>

    <div v-if="tab === 'transactions'">
      <GroupCard
          :key="group"
          :group="group"
          :goToEnabled="false"
          @add:transaction="refreshPage"
          @delete="goToGroupsPage"
          @edit="refreshPage"
        />
        <TransactionsTable
          :key="{ transactionRows, paginationForTable }"
          :transactions="transactionRows"
          ref="transactionTable"
          :on-delete="refreshPage"
          :on-edit="refreshPage"
          :on-request="fetchTransactionsPageWithTableLoading"
          :pagination="paginationForTable"
          :transaction-type="transactionTypePayload.transactionType"
          :parent-id="groupId"
        />
        <ComponentLoading
          :is-loading="localLoading"
        />
    </div>

    <div v-if="tab === 'users'">
      <q-btn
        flat
        icon-right="add"
        :label="$t('group.addUser')"
        v-if="isCurrentUserAdmin"
        color="primary"
      >
        <q-popup-proxy>
          <q-form @submit="addToGroup" class="q-pa-lg">
            <q-input v-model="addUserToGroupForm.userId" :label="$t('user.userId')"/>
            <q-select v-model="addUserToGroupForm.role"
                      :label="$t('user.userRole')"
                      :options="addUserToGroupForm.roleOptions"
            />
            <q-btn type="submit" color="primary" class="q-mt-md" label="OK"/>
          </q-form>
        </q-popup-proxy>
      </q-btn>
      <q-list>
        <q-item v-for="role in group.roles" :key="role.id">
          <q-item-section avatar>
            <q-icon name="star" color="yellow" v-if="isAdminRole(role.role)"/>
          </q-item-section>
          <q-item-section>
            <q-item-label>{{ role.userId }}</q-item-label>
            <q-item-label caption> {{ role.role }}</q-item-label>
          </q-item-section>

          <q-item-section side top>
            <div class="text-grey-8 q-gutter-xs">
              <q-btn
                class="gt-xs"
                size="12px"
                flat
                dense
                round
                icon="delete"
                v-if="isCurrentUserAdmin && role.userId !== userGetter.id"
                @click="deleteFromGroup(role.userId)"
              />
            </div>
          </q-item-section>
        </q-item>
      </q-list>
    </div>
    <ComponentLoading
      :is-loading="localLoading"
    />
  </q-page>
</template>

<script>
import currenciesNameMapMixin from 'src/mixins/resources/currenciesNameMapMixin';
import withLoadingAndErrorDialogMixin from 'src/mixins/decorators/withLoadingAndErrorDialogMixin';
import groupStoreMixin from 'src/mixins/store/groupStoreMixin';
import transactionStoreMixin from 'src/mixins/store/transactionStoreMixin';
import GroupCard from 'components/pages/groups/GroupCard';
import TransactionsTable from 'components/pages/account/TransactionsTable';
import { Routes } from 'src/router/routes';
import ComponentLoading from 'components/global/ComponentLoading';
import { TransactionStoreMeta } from 'src/store/transaction/store.meta';
import categoryStoreMixin from 'src/mixins/store/categoryStoreMixin';
import userStoreMixin from 'src/mixins/store/userStoreMixin';

export default {
  name: 'Group',
  components: { ComponentLoading, TransactionsTable, GroupCard },
  mixins: [
    currenciesNameMapMixin,
    withLoadingAndErrorDialogMixin,
    groupStoreMixin,
    transactionStoreMixin,
    categoryStoreMixin,
    userStoreMixin,
  ],
  data() {
    return {
      tab: 'transactions',
      transactionPagination: {
        page: 1,
        rowsPerPage: 5,
      },
      addUserToGroupForm: {
        role: 'Reader',
        roleOptions: [
          'Reader', 'Editor', 'Admin',
        ],
        userId: -1,
      },
      propsFromTable: {},
      transactionTypePayload: {
        transactionType: TransactionStoreMeta.Types.Group,
      },
      chosenUserId: -1,
    };
  },
  methods: {
    isAdminRole(role) {
      return role === 'Admin';
    },
    async fetchTransactionsPageWithTableLoading(props) {
      this.propsFromTable = {
        ...props,
      };
      await this.withComponentLoading(
        this.fetchTransactionsPage,
        this.$refs.transactionTable,
      );
    },
    async fetchTransactionsPage() {
      const { pagination } = this.propsFromTable;
      const { page, rowsPerPage } = pagination;
      this.transactionPagination = {
        page, rowsPerPage,
      };
      await this.fetchTransactions();

      const newPagination = {
        ...this.paginationForTable,
        page,
      };
      this.$refs.transactionTable.setPagination(newPagination);
    },
    async refreshPage() {
      await this.withLocalLoadingAndErrorDialog(this.fetchGroupAndTransactions);
    },
    async fetchGroupAndTransactions() {
      await Promise.all([
        this.fetchGroup(),
        this.fetchTransactions(),
        this.fetchCategoriesAction(),
      ]);
    },
    async fetchGroup() {
      await this.fetchGroupAction(this.groupId);
    },
    async fetchTransactions() {
      const { groupId } = this;
      await this.fetchTransactionsAction({
        ...this.transactionTypePayload,
        parentId: groupId,
        page: this.transactionPagination.page,
        pageSize: this.transactionPagination.rowsPerPage,
      });
    },
    async deleteFromGroup(userId) {
      this.chosenUserId = userId;
      this.$q.dialog({
        title: this.$t('confirm'),
        message: this.$t('transaction.areYouSureToDelete'),
        cancel: true,
        persistent: true,
      }).onOk(this.deleteFromGroupAndRefresh);
    },
    async deleteFromGroupAndRefresh() {
      const newGroup = {
        ...this.group,
        roles: this.group.roles.filter((r) => r.userId !== this.chosenUserId),
      };
      await this.updateGroupAction(newGroup);
      await this.refreshPage();
    },
    async addToGroup() {
      const { userId, role } = this.addUserToGroupForm;
      const newGroup = {
        ...this.group,
        roles: [
          ...this.group.roles,
          {
            userId,
            role,
            dateTo: '9999-12-31T23:59:59.999999',
          },
        ],
      };
      await this.updateGroupAction(newGroup);
      await this.refreshPage();
    },
    goToGroupsPage() {
      this.$router.push(Routes.groups);
    },
  },
  computed: {
    isCurrentUserAdmin() {
      return this.group.roles.find(
        (r) => r.userId === this.userGetter.id && this.isAdminRole(r.role),
      );
    },
    group() {
      return this.groupGetter;
    },
    groupId() {
      return this.$route.params.groupId;
    },
    transactionRows() {
      return this.transactionsGetter.map((t) => ({
        ...t,
        categoryName: this.categoriesGetter.find(({ id }) => id === t.categoryId)?.name,
      }));
    },
    paginationForTable() {
      return {
        // sortBy: 'desc',
        // descending: false,
        ...this.transactionPagination,
        rowsNumber: this.transactionTotalRowCountGetter,
      };
    },
  },
  async mounted() {
    await this.refreshPage();
  },
};
</script>
