import router from "@/router";
// import { store } from "@/store";
// import { mapGetters, useStore } from "vuex";
import { utils } from "./utils";

router.beforeEach((to: any, from: any, next: any) => {
  next();
});

router.afterEach((to: any, from: any) => {});

router.onError((error: any) => {
  console.log("router.onError", error);
});
