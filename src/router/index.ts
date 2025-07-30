import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "Layout",
    component: () =>
      import(/* webpackChunkName: "Layout" */ "@/views/Layout.vue"),
    children: [
      {
        path: "/HomePage",
        name: "HomePage",
        component: () =>
          import(/* webpackChunkName: "HomePage" */ "../views/HomePage.vue"),
      },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
