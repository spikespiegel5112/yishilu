<template>
  <router-view></router-view>
</template>

<script lang="tsx" setup>
import {
  reactive,
  watch,
  computed,
  onMounted,
  onBeforeUnmount,
  getCurrentInstance,
  ref,
  nextTick,
} from "vue";

import type { ComponentInternalInstance } from "vue";

const currentInstance = getCurrentInstance() as ComponentInternalInstance;
const global = currentInstance.appContext.config.globalProperties;
// import {getAndStoreUserInfo} from '../api/auth.js'

const getUserInfoCache = () => {
  if (!global.$isEmpty(sessionStorage.getItem("userInfo"))) {
    global.$store.commit(
      "setUserInfo",
      JSON.parse(sessionStorage.getItem("userInfo"))
    );
  }
};
</script>

<style lang="less">
/*@import '~vux/src/styles/reset.less';*/

// body {
//   background-color: #fbf9fe;
// }

@group-title-margin-top: 0px;
</style>

<style lang="scss">
@import "sass/reset.scss";
@import "sass/common.scss";
@import "sass/main.scss";
</style>

<style lang="css">
@import "../node_modules/swiper/dist/css/swiper.min.css";
</style>
