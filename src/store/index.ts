import type { InjectionKey } from "vue";
import app from "./modules/app";
import user from "./modules/user";

export const store = createStore({
  modules: {
    app,
    user,
  },
});
