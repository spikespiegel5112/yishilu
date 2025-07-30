<template>
  <div class="common_main_container">
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
  ComponentInternalInstance,
  ref,
  nextTick,
} from "vue";

const currentInstance = getCurrentInstance() as ComponentInternalInstance;
const global = currentInstance.appContext.config.globalProperties;

const state = reactive({
  clientWidth: 0,
  mobileMode: false,
  isWeLink: false,
  pageReady: false,
  darkMode: true,
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
</script>
