<template>
  <div class="lotterydraw_main_container">
    <div class="common_logo_wrapper">
      <div class="common_logo_item"></div>
    </div>
    <div class="common_title_item">
      <img src="@/assets/image/common/title_00000.png" />
    </div>
    <div class="content">
      <div class="common_subtitle_item">
        <img src="@/assets/image/interaction/subtitle_00000.png" />
      </div>
      <div class="wheel">
        <canvas
          id="wheelcanvas"
          :width="state.remUnit * 13.5"
          :height="state.remUnit * 13.5"
        >
          抱歉！浏览器不支持。
        </canvas>
        <a class="begin" @click="drawPrize"></a>
      </div>
      <div class="hint">
        <p>奖品请至依视路展台领取</p>
      </div>
      <div class="common_goback_wrapper">
        <CommonGoBack to="Interaction" />
      </div>
    </div>
    <div
      v-if="state.dialogThankYouFlag"
      class="common_dialog_container thankyou"
    >
      <div class="dialog_wrapper">
        <a class="close" @click="closeThankYou"></a>
        <div class="content">
          <p>谢谢参与</p>
        </div>
        <div class="otheractivity">
          <router-link :to="{ name: 'InteractionLogo' }">
            *请点击此处，了解额外互动活动
          </router-link>
        </div>
      </div>
    </div>
    <div v-if="state.dialogPrizeFlag" class="common_dialog_container prize">
      <div class="dialog_wrapper">
        <a class="close" @click="closePrize"></a>
        <div class="content">
          <div class="title">
            <img src="@/assets/image/lotterydraw/gift_00000.png" />
          </div>
          <div class="desc">
            <div class="prizeinfo">
              <p class="degree">恭喜您获得{{ state.prizeData.prize_Name }}</p>
              <p class="prize">
                {{ state.prizeData.prize_Content }}{{ state.prizeData.unit }}
              </p>
            </div>
            <p class="hint">{{ state.prizeData.remark }}</p>
          </div>
        </div>
        <div class="otheractivity">
          <router-link :to="{ name: 'InteractionLogo' }">
            *请点击此处，了解额外互动活动
          </router-link>
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
  drawlistRequest: "h5.get.wxuser.drawlist",
  drawRequest: "h5.user.luck.draw",
  goToNextflag: false,
  dialogThankYouFlag: false,
  dialogPrizeFlag: false,
  status: false,
  rotatingFlag: false,
  actualRotate: 0,
  alreadyReleasedPrize: false,
  alreadyReceivedPrize: false,
  rotateDuration: 3600,
  colorDictionary: ["#E6E6E6", "#1D88C2"],
  textColorDictionary: ["#1D88C2", "#E6E6E6"],
  dotsColorDictionary: ["#1D88C2", "#E6E6E6"],
  wheelData: [
    {
      name: "比萨饼",
      value: 10,
      image:
        "https://pic5.40017.cn/01/000/79/0a/rBLkBVpVuxmAUQqmAAARnUFXcFc487.png",
    },
    {
      name: "酱肘子",
      value: 20,
      image:
        "http://upload.wikimedia.org/wikipedia/commons/thumb/4/47/PNG_transparency_demonstration_1.png/280px-PNG_transparency_demonstration_1.png",
    },
    {
      name: "红烧肉",
      value: 30,
      image: "http://pic.c-ctrip.com/platform/online/home/un_index_supply.png",
    },
    {
      name: "炖排骨",
      value: 30,
      image: "http://pic.c-ctrip.com/platform/online/home/un_index_supply.png",
    },
    {
      name: "小鸡炖蘑菇",
      value: 30,
      image: "http://pic.c-ctrip.com/platform/online/home/un_index_supply.png",
    },
    {
      name: "牛排",
      value: 30,
      image: "http://pic.c-ctrip.com/platform/online/home/un_index_supply.png",
    },
    {
      name: "小鸡炖蘑菇",
      value: 30,
      image: "http://pic.c-ctrip.com/platform/online/home/un_index_supply.png",
    },
    {
      name: "牛排",
      value: 30,
      image: "http://pic.c-ctrip.com/platform/online/home/un_index_supply.png",
    },
  ],
  userInfo: {
    id: "",
  },
  prizeData: {
    prize_Name: "",
    prize_Content: "",
    unit: "",
    remark: "",
  },
  rewardCode: "",
  remUnit: 0,
  canvasWidth: "",
  canvasHeight: "",
  loading: false,
  dailyLimit: 10,
});

let wheelCanvas: any;

const userInfo = computed(() => {
  return global.$store.state.user.userInfo;
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
    if (newValue === true) {
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
      state.canvasWidth = newValue * 13.5 + "px";
      state.canvasHeight = newValue * 13.5 + "px";
      getPrizeList();
    });
  }
);

const init = () => {};

const closeThankYou = () => {
  state.dialogThankYouFlag = false;
};
const closePrize = () => {
  state.dialogPrizeFlag = false;
};
const getPrizeList = () => {
  state.loading = true;
  console.log(state.wheelData);
  drawCanvas();
  getCachedCircleNumber();
  // global.$http
  //   .get(global.$baseUrl + state.drawlistRequest, {
  //     params: {
  //       u_id: global.$store.state.user.userInfo,
  //     },
  //   })
  //   .then((response: any) => {
  //     console.log("getPrizeList+++++++=", response);
  //     state.loading = false;
  //     state.wheelData = [];
  //     response.data.forEach((item: any, index: number) => {
  //       state.wheelData.push({
  //         name: item.prize_Name,
  //         value: item.id,
  //       });
  //     });
  //     drawCanvas();
  //     getCachedCircleNumber();
  //   })
  //   .catch((error: any) => {
  //     console.log(error);
  //     state.loading = false;
  //   });
};
const drawCanvas = () => {
  console.log(state.remUnit);
  wheelCanvas = document.getElementById("wheelcanvas");
  let ctx = wheelCanvas.getContext("2d");
  let ctx2 = wheelCanvas.getContext("2d");

  let baseAngle = (Math.PI * 2) / state.wheelData.length;

  let canvasWidth = state.remUnit * 13.5;
  let canvasHeight = state.remUnit * 13.5;

  ctx.font = state.remUnit;

  ctx2.beginPath();
  ctx2.arc(
    canvasWidth / 2,
    canvasHeight / 2,
    state.remUnit * 6.75,
    0,
    Math.PI * 2,
    true
  );
  ctx2.fillStyle = "#51a5ff";
  ctx2.fill();

  ctx2.beginPath();
  ctx2.arc(
    canvasWidth / 2,
    canvasHeight / 2,
    state.remUnit * 6.57,
    0,
    Math.PI * 2,
    true
  );
  ctx2.fillStyle = "#003e81";
  ctx2.fill();

  ctx2.beginPath();
  ctx2.arc(
    canvasWidth / 2,
    canvasHeight / 2,
    state.remUnit * 6.35,
    0,
    Math.PI * 2,
    true
  );
  ctx2.fillStyle = "#0b57aa";
  ctx2.fill();

  ctx2.save();

  const showDots = () => {
    for (let i = 0; i < 24; i++) {
      ctx.beginPath();
      let angle = ((Math.PI * 2) / 24) * i;
      let translateX =
        canvasWidth * 0.5 + Math.cos(angle) * state.remUnit * 5.9;
      let translateY =
        canvasHeight * 0.5 + Math.sin(angle) * state.remUnit * 5.9;
      ctx.arc(
        translateX,
        translateY,
        state.remUnit * 0.35,
        state.remUnit,
        Math.PI * 2,
        true
      );
      ctx.fillStyle =
        i % 2 === 0
          ? state.dotsColorDictionary[0]
          : state.dotsColorDictionary[1];
      ctx.fill();
    }
  };
  showDots();
  setInterval(() => {
    showDots();
    state.dotsColorDictionary = state.dotsColorDictionary.reverse();
  }, 1000);

  let imageSequence = [];

  state.wheelData.forEach((item: any, index: number) => {
    let imageObj = new Image();
    imageObj.width = 150;
    imageObj.height = 150;
    // imageObj.src = this.$replaceProtocol(state.wheelData[index].image);
    // imageObj.transparency = 0.2;
    imageSequence.push(imageObj);
  });

  state.wheelData.forEach((item: any, index: number) => {
    let angle = baseAngle * index;

    if (checkLowestCommonDivisorWith2(state.wheelData.length)) {
      angle = angle - Math.PI;
    } else {
      angle = angle - Math.PI + baseAngle;
    }

    ctx.beginPath();
    ctx.moveTo(canvasWidth / 2, canvasHeight / 2);
    ctx.lineWidth = 3;
    if (checkLowestCommonDivisorWith2(state.wheelData.length)) {
      ctx.arc(
        canvasWidth / 2,
        canvasHeight / 2,
        state.remUnit * 5.4,
        angle + baseAngle - Math.PI / state.wheelData.length,
        angle - Math.PI / state.wheelData.length,
        true
      );
    } else {
      ctx.arc(
        canvasWidth / 2,
        canvasHeight / 2,
        state.remUnit * 5.4,
        angle + baseAngle,
        angle,
        true
      );
    }
    ctx.lineTo(canvasWidth / 2, canvasHeight / 2);

    ctx.strokeStyle = "#4387BD";
    ctx.stroke();
    ctx.fillStyle = state.colorDictionary[index % 2];

    ctx.save();
    ctx.fill();

    // imageSequence[index].onload = () => {};

    setTimeout(() => {
      let translateX, translateY;
      if (checkLowestCommonDivisorWith2(state.wheelData.length)) {
        translateX =
          canvasWidth * 0.5 +
          Math.cos(
            angle +
              baseAngle / 2 -
              Math.PI / 2 -
              Math.PI / state.wheelData.length
          ) *
            state.remUnit *
            5;
        translateY =
          canvasHeight * 0.5 +
          Math.sin(
            angle +
              baseAngle / 2 -
              Math.PI / 2 -
              Math.PI / state.wheelData.length
          ) *
            state.remUnit *
            5;
      } else {
        translateX =
          canvasWidth * 0.5 +
          Math.cos(angle + baseAngle / 2) * state.remUnit * 5;
        translateY =
          canvasHeight * 0.5 +
          Math.sin(angle + baseAngle / 2) * state.remUnit * 5;
      }

      ctx.font = state.remUnit * 0.6 + "px Georgia";
      ctx.fillStyle = state.textColorDictionary[index % 2];
      ctx.translate(translateX, translateY);
      ctx.rotate(angle);
      // ctx.fillText(state.wheelData[index].name, -ctx.measureText(state.wheelData[index].name).width / 2, 22);
      const offset = state.remUnit / 1.7;
      state.wheelData[index].name
        .split("")
        .forEach((item: any, index: number) => {
          if (index < 6) {
            ctx.fillText(
              item,
              -ctx.measureText(item).width / 2,
              (index * state.remUnit) / 1.4 + offset
            );
          }
        });
      ctx.shadowColor = "#000"; // green for demo purposes
      ctx.shadowBlur = 10;
      ctx.shadowOffsetX = 0;
      ctx.shadowOffsetY = 0;

      ctx.restore();
      ctx.save();
    }, 500);

    // ctx.restore();
    // ctx.save();
    // let pointer = new Image();
    // pointer.url = 'http://localhost/static/img/pointer_00000.png';
    // pointer.width = '100';
    // pointer.height = '100';
  });
  setTimeout(() => {
    ctx.translate(200, 150);
    ctx.rotate(-Math.PI / 4);

    ctx.restore();
    ctx.save();
  }, 1000);

  getCachedCircleNumber();
};

const drawPrizeLocalPromise = () => {
  return new Promise((resolve, reject) => {
    let index = 0;
    state.wheelData.forEach((item1, index1) => {
      if (index1 === Math.ceil((state.wheelData.length - 1) * Math.random())) {
        index = index1;
        console.log("match+++++++", item1);
        console.log("match+++++++", index);
      }
    });
    if (!state.alreadyReleasedPrize) {
      rotateWheel(index)
        .then(() => {
          state.alreadyReleasedPrize = true;
          state.dailyLimit =
            Number(state.dailyLimit) > 0
              ? Number(state.dailyLimit) - 1
              : state.dailyLimit;
          resolve({
            drawCount: 2,
            id: 2,
            rewardCode: "",
            prize_Name: state.wheelData[index].name,
            prize_Content: "",
            unit: 1,
            remark: "",
          });
        })
        .catch((error) => {
          console.log(error);
          reject(error);
        });
    }
  });
};

const drawPrizeCallback = (response: any) => {
  console.log(response);
  state.loading = false;
  state.prizeData = response;
  state.rewardCode = response.rewardCode;
  if (response.drawCount < 2) {
    state.wheelData.forEach((item1, index1) => {
      if (item1.value === response.id) {
        rotateWheel(index1).then(() => {
          state.alreadyReleasedPrize = true;
          if (response.id < 5) {
            state.dialogPrizeFlag = true;
          } else {
            state.dialogThankYouFlag = true;
          }
        });
      }
    });
  } else {
    if (response.id < 5) {
      state.dialogPrizeFlag = true;
    } else {
      state.dialogThankYouFlag = true;
    }
  }
};
const drawPrize = () => {
  state.loading = true;
  // 本地调试代码
  drawPrizeLocalPromise().then((response) => {
    drawPrizeCallback(response);
  });
  // global.$http
  //   .post(this.drawRequest, {
  //     u_id: global.$store.state.user.userInfo,
  //   })
  //   .then((response: any) => {
  //     console.log(response);
  //     state.loading = false;
  //     response = response.data;
  //     state.prizeData = response;
  //     state.rewardCode = response.rewardCode;
  //     if (response.drawCount < 2) {
  //       state.wheelData.forEach((item1, index1) => {
  //         if (item1.value === response.id) {
  //           rotateWheel(index1).then(() => {
  //             state.alreadyReleasedPrize = true;
  //             if (response.id < 5) {
  //               state.dialogPrizeFlag = true;
  //             } else {
  //               state.dialogThankYouFlag = true;
  //             }
  //           });
  //         }
  //       });
  //     } else {
  //       if (response.id < 5) {
  //         state.dialogPrizeFlag = true;
  //       } else {
  //         state.dialogThankYouFlag = true;
  //       }
  //     }
  //   })
  //   .catch((error: any) => {
  //     state.loading = false;
  //     console.log(error);
  //   });
};
const checkLowestCommonDivisorWith2 = (source: number) => {
  let flag = true;
  for (let i = 2; i < source; i++) {
    source = source / 2;
    if (source !== 2) {
      flag = source % 2 === 0;
    }
  }
  return flag;
};
const getCachedCircleNumber = () => {
  const actualRotate: string | null = sessionStorage.getItem("actualRotate");
  if (actualRotate && Number(actualRotate) !== 0) {
    state.actualRotate = Number(actualRotate);
    let offsetAngle = state.actualRotate % 360;
    // alert(offsetAngle)
    // wheelCanvas.style.transform = 'rotate(' + state.actualRotate + 'deg)';
    wheelCanvas.style.transform = "rotate(" + offsetAngle + "deg)";
  }
};
const rotateWheel = (offset: any) => {
  return new Promise((resolve, reject) => {
    console.log(111, offset);

    //因为canvas绘图顺序为顺时针，旋转顺序也为顺时针的话，旋转过后的个数会从最大值往最小值数，所以索性对偏移的个数进行取反

    offset = state.wheelData.length - offset - 4;
    let initRotateAngle = 3600;
    let unitAngle = 360 / state.wheelData.length;
    state.actualRotate = initRotateAngle + unitAngle * offset;
    // alert(state.actualRotate)
    wheelCanvas.style.transform = "rotate(-" + state.actualRotate + "deg)";

    sessionStorage.setItem("actualRotate", state.actualRotate.toString());
    if (!state.rotatingFlag) {
      state.rotatingFlag = true;
      wheelCanvas.style.transition =
        "all " + state.rotateDuration / 1000 + "s ease";
      wheelCanvas.style.transform = "rotate(" + state.actualRotate + "deg)";
      setTimeout(() => {
        state.rotatingFlag = false;
        wheelCanvas.style.transition = "rotate 0s ease";
        resolve();
      }, state.rotateDuration);
    } else {
      reject();
    }
  });
};

onMounted(() => {
  setTimeout(() => {
    init();
  }, 100);
  nextTick(() => {
    state.remUnit = Number(
      document.getElementsByTagName("html")[0].style.fontSize.replace("px", "")
    );
  });
});
</script>

<style scoped lang="scss">
.lotterydraw_main_container {
  width: 100%;
  overflow: hidden;

  .bg {
    width: 100%;
    height: 100vh;
    // background-image: url("@/assets/image/interaction/bg_interaction_00000.png");
    background-repeat: no-repeat;
    background-size: contain;
    position: absolute;
    top: 0;
    left: 0;
  }

  .content {
    margin: 1rem 0 0 0;
    position: relative;

    .wheel {
      margin: 0 auto 1.5rem;
      width: 14rem;
      height: 14rem;
      background-image: url("./image/lotterydraw/whell_bg_00000.png");
      background-size: contain;
      position: relative;
      text-align: center;

      &:before {
        content: "";
        display: inline-block;
        width: 0;
        height: 100%;
        vertical-align: middle;
      }

      canvas {
        display: inline-block;
        width: 14rem;
        height: 14rem;
        transform: rotate(0deg);
        transition: rotate 6s ease;
        vertical-align: middle;
      }

      .begin {
        display: inline-block;
        width: 3rem;
        height: 3rem;
        background-image: url("@/assets/image/lotterydraw/pointer_begin_00000.png");
        background-size: 100%;
        position: absolute;
        top: 5.35rem;
        left: 5.5rem;

        span {
          display: block;
          margin: 1.4rem auto;
          width: 1rem;
          color: #fff;
          font-size: 0.5rem;
        }
      }
    }

    .hint {
      p {
        width: 100%;
        text-align: center;
        color: #1d88c2;
        font-size: 0.6rem;
      }
    }
  }
}
</style>
