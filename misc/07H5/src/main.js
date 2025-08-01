// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from "vue";
import FastClick from "fastclick";
// import VueRouter from 'vue-router'
// import Vuex from 'vuex'
import wxsdk from "./js/wxsdk";

// import axios from 'axios'
import moment from "moment";
// import { ConfirmPlugin, LoadingPlugin, ToastPlugin } from 'vux'
// import Cookies from 'js-cookie'

import App from "./App";
import router from "./router/router";
import service from "./js/request";
import util from "./js/util";
import { baseUrl } from "./js/request";

import store from "./store/store";
import CommonGoBack from "./components/common/CommonGoBack.vue";
import CommonLoading from "./components/common/CommonLoading.vue";

// Vue.use(ConfirmPlugin)
// Vue.use(LoadingPlugin)
// Vue.use(ToastPlugin)
// Vue.use(Popup)
//
// Vue.use(Cookies)
Vue.use(util);

moment.locale("zh-cn");
FastClick.attach(document.body);

Vue.config.productionTip = false;
Vue.prototype.$moment = moment;
Vue.prototype.$http = service;
// Vue.prototype.$baobaoAudioPlayback = BaobaoAudioPlayback;

Vue.prototype.$baseUrl = baseUrl;
// Vue.prototype.$domainUrl = process.env.NODE_ENV === 'production' ? 'http://jgxzq.pengpai.nplusgroup.com/production/index.html' : 'http://jgxzq.pengpai.nplusgroup.com/test/index.html:81';

Vue.prototype.$prodEnv = process.env.NODE_ENV === "production";

Vue.component("CommonGoBack", CommonGoBack);
Vue.component("CommonLoading", CommonLoading);

Vue.prototype.$webStorage.type = "localStorage";
util.$webStorage.type = "localStorage";

Vue.prototype.$wxsdk = wxsdk;

// Vue.prototype.$webStorage.setItem('authToken', 'eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJhZG1pbiIsImNyZWF0ZWQiOjE1NjMwNTI2OTUwNjUsImV4cCI6MTU2NTY0NDY5NSwidXNlciI6IntcImlkXCI6MSxcIm5hbWVcIjpcImFkbWluXCIsXCJuaWNrXCI6XCJhZG1pblwiLFwic3RhdHVzXCI6XCJhY3RpdmVcIixcInBob25lXCI6XCJhZG1pblwifSJ9.SThkJAbTQtHGFCa4D4ltReUOoxxyGRc2oR1fqRmFkTcCbyDpHvVD1QYeymCVYffZp3ngkdYXNqH2-7wPAH8auA')
router.beforeEach((to, from, next) => {
  const isEmpty = (value) => {
    return (
      typeof value === "undefined" ||
      value === null ||
      value === "null" ||
      value === ""
    );
  };

  let authToken = util.$webStorage.getItem("authToken");
  let environment = util.$webStorage.getItem("environment");
  let userInfo = util.$webStorage.getItem("userInfo");

  // let environment = 'others';
  const noAuthWriteList = ["showcase", "album"];

  // next()

  if (
    to.path !== "/auth" &&
    noAuthWriteList.filter((item) => `/${item}` === to.path).length === 0
  ) {
    util.$webStorage.setItem("backRoute", to.fullPath);
    console.log("environment+++++", environment);
    if (isEmpty(userInfo) && environment !== "others") {
      next("/auth");
      //  next()
    } else {
      next();
    }
  } else {
    next();
  }
});

/* eslint-disable no-new */
new Vue({
  el: "#app",
  router,
  store,
  template: "<App/>",
  components: { App },
});
