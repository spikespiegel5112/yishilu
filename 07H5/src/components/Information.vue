<template>
  <div class="information_main_container">
    <div class="common_logo_wrapper">
      <div class="common_logo_item"></div>
    </div>
    <div class="common_title_item">
      <img src="@/image/common/title_00000.png" alt />
      <img src alt />
    </div>
    <div class="content">
      <div class="common_subtitle_item">
        <img src="@/image/information/subtitle_00000.png" alt />
      </div>
      <div class="frame_wrapper">
        <!-- <div class="frame"></div> -->
        <div class="swiper-container">
          <ul class="swiper-wrapper">
            <li class="swiper-slide">
              <img src="@/image/information/slide_item_2_00000.png" alt />
            </li>
            <li class="swiper-slide">
              <img src="@/image/information/slide_item_3_00000.png" alt />
            </li>
            <li class="swiper-slide">
              <img src="@/image/information/slide_item_4_00000.png" alt />
            </li>
          </ul>
        </div>
      </div>
      <div class="position">
        <img src="@/image/information/position_00000.png" alt />
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

<script>
import wx from "weixin-js-sdk";
import Swiper from "swiper";
export default {
  name: "Information",
  components: {},
  data() {
    return {
      goToNextflag: false,
      latitude: "",
      longitude: "",
      locationData: {},
    };
  },
  computed: {
    navigatorStyle() {
      return {
        "background-image":
          "url(" + require("@/image/homepage/navigator_00000.png") + ")",
      };
    },
  },
  watch: {
    goToNextflag(value) {
      if (value === true) {
        this.$router.push({
          name: "homepage",
        });
      }
    },
    remUnit(value) {
      this.$nextTick(() => {
        let windowWidth = document.body.clientWidth
          .toString()
          .replace("px", "");
        this.canvasWidth = document.body.clientWidth + "px";
        this.canvasHeight = document.body.clientHeight + "px";
      });
    },
  },

  mounted() {
    this.init();
  },
  methods: {
    init() {
      // console.log(FrameAnimation)
      const swiper = new Swiper(".swiper-container", {
        loop: true, // 循环模式选项
      });
    },
    getLocation() {
      this.initGetLocation()
        .then((response) => {
          console.log("res+++++++++++++++++", response);
          var _this = this;
          console.log(
            "http://api.map.baidu.com/direction?origin=" +
              response.latitude +
              "," +
              response.longitude +
              "&destination=31.18846,121.496541&mode=driving&region=上海市&output=html",
          );
          location.href =
            "http://api.map.baidu.com/direction?origin=" +
            response.latitude +
            "," +
            response.longitude +
            "&destination=31.18846,121.496541&mode=driving&region=上海市&output=html";
        })
        .catch((error) => {
          console.error(error);
        });
    },
    initGetLocation() {
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
        this.$wxsdk.getLocation(
          function (res) {
            resolve(res);
          },
          function (error) {
            console.log("error++++++", error);
            reject(error);
          },
        );
      });
    },
  },
};
</script>

<style scoped></style>
