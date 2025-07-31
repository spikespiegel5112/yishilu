<template>
  <div class="homepage_main_container">
    <div class="common_logo_wrapper">
      <div class="common_logo_item"></div>
    </div>
    <div class="common_title_item">
      <img src="@/assets/image/common/title_00000.png" />
    </div>
    <div class="content">
      <div class="bg" :style="navigatorStyle"></div>

      <!-- <img src="@/assets/image/homepage/navigator_00000.png" /> -->
      <ul class="links">
        <li class="link1">
          <router-link :to="{ name: 'Information' }" />
        </li>
        <li class="link2">
          <router-link :to="{ name: 'Interaction' }" />
        </li>
        <li class="link3">
          <router-link :to="{ name: 'Category' }" />
        </li>
        <li class="link4">
          <router-link :to="{ name: 'Customize' }" />
        </li>
        <li class="link5">
          <a @click="checkCollection"></a>
        </li>
      </ul>
    </div>
    <div
      v-if="state.collectionNotYetFlag"
      class="common_dialog_container collection"
    >
      <div class="dialog_wrapper">
        <a class="close" @click="closeDialog"></a>
        <div class="content">
          <p class="title">活动尚未开始，敬请期待</p>
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

const currentInstance = getCurrentInstance() as ComponentInternalInstance;
const global = currentInstance.appContext.config.globalProperties;

const state = reactive({
  goToNextflag: false,
  collectionNotYetFlag: false,
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
  () => state.remUnit,
  (newValue: any) => {
    nextTick(() => {
      let windowWidth = document.body.clientWidth.toString().replace("px", "");
      state.canvasWidth = document.body.clientWidth + "px";
      state.canvasHeight = document.body.clientHeight + "px";
    });
  }
);

onMounted(() => {
  nextTick(() => {
    state.remUnit = Number(
      document.getElementsByTagName("html")[0].style.fontSize.replace("px", "")
    );
  });
});

const checkCollection = () => {
  const destinationTimeStamp = global.$dayjs(new Date("2020-2-11")).valueOf();
  const currentTimeStamp = Date.parse(new Date());
  console.log(destinationTimeStamp);
  console.log(currentTimeStamp);
  if (currentTimeStamp > destinationTimeStamp) {
    global.$router.push({
      name: "Entrance",
    });
  } else {
    global.collectionNotYetFlag = true;
  }
};
const closeDialog = () => {
  global.collectionNotYetFlag = false;
};
</script>

<style scoped lang="scss">
.homepage_main_container {
  width: 100%;

  .common_title_item {
    // margin-top: 1.5rem;
  }

  .content {
    margin: 3rem auto 0;
    padding: 0 0 2rem 0;
    width: 12.5rem;
    height: 12.5rem;
    position: relative;

    .bg {
      width: 100%;
      height: 100%;
      background-size: 100%;
      background-repeat: no-repeat;
      position: absolute;
      top: 0;
    }

    .links {
      width: 100%;
      height: 100%;
      position: relative;

      li {
        display: inline-block;
        width: 2.7rem;
        height: 2.7rem;
        position: absolute;

        a {
          display: inline-block;
          width: 100%;
          height: 100%;
          border-radius: 50%;
          // background-color: red;
        }

        &.link1 {
          left: 5rem;
          top: 0.5rem;
        }

        &.link2 {
          left: 9.4rem;
          top: 3.3rem;
        }

        &.link3 {
          left: 8rem;
          top: 8.3rem;
        }

        &.link4 {
          left: 2.1rem;
          top: 8.3rem;
        }

        &.link5 {
          left: 0.55rem;
          top: 3.3rem;
        }
      }
    }
  }

  .common_dialog_container {
    &.collection {
      .dialog_wrapper {
        width: 9rem;
        height: 5rem;
        .content {
          margin: 0;
          padding: 0;
          width: 100%;
          height: 100%;
          &:before {
            content: "";
            display: inline-block;
            width: 0;
            height: 100%;
            vertical-align: middle;
          }
          .title {
            display: inline-block;
            width: 100%;
            vertical-align: middle;
            font-size: 0.7rem;
          }
        }
      }
    }
  }
}
</style>
