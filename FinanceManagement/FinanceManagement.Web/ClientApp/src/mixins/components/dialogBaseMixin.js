export default {
  // required api for modal window component to be called with $q.dialog
  // should be imported via mixins in the component where DialogBase is used as wrapper as well
  props: {
    onCancelHandler: {
      type: Function,
      default: () => null,
    },
    onOkHandler: {
      type: Function,
      default: () => null,
    },
  },

  emits: [
    // REQUIRED
    'ok', 'hide',
  ],

  methods: {
    // following method is REQUIRED
    // (don't change its name --> "show")
    show() {
      this.$refs.dialog.show();
    },

    // following method is REQUIRED
    // (don't change its name --> "hide")
    hide() {
      this.$refs.dialog.hide();
    },

    onDialogHide() {
      // required to be emitted
      // when QDialog emits "hide" event
      this.$emit('hide');
    },

    emitOK() {
      this.$emit('ok');
    },
    async onOKClick() {
      // on OK, it is REQUIRED to
      // emit "ok" event (with optional payload)
      // before hiding the QDialog
      try {
        await this.onOkHandler();
        this.emitOK();
        // or with payload: this.$emit('ok', { ... })
        // then hiding dialog
        this.hide();
      } finally {
        // skip
      }
    },
    async onCancelClick() {
      // we just need to hide the dialog
      await this.onCancelHandler();
      this.hide();
    },
  },
};
