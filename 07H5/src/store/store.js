import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

let testPrefix = process.env.NODE_ENV === "production" ? "production" : "test";

export const store = createStore({
  modules: {
    app,
    user,
  },
});
