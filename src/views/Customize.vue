<template>
  <div class="customize_main_container">
    <div class="common_logo_wrapper">
      <div class="common_logo_item"></div>
    </div>
    <div class="common_title_item">
      <img src="@/assets/image/common/title_00000.png" />
      <img src />
    </div>
    <div class="content">
      <div class="common_subtitle_item">
        <img src="@/assets/image/customize/subtitle_00000.png" />
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
              <img src="@/assets/image/customize/long_picture_00000.jpg" />
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
        <a class="close" @click="close"></a>
        <div class="content">
          <div class="title">
            {{ state.dialogData[state.dialogIndex].title }}
          </div>
          <div class="desc">
            <div class="picture">
              <img :src="state.dialogData[state.dialogIndex].image" />
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

const init = () => {
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

const checkDetail = (index: number) => {
  state.dialogFlag = true;
  state.dialogIndex = index;
};

const close = () => {
  state.dialogFlag = false;
};

onMounted(() => {
  init();
});
</script>

<style scoped lang="scss">
.customize_main_container {
  width: 100%;
  overflow: hidden;

  .longpicture_wrapper {
    width: 100%;

    .swiper-slide {
      width: 30rem;
      height: 100%;
      -webkit-box-sizing: border-box;
      box-sizing: border-box;

      a {
        display: inline-block;
        width: 1.2rem;
        height: 1.2rem;
        position: absolute;
        top: 3rem;
        background-image: url("@/assets/image/customize/dot_00000.png");
        background-size: contain;

        &.point {
          animation: blink 1s infinite;

          @keyframes blink {
            from {
              opacity: 0;
            }

            to {
              opacity: 1;
            }
          }
        }

        &.point1 {
          left: 7.5rem;
        }

        &.point2 {
          left: 15.5rem;
        }

        &.point3 {
          left: 25.5rem;
        }
      }

      img {
        display: inline-block;
        width: 100%;
        height: 100%;
        // overflow: hidden;
      }
    }
  }

  .hint {
    padding: 0.5rem;
    width: 100%;
    height: 2rem;
    color: #4387bd;
    box-sizing: border-box;

    p {
      font-size: 0.5rem;
    }
  }

  .common_dialog_container {
    .dialog_wrapper {
      width: 14rem;
      height: auto;

      .content {
        padding: 0.5rem 0.5rem 0.3rem 0.5rem;
        color: #fff;

        .title {
          display: block;
          padding: 0.5rem 0;
          font-size: 1.5rem;
        }

        .desc {
          display: flex;

          .picture {
            display: inline-block;
            padding: 0 0 0.3rem 0.5rem;
            width: 6rem;
            height: 100%;
            box-sizing: border-box;

            img {
              display: inline-block;
              width: 100%;
              height: auto;
            }
          }

          .main {
            display: inline-block;
            flex: 1;
            padding: 0 0.3rem;
            overflow: auto;
            text-align: left;

            p {
              margin: 0 0 0.6rem 0;
              font-size: 0.6rem;
            }
          }
        }
      }
    }
  }
}
</style>
