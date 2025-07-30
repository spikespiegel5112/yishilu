import Vue from "vue";
import VueRouter from "vue-router";

Vue.use(VueRouter);

import Layout from "../components/Layout";
// import Auth from '../components/Auth'
// import Entrance from '../components/Entrance'
// import HomePage from '../components/HomePage'

let testPrefix = process.env.NODE_ENV === "production" ? "/" : "/test";
const routes = [
  {
    path: "/auth",
    component: () => import("../components/Auth.vue"),
  },
  {
    path: "/",
    redirect: "/entrance",
    component: Layout,
    children: [
      {
        path: "/entrance",
        name: "entrance",
        component: () => import("../components/Entrance.vue"),
      },
      {
        path: "/homepage",
        name: "homePage",
        component: () => import("../components/HomePage.vue"),
      },
      {
        path: "/information",
        name: "information",
        component: () => import("../components/Information.vue"),
      },
      {
        path: "/customize",
        name: "customize",
        component: () => import("../components/Customize.vue"),
      },
      {
        path: "/interaction",
        name: "interaction",
        component: () => import("../components/Interaction.vue"),
      },
      {
        path: "/interactionlogo",
        name: "interactionLogo",
        component: () => import("../components/InteractionLogo.vue"),
      },
      {
        path: "/category",
        name: "category",
        component: () => import("../components/Category.vue"),
      },
      {
        path: "/lotterydraw",
        name: "lotteryDraw",
        component: () => import("../components/LotteryDraw.vue"),
      },
      {
        path: "/lotterydrawlogo",
        name: "lotteryDrawLogo",
        component: () => import("../components/LotteryDrawLogo.vue"),
      },
    ],
  },
];

const router = new VueRouter({
  routes,
  // mode: 'history'
});

export default router;
