<template>
  <div class="customize_main_container">
    <div class="common_logo_wrapper">
      <div class="common_logo_item"></div>
    </div>
    <div class="common_title_item">
      <img src="@/assets/image/common/title_00000.png" alt />
      <img src alt />
    </div>
    <div class="content">
      <div class="common_subtitle_item">
        <img src="@/assets/image/customize/subtitle_00000.png" alt />
      </div>
      <div class="longpicture_wrapper">
        <div class="swiper-container">
          <ul class="swiper-wrapper">
            <li class="swiper-slide">
              <a
                class="point1 point"
                href="javascript:;"
                @click="checkDetail(0)"
              >
                <span></span>
              </a>
              <a
                class="point2 point"
                href="javascript:;"
                @click="checkDetail(1)"
              >
                <span></span>
              </a>
              <a
                class="point3 point"
                href="javascript:;"
                @click="checkDetail(2)"
              >
                <span></span>
              </a>
              <img src="@/assets/image/customize/long_picture_00000.jpg" alt />
            </li>
          </ul>
        </div>
      </div>
      <div class="hint">
        <p>请左右拖动查看全景照片</p>
      </div>
      <div class="common_goback_wrapper">
        <CommonGoBack />
      </div>
    </div>
    <div v-if="state.dialogFlag" class="common_dialog_container">
      <div class="dialog_wrapper">
        <a href="javascript:;" class="close" @click="close"></a>
        <div class="content">
          <div class="title">
            {{ state.dialogData[state.dialogIndex].title }}
          </div>
          <div class="desc">
            <div class="picture">
              <img :src="state.dialogData[state.dialogIndex].image" alt />
            </div>
            <div class="main">
              <p v-for="item in state.dialogData[state.dialogIndex].paragraph">
                {{ item }}
              </p>
            </div>
          </div>
        </div>
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
import Swiper from "swiper";

const currentInstance = getCurrentInstance() as ComponentInternalInstance;
const global = currentInstance.appContext.config.globalProperties;

const state = reactive({
  goToNextflag: false,
  dialogFlag: false,
  dialogIndex: 0,
  dialogData: [
    {
      title: "成人视活力赋能",
      paragraph: [
        "店内场景的标准化指引",
        "明星产品推荐",
        "演示道具配置说明",
        "眼健康设备筛查验配等",
      ],
      image: require("@/assets/image/customize/dialog_picture_3_00000.jpg"),
    },
    {
      title: "中生代视耐力续航",
      paragraph: [
        "店内场景的标准化指引",
        "明星产品推荐",
        "演示道具配置说明",
        "眼健康设备筛查验配等",
      ],
      image: require("@/assets/image/customize/dialog_picture_2_00000.jpg"),
    },
    {
      title: "青少年视潜力管理",
      paragraph: [
        "店内场景的标准化指引",
        "明星产品推荐",
        "演示道具配置说明",
        "眼健康设备筛查验配等",
      ],
      image: require("@/assets/image/customize/dialog_picture_1_00000.jpg"),
    },
  ],
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
    direction: "horizontal",
    slidesPerView: "auto",
    freeMode: true,
    // scrollbar: {
    // 	el: '.swiper-scrollbar',
    // },
    mousewheel: true,
  });
};

const checkDetail = (index) => {
  state.dialogFlag = true;
  state.dialogIndex = index;
};

const close = () => {
  state.dialogFlag = false;
};

onMounted(() => {
  setTimeout(() => {
    init();
  }, 100);
});
</script>

<style scoped></style>
