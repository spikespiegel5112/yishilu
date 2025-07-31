import {
  createRouter,
  createWebHistory,
  createWebHashHistory,
} from "vue-router";

import type { RouteRecordRaw } from "vue-router";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/auth",
    component: () => import(/* webpackChunkName: "auth" */ "@/views/Auth.vue"),
  },
  {
    path: "/",
    redirect: "/Entrance",
    component: () =>
      import(/* webpackChunkName: "Layout" */ "@/views/Layout.vue"),
    children: [
      {
        path: "/Entrance",
        name: "Entrance",
        component: () =>
          import(/* webpackChunkName: "Entrance" */ "@/views/Entrance.vue"),
      },
      {
        path: "/HomePage",
        name: "HomePage",
        component: () =>
          import(/* webpackChunkName: "HomePage" */ "@/views/HomePage.vue"),
      },
      {
        path: "/Information",
        name: "Information",
        component: () =>
          import(
            /* webpackChunkName: "Information" */ "@/views/Information.vue"
          ),
      },
      {
        path: "/Customize",
        name: "Customize",
        component: () =>
          import(/* webpackChunkName: "Customize" */ "@/views/Customize.vue"),
      },
      {
        path: "/Interaction",
        name: "Interaction",
        component: () =>
          import(
            /* webpackChunkName: "Interaction" */ "@/views/Interaction.vue"
          ),
      },
      {
        path: "/InteractionLogo",
        name: "InteractionLogo",
        component: () =>
          import(
            /* webpackChunkName: "InteractionLogo" */ "@/views/InteractionLogo.vue"
          ),
      },
      {
        path: "/Category",
        name: "Category",
        component: () =>
          import(/* webpackChunkName: "Category" */ "@/views/Category.vue"),
      },
      {
        path: "/LotteryDraw",
        name: "LotteryDraw",
        component: () =>
          import(
            /* webpackChunkName: "LotteryDraw" */ "@/views/LotteryDraw.vue"
          ),
      },
    ],
  },
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});
export default router;
