import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

import Layout from '../components/Layout'
import Entrance from '../components/Entrance'
import HomePage from '../components/HomePage'

let testPrefix = process.env.NODE_ENV === 'production' ? '/' : '/test';
const routes = [{
  path: '/',
  redirect: '/homepage',
  component: Layout,
  children: [{
    path: '/entrance',
    name:'entrance',
    component: () => import('../components/Entrance.vue')
  }, {
    path: '/homepage',
    name:'homePage',
    component: () => import('../components/HomePage.vue')
  }, {
    path: '/information',
    name:'information',
    component: () => import('../components/Information.vue')
  }, {
    path: '/customize',
    name:'customize',
    component: () => import('../components/Customize.vue')
  }, {
    path: '/interaction',
    name:'interaction',
    component: () => import('../components/Interaction.vue')
  }, {
    path: '/lotterydraw',
    name:'lotteryDraw',
    component: () => import('../components/LotteryDraw.vue')
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
