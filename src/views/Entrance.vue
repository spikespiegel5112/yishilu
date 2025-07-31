<template>
  <div class="entrance_main_container">
    <div class="content">
      <CommonLoading :loading="state.loading" />
      <img :src="state.entranceImage" />
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
  countdownFlag: false,
  loading: true,
  userInfo: {
    id: "",
  },
  entranceImage: require("@/assets/image/entrance/entrance_00000.png"),
  entranceImagePath: require("@/assets/image/entrance/entrance_00000.png"),
  remUnit: 0,
  canvasWidth: "",
  canvasHeight: "",
});

watch(
  () => state.countdownFlag,
  (newValue: any) => {
    if (newValue) {
      goToNextPage();
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

const getImage = () => {
  const imageObject = new Image();
  imageObject.src = state.entranceImagePath;
  imageObject.onload = () => {
    state.loading = false;
    state.countdownFlag = true;
  };
};

const addscancount = () => {
  global.$http
    .get(global.$baseUrl + "h5.wxuser.pageaccess", {
      params: { u_id: global.$store.state.user.userInfo, scan_type: 0 },
    })
    .then((response: any) => {
      console.log(response);
    })
    .catch((error: any) => {
      console.log(error);
    });
};

const goToNextPage = () => {
  const duration = 3000;
  setTimeout(() => {
    global.$router.push({
      name: "HomePage",
    });
  }, duration);
};

onMounted(() => {
  getImage();

  addscancount();
});
</script>

<style scoped lang="scss">
.entrance_main_container {
  width: 100%;
  //height: 100%;
  background-size: 100%;
  position: relative;
  overflow: hidden;

  .content {
    width: 100%;
    height: 100vh;
    overflow: auto;

    img {
      display: block;
      width: 100%;
    }
  }

  .common_bg_item {
    position: absolute;
    bottom: 0;
  }

  .entrance_button_wrapper {
    width: 100%;
    position: absolute;
    bottom: 4.75rem;
    z-index: 366;

    .entrance_beginbutton {
      display: block;
      //margin: 27.7rem auto 0;
      margin: 21.4rem auto 0;
      width: 6.5rem;
      height: 2.2rem;
      // background-image: url("./image/entrance/begin_button_00000.png");
      background-repeat: no-repeat;
      background-size: contain;
      opacity: 1;
      transition: all 0.6s;
      z-index: -10;
    }

    .active {
      opacity: 0;
      transition: all 0.6s;
    }
  }

  .common_logo_wrapper {
    margin: 1rem 0 0 0;
    position: absolute;
    bottom: 1.5rem;
  }
}
</style>
