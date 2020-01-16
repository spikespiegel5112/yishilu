import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

import Layout from '../components/Layout'

let testPrefix = process.env.NODE_ENV === 'production' ? '/' : '/test';
const routes = [{
  path: '/',
  redirect: '/entrance',
}, {
  path: '/entrance',
  redirect: '/entrance',
  name: 'entrance',
  component: Layout,
  children: [{
    path: '/',
    component: () => import('../components/Entrance.vue')
  }]
}, {
  path: '/auth',

  component: Layout,
  children: [{
    path: '/',
    name: 'auth',

    component: () => import('../components/Auth.vue')
  }]
}];


const router = new VueRouter({
  routes,
  // mode: 'history'
});


export default router;
