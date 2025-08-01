<template>
  <div class="common_main_container" :style="state.appStyle">
    <router-view />
  </div>
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
import app from "../main";

const currentInstance = getCurrentInstance() as ComponentInternalInstance;
const global = currentInstance.appContext.config.globalProperties;

const state = reactive({
  clientWidth: 0,
  mobileMode: false,
  isWeLink: false,
  pageReady: false,
  darkMode: true,
  appStyle: {},
  menuList: [
    {
      title: "画集",
      name: "gallary",
      active: false,
    },
    {
      title: "文生图",
      name: "txt2Img",
      active: true,
    },
  ],
});

const calculateAppStyle = () => {
  const clientWidth = document.body.clientWidth;
  const clientHeight = document.body.clientHeight;
  const ratio = clientWidth / clientHeight;
  const result = {
    width: "100%",
  };
  if (ratio > 0.562) {
    result.width = document.body.clientHeight * 0.562 + "px";
  }
  state.appStyle = result;
};

onMounted(() => {
  const userInfo = JSON.parse(sessionStorage.getItem("userInfo"));
  global.$store.commit("user/updateUserInfo", userInfo);

  global.$remResizing({
    baseline: 320,
    fontSize: 20,
    threshold: 640,
  });
  window.onresize = () => {
    calculateAppStyle();
  };
  calculateAppStyle();
});
</script>
