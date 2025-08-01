<template>
  <div class="interactionlogo_main_container">
    <div class="common_logo_wrapper">
      <div class="common_logo_item"></div>
    </div>
    <div class="common_title_item">
      <img src="@/assets/image/common/title_00000.png" />
    </div>
    <div class="content">
      <div class="common_subtitle_item">
        <img src="@/assets/image/interactionlogo/subtitle_00000.png" />
      </div>
      <div class="rules">
        <ol>
          <li>请前往下方7个依视路集团子品牌展台，进行扫码打卡签到；</li>
          <li>
            您可点击下方Logo查看相关展台展位号，扫码完成后该展台Logo将被点亮；
          </li>
          <li>
            完成任意2个Logo点亮将赢得一次额外的抽奖机会，数量有限领完即止。
          </li>
        </ol>
      </div>
      <div class="status_wrapper">
        <ul>
          <li
            v-for="(item, index) in brandList"
            :key="item.name"
            :class="item.active ? 'active' + ' ' + item.name : item.name"
            @click="receivetask(index)"
          >
            <a @click="checkBrandInfo(item)">
              <div class="enabled">
                <img :src="item.enableSrc" />
              </div>
              <div class="disabled">
                <img :src="item.disabledSrc" />
              </div>
            </a>
          </li>
        </ul>
        <div class="hint">
          <p>*排名不分先后</p>
        </div>
      </div>
      <div class="navigation_wrapper">
        <div class="common_navigation_item">
          <a @click="checkStatus">点击抽奖</a>
        </div>
        <div class="common_navigation_item">
          <a @click="checkPrize">我的奖品</a>
        </div>
      </div>
      <div class="common_goback_wrapper">
        <CommonGoBack to="LotteryDraw" />
      </div>
    </div>
    <div v-if="state.dialogNotYetFlag" class="common_dialog_container notyet">
      <div class="dialog_wrapper">
        <a class="close" @click="closeDialog"></a>
        <div class="content">
          <p>请前往相关展台扫码点亮</p>
          <p>任意两个 Logo 进行抽奖</p>
        </div>
      </div>
    </div>
    <div
      v-if="state.dialogPositionFlag"
      class="common_dialog_container position"
    >
      <div class="dialog_wrapper">
        <a class="close" @click="closeDialog"></a>
        <div class="content">
          <p class="brandname">{{ state.currentBrandData.brandName }}</p>
          <p>展位号</p>
          <p class="number">{{ state.currentBrandData.positionNumber }}</p>
          <p class="hint">*奖品数量有限，领完即止</p>
        </div>
      </div>
    </div>
    <div v-if="state.dialogLightenFlag" class="common_dialog_container lighten">
      <div class="dialog_wrapper">
        <a class="close" @click="closeDialog"></a>
        <div class="content">
          <p>您已点亮</p>
          <p class="brandname">{{ state.currentBrandData.brandName }}</p>
        </div>
      </div>
    </div>
    <div
      v-if="state.dialogPrizeListFlag"
      class="common_dialog_container prizelist"
    >
      <div class="dialog_wrapper">
        <a class="close" @click="closeDialog"></a>
        <div class="content">
          <p class="title" @click="closeDialog">我的奖品</p>
          <div class="list">
            <ul>
              <li v-for="(item, index) in state.prizeList" :key="index">
                {{ item.name !== "" ? item.index + "." + item.name : "" }}
              </li>
            </ul>
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

const currentInstance = getCurrentInstance() as ComponentInternalInstance;
const global = currentInstance.appContext.config.globalProperties;

const state = reactive({
  getLighten2Request: "h5.get.wxuser.lighten2",
  getDrawprizelist2Request: "h5.get.wxuser.drawprizelist2",
  getTaskRequest: "h5.user.light.task",
  goToNextflag: false,
  dialogFlag: false,
  dialogPositionFlag: false,
  dialogLightenFlag: false,
  dialogNotYetFlag: false,
  dialogPrizeListFlag: false,
  status: false,
  task_type: 0,
  prizeList: [
    {
      name: "暂无中奖信息",
      index: "",
      hasPrize: false,
    },
    {
      name: "暂无中奖信息",
      index: "",
      hasPrize: false,
    },
    {
      name: "暂无中奖信息",
      index: "",
      hasPrize: false,
    },
  ] as any,
  currentBrandData: {
    brandName: "",
    positionNumber: "",
  },
  remUnit: 0,
  canvasWidth: "",
  canvasHeight: "",
  dialogYanFlag: false,
  dialogShiFlag: false,
  dialogGuangFlag: false,
});

const userInfo = computed(() => {
  return global.$store.state.user.userInfo;
});

const brandList = computed(() => {
  return global.$store.state.app.brandList;
});

watch(
  () => state.goToNextflag,
  (newValue: any) => {
    if (newValue === true) {
      global.$router.push({
        name: "HomePage",
      });
    }
  }
);

const init = () => {
  state.task_type = getParameter("task_type");
  console.log("state.task_type+++", state.task_type);
  if (state.task_type) {
    lightTask();
    addscancount();
  } else {
    getLighten();
  }
  getPrizeList();
};
const lightTask = () => {
  //点亮任务
  global.$http
    .post(global.$baseUrl + state.getTaskRequest, {
      User_Id: userInfo.value.id,
      Task_Type: state.task_type,
    })
    .then((response: any) => {
      console.log("lightTask+++", response);
      state.dialogLightenFlag = true;
      state.currentBrandData.brandName = brandList.value.find(
        (item: any) => item.taskType === Number(response.data)
      ).brandName;
      getLighten();
    })
    .catch((error: any) => {
      console.log(error);
    });
};
const getLighten = () => {
  //获取用户点亮的任务
  console.log("getLighten+++++++", global.$store.state);
  global.$http
    .get(global.$baseUrl + state.getLighten2Request, {
      params: { u_id: userInfo.value.id },
    })
    .then((response: any) => {
      if (response.data) {
        brandList.value.forEach((item1: any, index1: number) => {
          Object.keys(response.data).forEach((item2, index2) => {
            if (item1.code === item2) {
              global.$store.commit("setBrandActive", {
                index: index1,
                active: response.data[item2],
              });
              // brandList.value[index1].active = response.data[item2]
            }
          });
        });
      }
    })
    .catch((error: any) => {
      console.log(error);
    });
};
const getPrizeList = () => {
  global.$http
    .get(global.$baseUrl + state.getDrawprizelist2Request, {
      params: {
        u_id: global.$store.state.user.userInfo,
      },
    })
    .then((response: any) => {
      console.log("getPrizeList+++++", response);
      response = response.data;
      state.prizeList = state.prizeList.map((item: any, index: number) => {
        let result = {};
        if (response[index]) {
          result = {
            name: response[index].prize_Content,
            index: index + 1,
            hasPrize: true,
          };
        } else {
          result = Object.assign(item, {
            name: item.name,
            index: index + 1,
          });
        }
        return result;
      });
      console.log("state.prizeList+++++", state.prizeList);
    })
    .catch((error: any) => {
      console.log(error);
    });
};

const addscancount = () => {
  //增加扫码次数
  global.$http
    .get(global.$baseUrl + "h5.wxuser.pageaccess", {
      params: {
        u_id: global.$store.state.user.userInfo,
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
const receivetask = (type: number) => {
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
  const lightenCount = brandList.value.filter(
    (item: any) => item.active
  ).length;
  let temp1 =
    state.prizeList.filter((item: any) => item.name !== "").length * 2;
  let temp2 =
    lightenCount -
    state.prizeList.filter((item: any) => item.name !== "").length * 2;
  console.log(lightenCount);
  console.log(temp1);
  console.log(temp2);
  const notYetFlag =
    lightenCount -
      state.prizeList.filter((item: any) => item.hasPrize).length * 2 <
    2;
  if (notYetFlag) {
    state.dialogNotYetFlag = true;
  } else {
    global.$router.push({
      name: "LotteryDrawLogo",
    });
  }
};
const checkPrize = () => {
  closeDialog();
  state.dialogPrizeListFlag = true;
};
const closeDialog = () => {
  state.dialogFlag = false;
  state.dialogPositionFlag = false;
  state.dialogNotYetFlag = false;
  state.dialogLightenFlag = false;
  state.dialogPrizeListFlag = false;
};
const checkBrandInfo = (data: any) => {
  state.currentBrandData = brandList.value.find(
    (item: any) => item.taskType === data.taskType
  );
  state.dialogPositionFlag = true;
};
const getParameter = (key: any) => {
  let url = location.href;
  let paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
  let paraObj = {} as any;
  for (let i = 0, len = paraString.length; i < len; i++) {
    let j = paraString[i];
    paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(
      j.indexOf("=") + 1,
      j.length
    );
  }
  let returnValue = paraObj[key.toLowerCase()];
  if (!returnValue) {
    return "";
  } else {
    return returnValue;
  }
};

onMounted(() => {
  init();
});
</script>

<style scoped lang="scss">
.interactionlogo_main_container {
  > .content {
    .common_subtitle_item {
      margin: 0.9rem auto 0;
    }
    .rules {
      margin: 0.5rem auto;
      padding: 0;
      width: 13.5rem;
      text-align: left;
      font-size: 0.4rem;
      ol {
        li {
          margin: 0 0 0.2rem 0;
        }
      }
    }
  }
  .status_wrapper {
    ul {
      padding: 0;
      li {
        display: inline-block;
        margin: 0.5rem 0;
        padding: 0 0.8rem;
        vertical-align: middle;
        &.active {
          a {
            .enabled {
              display: block;
            }
            .disabled {
              display: none;
            }
          }
        }
        a {
          display: block;
          position: relative;
          img {
            display: inline-block;
            width: 100%;
            height: auto;
          }
          .enabled {
            display: none;
          }
          .disabled {
            // position: absolute;
            // left: 0;
            // top: 0;
          }
        }
        &.wanxin {
          margin-left: 1.5rem;
          width: 4rem;
        }
        &.yolioptical {
          margin-right: 1.5rem;
          width: 3rem;
        }
        &.chemilens {
          width: 1.8rem;
        }
        &.seeworld {
          width: 3rem;
        }
        &.creasky {
          width: 4.5rem;
        }
        &.newtianhongoptical {
          width: 4rem;
        }
        &.bolon {
          width: 3rem;
        }
      }
    }
    .hint {
      margin: 0 auto;
      width: 12.5rem;
      text-align: right;
      font-size: 0.4rem;
      color: #4d4d4d;
    }
  }
  .navigation_wrapper {
    margin: 0.5rem 0 0 0;
    width: 100%;
    text-align: center;
    .common_navigation_item {
      display: inline-block;
      margin: 0 0.5rem;
    }
  }
  .common_dialog_container {
  }
}
</style>
