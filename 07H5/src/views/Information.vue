<template>
  <div class="information_main_container">
    <div class="common_logo_wrapper">
      <div class="common_logo_item"></div>
    </div>
    <div class="common_title_item">
      <img src="@/assets/image/common/title_00000.png" alt />
      <img src alt />
    </div>
    <div class="content">
      <div class="common_subtitle_item">
        <img src="@/assets/image/information/subtitle_00000.png" alt />
      </div>
      <div class="frame_wrapper">
        <!-- <div class="frame"></div> -->
        <div class="swiper-container">
          <ul class="swiper-wrapper">
            <li class="swiper-slide">
              <img
                src="@/assets/image/information/slide_item_2_00000.png"
                alt
              />
            </li>
            <li class="swiper-slide">
              <img
                src="@/assets/image/information/slide_item_3_00000.png"
                alt
              />
            </li>
            <li class="swiper-slide">
              <img
                src="@/assets/image/information/slide_item_4_00000.png"
                alt
              />
            </li>
          </ul>
        </div>
      </div>
      <div class="position">
        <img src="@/assets/image/information/position_00000.png" alt />
      </div>
      <div class="common_navigation_item">
        <a @click="getLocation">一键导航</a>
      </div>
      <div class="common_goback_wrapper">
        <CommonGoBack />
      </div>
    </div>
  </div>
</template>

<script lang="tsx" setup>
import wx from "weixin-js-sdk";
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
import Swiper from "swiper";

const currentInstance = getCurrentInstance() as ComponentInternalInstance;
const global = currentInstance.appContext.config.globalProperties;

const state = reactive({
  goToNextflag: false,
  latitude: "",
  longitude: "",
  locationData: {},
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
        name: "homepage",
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

const init = () => {
  // console.log(FrameAnimation)
  const swiper = new Swiper(".swiper-container", {
    loop: true, // 循环模式选项
  });
};
const getLocation = () => {
  initGetLocation()
    .then((response: any) => {
      console.log("res+++++++++++++++++", response);
      var _this = this;
      console.log(
        "http://api.map.baidu.com/direction?origin=" +
          response.latitude +
          "," +
          response.longitude +
          "&destination=31.18846,121.496541&mode=driving&region=上海市&output=html"
      );
      location.href =
        "http://api.map.baidu.com/direction?origin=" +
        response.latitude +
        "," +
        response.longitude +
        "&destination=31.18846,121.496541&mode=driving&region=上海市&output=html";
    })
    .catch((error:any) => {
      console.error(error);
    });
};

const initGetLocation = () => {
  return new Promise((resolve, reject) => {
    const that = this;

    // wx.getLocation({
    // 	success: function (res) {
    // 		callback(res)
    // 	},
    // 	cancel: function (res) {
    // 		alert('用户拒绝授权获取地理位置')
    // 	}
    // })
    global.$wxsdk.getLocation(
      function (res) {
        resolve(res);
      },
      function (error) {
        console.log("error++++++", error);
        reject(error);
      }
    );
  });
};

onMounted(() => {
  init();
});
</script>

<style scoped></style>
