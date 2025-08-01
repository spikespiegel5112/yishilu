<template>
  <div class="interaction_main_container">
    <div class="common_logo_wrapper">
      <div class="common_logo_item"></div>
    </div>
    <div class="common_title_item">
      <img src="@/assets/image/common/title_00000.png" />
    </div>
    <!-- <div class="bg"></div> -->
    <div class="content">
      <div class="common_subtitle_item">
        <img src="@/assets/image/interaction/subtitle_00000.png" />
      </div>
      <div class="interaction_status_wrapper">
        <ul>
          <li class="yan" @click="receivetask(1)">
            <a>
              <img
                class="disabled"
                src="@/assets/image/interaction/button_interaction_yan_00000.png"
              />
              <img
                :class="state.yan_enabled"
                src="@/assets/image/interaction/button_interaction_yan_active_00000.png"
              />
            </a>
          </li>
          <li class="shi" @click="receivetask(2)">
            <a>
              <img
                class="disabled"
                src="@/assets/image/interaction/button_interaction_shi_00000.png"
              />
              <img
                :class="state.shi_enabled"
                src="@/assets/image/interaction/button_interaction_shi_active_00000.png"
              />
            </a>
          </li>
          <li class="guang" @click="receivetask(3)">
            <a>
              <img
                class="disabled"
                src="@/assets/image/interaction/button_interaction_guang_00000.png"
              />
              <img
                :class="state.guang_enabled"
                src="@/assets/image/interaction/button_interaction_guang_active_00000.png"
              />
            </a>
          </li>
        </ul>
      </div>
      <div class="rules">
        <p>游戏规则：</p>
        <p>
          点击上方图标，找到“眼-依视路设备”、“视-优视路”、“光-全视线”展位现场的互动二维码，使用微信的扫一扫功能，扫码点亮本页标签，即可参加抽奖活动，有机会获得暖心礼品一份。
        </p>
      </div>
      <div class="common_navigation_item">
        <a @click="checkStatus">
          <span>参加抽奖</span>
        </a>
      </div>
      <div class="common_goback_wrapper">
        <CommonGoBack />
      </div>
    </div>
    <div v-if="state.dialogNotYetFlag" class="common_dialog_container notyet">
      <div class="dialog_wrapper">
        <a class="close" @click="closeDialog"></a>
        <div class="content">
          <p>您尚未完成全部打卡</p>
          <p>请全部完成后进行抽奖</p>
        </div>
      </div>
    </div>
    <div v-if="state.dialogYanFlag" class="common_dialog_container prompt">
      <div class="dialog_wrapper">
        <a class="close" @click="closeDialog"></a>
        <div class="content">
          <p class="title"><span>眼</span>依视路设备</p>
          <p class="desc">
            给大众更全面、更完善的眼健康
            <br />和视觉质量解决方案。
          </p>
          <p class="position">展位号：二号馆2G40-2</p>
        </div>
      </div>
    </div>
    <div v-if="state.dialogShiFlag" class="common_dialog_container prompt">
      <div class="dialog_wrapper">
        <a class="close" @click="closeDialog"></a>
        <div class="content">
          <p class="title"><span>视</span>依视路</p>
          <p class="desc">
            依视路改善视力，改善生活，
            <br />让清晰视觉变得触手可得
          </p>
          <p class="position">展位号：一号馆1A02</p>
        </div>
      </div>
    </div>
    <div v-if="state.dialogGuangFlag" class="common_dialog_container prompt">
      <div class="dialog_wrapper">
        <a class="close" @click="closeDialog"></a>
        <div class="content">
          <p class="title"><span>光</span>全视线</p>
          <p class="desc">
            全视线第八代感光变色镜片，
            <br />前沿科技，光彩上市
          </p>
          <p class="position">展位号：二号馆2F06</p>
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
  getLightenRequest: "h5.get.wxuser.lighten",
  goToNextflag: false,
  dialogFlag: false,
  dialogNotYetFlag: false,
  dialogYanFlag: false,
  dialogShiFlag: false,
  dialogGuangFlag: false,
  status: false,
  yan_enabled: "enabled",
  shi_enabled: "enabled",
  guang_enabled: "enabled",
  userInfo: {
    id: "",
  },
  task_type: 0,
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

const userInfo = computed(() => {
  return global.$store.state.user.userInfo;
});

watch(
  () => state.goToNextflag,
  (newValue: any) => {
    if (newValue) {
      if (newValue === true) {
        global.$router.push({
          name: "HomePage",
        });
      }
    }
  },
);

const init = () => {
  //task_type    1:眼  2:视  3:光
  state.task_type = getParameter("task_type");
  console.log("state.task_type+++", state.task_type);
  if (state.task_type) {
    lightTask();
    addscancount();
  } else {
    getLighten();
  }
};
const lighttask = () => {
  //点亮任务
  global.$http
    .post("h5.user.light.task", {
      User_Id: userInfo.value.id,
      Task_Type: state.task_type,
    })
    .then((r) => {
      console.log("lighttask+++", r);
      global.$vux.confirm.show({
        showCancelButton: false,
        title: r.msg,
        onConfirm() {},
      });
      getLighten();
    })
    .catch((error: any) => {
      console.log(error);
    });
};
const getLighten = () => {
  //获取用户点亮的任务
  global.$http
    .get(global.$baseUrl + state.getLightenRequest, {
      params: { u_id: userInfo.value.id },
    })
    .then((response: any) => {
      if (response.data) {
        state.yan_enabled = response.data.f_eye ? "disabled" : "enabled";
        state.shi_enabled = response.data.f_regard ? "disabled" : "enabled";
        state.guang_enabled = response.data.f_light ? "disabled" : "enabled";
      }
    })
    .catch((error: any) => {
      console.log(error);
    });
};
const getParameter = (key: any) => {
  var url = location.href;
  var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
  var paraObj = {};
  for (var i = 0, len = paraString.length; i < len; i++) {
    var j = paraString[i];
    paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(
      j.indexOf("=") + 1,
      j.length,
    );
  }
  var returnValue = paraObj[key.toLowerCase()];
  if (typeof returnValue === "undefined") {
    return "";
  } else {
    return returnValue;
  }
};
const addscancount = () => {
  //增加扫码次数
  global.$http
    .get(global.$baseUrl + "h5.wxuser.pageaccess", {
      params: {
        u_id: userInfo.value,
        scan_type: state.task_type,
      },
    })
    .then((response: any) => {
      console.log(response);
    })
    .catch((error: any) => {
      console.log(error);
    });
};
const receivetask = (type) => {
  //领取任务
  switch (type) {
    case 1:
      state.dialogYanFlag = true;
      break;
    case 2:
      state.dialogShiFlag = true;
      break;
    case 3:
      state.dialogGuangFlag = true;
      break;
    default:
      break;
  }
};
const checkStatus = () => {
  if (
    state.yan_enabled == "disabled" &&
    state.shi_enabled == "disabled" &&
    state.guang_enabled == "disabled"
  ) {
    global.$router.push({
      name: "LotteryDraw",
    });
  } else {
    state.dialogNotYetFlag = true;
  }
};
const closeDialog = () => {
  state.dialogFlag = false;
  state.dialogNotYetFlag = false;
  state.dialogYanFlag = false;
  state.dialogShiFlag = false;
  state.dialogGuangFlag = false;
};

onMounted(() => {
  init();
});
</script>

<style scoped lang="scss">
.interaction_main_container {
  width: 100%;
  overflow: hidden;

  .bg {
    width: 100%;
    height: 100vh;
    background-image: url("@/assets/image/interaction/bg_interaction_00000.png");
    background-repeat: no-repeat;
    background-size: contain;
    position: absolute;
    top: 0;
    left: 0;
  }

  > .content {
    margin: 1rem 0 0 0;
    position: relative;

    .rules {
      margin: 1.5rem auto 1rem;
      width: 13rem;
      text-align: left;
      font-size: 0.55rem;
    }

    .interaction_status_wrapper {
      ul {
        margin: auto;
        padding: 0;
        width: 7rem;
        font-size: 0;

        li {
          display: inline-block;
          width: 3.5rem;
          height: 3rem;
          overflow: hidden;
          background-image: url("@/assets/image/interaction/button_interaction_bg_00000.png");
          background-repeat: no-repeat;
          background-position: center;
          background-size: contain;

          &:first-child {
            width: 4.5rem;
          }

          a {
            display: inline-block;
            width: 3.5rem;
            height: auto;
            position: relative;

            img {
              display: inline-block;
              width: 3rem;
              position: absolute;
              left: 0.25rem;
            }

            .enabled {
              display: none;
            }
          }

          &.active {
            a {
              .disbled {
                display: none;
              }

              .enabled {
                display: inline-block;
              }
            }
          }
        }
      }
    }

    .common_navigation_item {
      a {
        // background-image: url("@/assets/image/interaction/button_drawalottery_00000.png");
      }
    }
  }

  .common_dialog_container {
    &.notyet {
      .dialog_wrapper {
        width: 10rem;
        height: 4rem;

        .content {
          padding: 1rem 0 0 0;

          p {
            font-size: 0.7rem;
          }
        }
      }
    }

    &.prompt {
      .dialog_wrapper {
        padding: 0.2rem;
        width: auto;
        height: auto;
      }
      .content {
        padding: 0.5rem 0.75rem 0.2rem 0.75rem;
        p {
          font-size: 0.7rem;
          text-align: left;
        }
        .title {
          font-size: 0.75rem;
          span {
            display: inline-block;
            padding: 0 0.2rem 0 0;
            font-size: 2rem;
            font-weight: bold;
          }
        }
        .desc {
          font-size: 0.55rem;
          line-height: 0.6rem;
        }
        .position {
          padding: 0.4rem 0 0 0;
          font-size: 0.7rem;
          line-height: 0.6rem;
          border-top: 2px solid #fff;
        }
      }
    }

    &.position {
      .dialog_wrapper {
        .content {
          .brandname {
            font-size: 20px;
          }
        }
      }
    }
  }
}
</style>
