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
  path: '/edit',
  component: Layout,
  children: [{
    path: '',
    name: 'edit',

    component: () => import('../components/Edit.vue')
  }]
}, {
  path: '/myflag',
  component: Layout,
  children: [{
    path: '',
    name: 'myFlag',

    component: () => import('../components/MyFlag.vue')
  }]
}, {
  path: '/poster',
  component: Layout,
  children: [{
    path: '',
    name: 'poster',

    component: () => import('../components/Poster.vue')
  }]
}, {
  path: '/posterOld',
  component: Layout,
  children: [{
    path: '',
    name: 'posterOld',

    component: () => import('../components/Poster.vue')
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
