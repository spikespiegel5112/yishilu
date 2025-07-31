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
  state.userInfo = JSON.parse(sessionStorage.getItem("userInfo"));
  getImage();

  addscancount();
});
</script>

<style scoped></style>
