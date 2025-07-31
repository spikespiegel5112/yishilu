<template>
  <div class="category_main_container">
    <div class="common_logo_wrapper">
      <div class="common_logo_item"></div>
    </div>
    <div class="common_title_item">
      <img src="@/assets/image/common/title_00000.png" />
      <img src />
    </div>
    <div class="content">
      <div class="common_subtitle_item">
        <img src="@/assets/image/category/subtitle_00000.png" />
      </div>
      <div class="main">
        <div class="describe">
          从消费者需求出发，引导推动品类的发展，集眼科、视光学为一体，依视路为您提供保健、预防、矫治和康复的综合眼健康管理方案。
        </div>
        <img src="@/assets/image/category/category_content_00000.png" alt="" />
      </div>
      <div class="common_goback_wrapper">
        <CommonGoBack />
      </div>
    </div>
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

const currentInstance = getCurrentInstance() as ComponentInternalInstance;
const global = currentInstance.appContext.config.globalProperties;

const state = reactive({
  goToNextflag: false,
  dialogFlag: false,
  dialogIndex: 0,
  remUnit: 0,
  canvasWidth: "",
  canvasHeight: "",
});

const navigatorStyle = computed(() => {
  return {
    "background-image":
      "url(" + require("@/assets/image/homepage/navigator_00000.png") + ")",
  };
});

watch(
  () => state.goToNextflag,
  (newValue: any) => {
    if (newValue) {
      global.$router.push({
        name: "HomePage",
      });
    }
  }
);

watch(
  () => state.remUnit,
  (newValue: any) => {
    nextTick(() => {
      let windowWidth = document.body.clientWidth.toString().replace("px", "");
      state.canvasWidth = document.body.clientWidth + "px";
      state.canvasHeight = document.body.clientHeight + "px";
    });
  }
);
</script>

<style scoped></style>
