<template>
  <div class="lotterydraw_main_container">
    <CommonLoading :loading="state.loadingFlag" />

    <div class="common_title_item">
      <img src="@/assets/image/common/title_00000.png" />
    </div>
    <!-- <div class="bg"></div> -->
    <div class="content">
      <div class="common_subtitle_item">
        <img src="@/assets/image/interaction/subtitle_00000.png" />
      </div>
      <div class="wheel">
        <canvas
          v-if="state.resetFlag"
          id="wheelcanvas"
          :width="state.remUnit * 13.5"
          :height="state.remUnit * 13.5"
          >抱歉！浏览器不支持。</canvas
        >
        <a class="begin" @click="drawPrize"></a>
      </div>
      <div class="hint">
        <p>奖品请至依视路展台领取</p>
      </div>
      <div class="common_goback_wrapper">
        <CommonGoBack to="InteractionLogo" />
      </div>
    </div>
    <div
      v-if="state.dialogThankYouFlag"
      class="common_dialog_container thankyou"
    >
      <div class="dialog_wrapper">
        <a class="close" @click="closeDialog"></a>
        <div class="content">
          <p>谢谢参与</p>
        </div>
      </div>
    </div>
    <div v-if="state.dialogRunOutFlag" class="common_dialog_container runout">
      <div class="dialog_wrapper">
        <a class="close" @click="closeDialog"></a>
        <div class="content">
          <p>您的抽奖机会已用完</p>
        </div>
      </div>
    </div>
    <div v-if="dialogPrize2Flag" class="common_dialog_container prize2">
      <div class="dialog_wrapper">
        <a class="close" @click="closeDialog"></a>
        <div class="content">
          <div class="desc">
            <p class="congrats">恭喜您，奖品请前往</p>
            <p class="degree">{{ brandData.brandName }}</p>
            <p class="prize">{{ brandData.positionNumber }}</p>
            <p class="query">咨询及领取</p>
            <p class="hint">*奖品数量有限，领完即止</p>
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
  drawlist2Request: "h5.get.wxuser.drawlist2",
  draw2Request: "h5.user.luck.draw2",
  state.dialogThankYouFlag: false,
  dialogPrize2Flag: false,
  dialogRunOutFlag: false,
  status: false,
  resetFlag: true,
  loadingFlag: false,
  wheelCanvas: {},
  remUnit: "",
  alreadyReleasedPrize: false,
  alreadyReceivedPrize: false,
  rotateDuration: 3600,
  colorDictionary: ["#E6E6E6", "#1D88C2"],
  textColorDictionary: ["#1D88C2", "#E6E6E6"],
  dotsColorDictionary: ["#ffd800", "#fe9166"],
  loading: true,
  actualRotate: 0,
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
  // userInfo: {
  id: "",
  prizeData: {},
  rewardCode: "",
  brandData: {},
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
const brandList = computed(() => {
  return global.$store.state.app.brandList;
});

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

const init = () => {
  // console.log(FrameAnimation)
};
// getUserInfo() {
// 	userInfo.value = JSON.parse(sessionStorage.getItem('userInfo'));
// },
const closeDialog = () => {
  state.state.dialogThankYouFlag = false;
  state.dialogRunOutFlag = false;
  state.dialogPrize2Flag = false;
};
const getPrizeList = () => {
  // drawCanvas();
  // getCachedCircleNumber();
  global.$http
    .get(global.$baseUrl + state.drawlist2Request, {
      params: {
        u_id: global.$store.state.user.userInfo,
      },
    })
    .then((response: any) => {
      console.log("getPrizeList+++++++=", response);
      state.loading = false;
      state.wheelData = [];
      // state.activityInfo = response.activityInfo;

      // if (JSON.parse(sessionStorage.getItem('dailyLimit') === 'null')) {
      // 	state.dailyLimit = response.activityInfo.dailyLimit;
      // }
      response.data.forEach((item: any, index: number) => {
        state.wheelData.push({
          name: item.prize_Name,
          // image: item.rewardImage !== null ? item.rewardImage + '-style_100x100' : '',
          // image: 'https://pic5.40017.cn/01/000/79/0a/rBLkBVpVuxmAUQqmAAARnUFXcFc487.png',
          value: item.id,
        });
        // if (response.data.length === 6 && index === 5) {
        // 	for (let index = 0; index < 2; index++) {
        // 		state.wheelData.push({
        // 			name: item.prize_Name,
        // 			// image: item.rewardImage !== null ? item.rewardImage + '-style_100x100' : '',
        // 			// image: 'https://pic5.40017.cn/01/000/79/0a/rBLkBVpVuxmAUQqmAAARnUFXcFc487.png',
        // 			value: item.id,
        // 		});
        // 	}
        // }
      });

      drawCanvas();
      // getCachedCircleNumber();
    })
    .catch((error: any) => {
      console.log(error);
      state.loading = false;
      global.$vux.confirm.show({
        showCancelButton: false,
        title: error.data.message,
        onConfirm() {},
      });
    });
};
const drawCanvas = () => {
  return new Promise((resolve, reject) => {
    console.log(state.remUnit);
    // console.log(state.canvasWidth)
    // console.log(state.wheelData)
    state.wheelCanvas = document.getElementById("wheelcanvas");
    let ctx = state.wheelCanvas.getContext("2d");
    let ctx2 = state.wheelCanvas.getContext("2d");

    let baseAngle = (Math.PI * 2) / state.wheelData.length;
    // document.querySelector('.wheel_wrapper .wheel').style.width = state.remUnit * 13.5;
    // document.querySelector('.wheel_wrapper .wheel').style.height = state.remUnit * 13.5;

    let canvasWidth = state.remUnit * 13.5;
    let canvasHeight = state.remUnit * 13.5;
    // console.log(canvasWidth)
    // console.log(canvasHeight)

    ctx.font = state.remUnit;

    // ctx2.beginPath();
    // ctx2.arc(canvasWidth / 2, canvasHeight / 2, state.remUnit * 6.75, 0, Math.PI * 2, true);
    // ctx2.fillStyle = 'rgba(188,75,61,0.5)';
    // ctx2.fill();

    // ctx2.beginPath();
    // ctx2.arc(canvasWidth / 2, canvasHeight / 2, state.remUnit * 6.57, 0, Math.PI * 2, true);
    // ctx2.fillStyle = '#bc4b3d';
    // ctx2.fill();

    // ctx2.beginPath();
    // ctx2.arc(canvasWidth / 2, canvasHeight / 2, state.remUnit * 6.35, 0, Math.PI * 2, true);
    // ctx2.fillStyle = '#f06949';
    // ctx2.fill();

    // ctx2.save();

    // let showDots = () => {
    // 	for (let i = 0; i < 24; i++) {
    // 		ctx.beginPath();
    // 		let angle = Math.PI * 2 / 24 * i;
    // 		let translateX = canvasWidth * 0.5 + Math.cos(angle) * state.remUnit * 5.9;
    // 		let translateY = canvasHeight * 0.5 + Math.sin(angle) * state.remUnit * 5.9;
    // 		ctx.arc(translateX, translateY, state.remUnit * 0.35, state.remUnit, Math.PI * 2, true);
    // 		ctx.fillStyle = i % 2 === 0 ? state.dotsColorDictionary[0] : state.dotsColorDictionary[1];
    // 		ctx.fill();
    // 	}
    // };
    // showDots();
    // setInterval(() => {
    // 	showDots();
    // 	state.dotsColorDictionary = state.dotsColorDictionary.reverse();
    // }, 1000);

    let imageSequence = [];

    state.wheelData.forEach((item: any, index: number) => {
      let imageObj = new Image();
      imageObj.width = "150";
      imageObj.height = "150";
      imageObj.transparency = 0.2;
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
      resolve();
    }, 1000);

    // getCachedCircleNumber();
  });
};
const drawPrize = () => {
  if (state.loading) {
    return;
  }
  state.loading = true;
  // 本地调试代码
  console.log(Math.random());
  console.log(Math.ceil((state.wheelData.length - 1) * Math.random()));
  console.log(
    state.wheelData.find(
      (item, index) =>
        index === Math.ceil((state.wheelData.length - 1) * Math.random())
    )
  );
  // let index = 0
  // state.wheelData.forEach((item1, index1) => {
  // 	if (index1 === Math.ceil((state.wheelData.length - 1) * Math.random())) {
  // 		index = index1
  // 		console.log('match+++++++', item1)
  // 		console.log('match+++++++', index)

  // 	}
  // })
  // if (!state.alreadyReleasedPrize) {
  // 	rotateWheel(index).then(() => {

  // 		state.alreadyReleasedPrize = true;

  // 		state.dailyLimit = Number(state.dailyLimit) > 0 ? Number(state.dailyLimit) - 1 : state.dailyLimit;

  // 	}).catch(error => {
  // 	})
  // }

  console.log(" userInfo.value.id", userInfo.value);
  global.$http
    .post(state.draw2Request, {
      u_id: global.$store.state.user.userInfo,
    })
    .then((response: any) => {
      console.log(response);
      response = response.data;
      // response = {
      // 	prize_Id: 14,
      // 	prize_Name: "恭喜中奖",
      // 	zhanTai: "凯米",
      // 	zhanTai_Id: 6,
      // }

      if (response === null) {
        state.dialogRunOutFlag = true;
      } else {
        state.prizeData = response;
        state.rewardCode = response.rewardCode;
        if (response.prize_Id) {
          state.wheelData.forEach((item1, index1) => {
            if (response.prize_Id === item1.value) {
              rotateWheel(index1).then(() => {
                state.alreadyReleasedPrize = true;
                state.loading = false;

                const brandData = brandList.value.find(
                  (item2: any) => item2.taskType === response.zhanTai_Id
                );
                if (brandData) {
                  state.dialogPrize2Flag = true;
                  state.brandData = brandData;
                } else {
                  state.state.dialogThankYouFlag = true;
                }
              });
            }
          });
        } else {
          if (response.prize_Id < 5) {
            state.dialogPrize2Flag = true;
          } else {
            state.state.dialogThankYouFlag = true;
          }
        }
      }
    })
    .catch((error: any) => {
      state.loading = false;
      console.log(error);
    });
};
const checkLowestCommonDivisorWith2 = (source) => {
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
  if (
    sessionStorage.getItem("actualRotate") !== undefined &&
    sessionStorage.getItem("actualRotate") !== 0
  ) {
    state.actualRotate = sessionStorage.getItem("actualRotate");
    let offsetAngle = state.actualRotate % 360;
    // alert(offsetAngle)
    // state.wheelCanvas.style.transform = 'rotate(' + state.actualRotate + 'deg)';
    state.wheelCanvas.style.transform = "rotate(" + offsetAngle + "deg)";
  }
};
const resetWheel = () => {
  return new Promise((resolve, reject) => {
    state.resetFlag = false;
    state.loadingFlag = true;
    setTimeout(() => {
      state.resetFlag = true;
      nextTick(() => {
        drawCanvas().then((response) => {
          state.loadingFlag = false;
          resolve();
        });
      });
    }, 100);
  });
};
const rotateWheel = (offset: any) => {
  return new Promise(async (resolve, reject) => {
    console.log(111, offset);

    //因为canvas绘图顺序为顺时针，旋转顺序也为顺时针的话，旋转过后的个数会从最大值往最小值数，所以索性对偏移的个数进行取反
    if (state.actualRotate !== 0) {
      await resetWheel();
    }

    offset = state.wheelData.length - offset - 4;
    let initRotateAngle = 3600;
    let unitAngle = 360 / state.wheelData.length;
    console.log(222, initRotateAngle + unitAngle * offset);
    state.actualRotate = initRotateAngle + unitAngle * offset;

    // state.wheelCanvas.style.transition = 'all 0s ease';
    // state.wheelCanvas.style.transform = 'rotate(-0deg)';
    // state.wheelCanvas.style.transformOrigin = 'center center';

    // state.wheelCanvas.style.transform = 'rotate(-' + state.actualRotate + 'deg)';

    sessionStorage.setItem("actualRotate", state.actualRotate);
    if (!state.rotatingFlag) {
      state.rotatingFlag = true;
      state.wheelCanvas.style.transition =
        "all " + state.rotateDuration / 1000 + "s ease";
      state.wheelCanvas.style.transform =
        "rotate(" + state.actualRotate + "deg)";
      setTimeout(() => {
        state.rotatingFlag = false;
        state.wheelCanvas.style.transition = "rotate 0s ease";
        // debugger
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
  console.log("userInfo.value++++++", userInfo.value);
});
</script>

<style scoped lang="scss">
.lotterydraw_main_container {
  width: 100%;
  overflow: hidden;

  .bg {
    width: 100%;
    height: 100vh;
    // background-image: url("image/interaction/bg_interaction_00000.png");
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
      width: 12rem;
      height: 12rem;
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
        width: 10rem;
        height: 10rem;
        transform: rotate(0deg);
        transition: rotate 6s ease;
        vertical-align: middle;
      }

      .begin {
        display: inline-block;
        width: 3rem;
        height: 3rem;
        background-image: url("./image/lotterydraw/pointer_begin_00000.png");
        background-size: 100%;
        position: absolute;
        top: 4.35rem;
        left: 4.5rem;

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
