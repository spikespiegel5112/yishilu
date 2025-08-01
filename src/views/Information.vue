<template>
  <div class="information_main_container">
    <div class="common_logo_wrapper">
      <div class="common_logo_item"></div>
    </div>
    <div class="common_title_item">
      <img src="@/assets/image/common/title_00000.png" />
    </div>
    <div class="content">
      <div class="common_subtitle_item">
        <img src="@/assets/image/information/subtitle_00000.png" />
      </div>
      <div class="frame_wrapper">
        <!-- <div class="frame"></div> -->
        <div class="swiper-container">
          <ul class="swiper-wrapper">
            <li class="swiper-slide">
              <img src="@/assets/image/information/slide_item_2_00000.png" />
            </li>
            <li class="swiper-slide">
              <img src="@/assets/image/information/slide_item_3_00000.png" />
            </li>
            <li class="swiper-slide">
              <img src="@/assets/image/information/slide_item_4_00000.png" />
            </li>
          </ul>
        </div>
      </div>
      <div class="position">
        <img src="@/assets/image/information/position_00000.png" />
      </div>
      <div class="common_navigation_item">
        <a @click="getLocation"></a>
      </div>
      <CommonGoBack />
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
        name: "HomePage",
      });
    }
  },
);

watch(
  () => state.remUnit,
  (newValue: any) => {
    nextTick(() => {
      let windowWidth = document.body.clientWidth.toString().replace("px", "");
      state.canvasWidth = document.body.clientWidth + "px";
      state.canvasHeight = document.body.clientHeight + "px";
    });
  },
);

const init = () => {
  // console.log(FrameAnimation)
  const swiper = new global.$Swiper(".swiper-container", {
    loop: true, // 循环模式选项
  });
};
const getLocation = () => {
  initGetLocation()
    .then((response: any) => {
      console.log("res+++++++++++++++++", response);
      let _this = this;
      location.href =
        "http://api.map.baidu.com/direction?origin=" +
        response.latitude +
        "," +
        response.longitude +
        "&destination=31.18846,121.496541&mode=driving&region=上海市&output=html";
    })
    .catch((error: any) => {
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
    // global.$wxsdk.getLocation(
    //   function (res: any) {
    //     resolve(res);
    //   },
    //   function (error: any) {
    //     console.log("error++++++", error);
    //     reject(error);
    //   }
    // );
  });
};

onMounted(() => {
  init();
});
</script>

<style scoped lang="scss">
$yellow: #f9c978;
.information_main_container {
  width: 100%;
  overflow: hidden;

  > .content {
    .frame_wrapper {
      width: 100%;
      height: 10rem;

      .swiper-container {
        .swiper-wrapper {
          padding: 0;
          .swiper-slide {
            background-color: transparent;

            img {
              width: 13rem;
            }
          }
        }
      }

      .frame {
        width: 100%;
        height: 100%;
        background-image: url("@/assets/image/information/frame_00000.png");
        background-size: contain;
        background-repeat: no-repeat;
      }
    }

    .position {
      margin: 0.8rem 0;

      img {
        width: 10rem;
      }
    }

    .common_navigation_item {
      a {
        background-image: url("@/assets/image/information/button_yijiandaohang_00000.png");
      }
    }
  }
}
</style>
