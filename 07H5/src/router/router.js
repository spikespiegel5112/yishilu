import Vue from "vue";
import VueRouter from "vue-router";

Vue.use(VueRouter);

import Layout from "../views/Layout";
// import Auth from '../views/Auth'
// import Entrance from '../views/Entrance'
// import HomePage from '../views/HomePage'

let testPrefix = process.env.NODE_ENV === "production" ? "/" : "/test";
const routes = [
  {
    path: "/auth",
    component: () => import("../views/Auth.vue"),
  },
  {
    path: "/",
    redirect: "/entrance",
    component: Layout,
    children: [
      {
        path: "/Entrance",
        name: "Entrance",
        component: () => import("../views/Entrance.vue"),
      },
      {
        path: "/HomePage",
        name: "HomePage",
        component: () => import("../views/HomePage.vue"),
      },
      {
        path: "/Information",
        name: "Information",
        component: () => import("../views/Information.vue"),
      },
      {
        path: "/Customize",
        name: "Customize",
        component: () => import("../views/Customize.vue"),
      },
      {
        path: "/Interaction",
        name: "Interaction",
        component: () => import("../views/Interaction.vue"),
      },
      {
        path: "/InteractionLogo",
        name: "InteractionLogo",
        component: () => import("../views/InteractionLogo.vue"),
      },
      {
        path: "/Category",
        name: "Category",
        component: () => import("../views/Category.vue"),
      },
      {
        path: "/LotteryDraw",
        name: "LotteryDraw",
        component: () => import("../views/LotteryDraw.vue"),
      },
      {
        path: "/LotteryDrawLogo",
        name: "LotteryDrawLogo",
        component: () => import("../views/LotteryDrawLogo.vue"),
      },
    ],
  },
];

const router = new VueRouter({
  routes,
  // mode: 'history'
});

export default router;
