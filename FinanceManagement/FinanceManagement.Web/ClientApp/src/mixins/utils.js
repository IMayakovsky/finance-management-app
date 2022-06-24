export const reduceMixins = (mixinList, procedure) => mixinList.reduce(
  (currentState, nextMixin) => (() => nextMixin(currentState)), procedure,
);
