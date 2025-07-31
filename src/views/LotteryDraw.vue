<template>
  <div class="lotterydraw_main_container">
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
      <div class="wheel">
        <canvas
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
        <CommonGoBack to="Interaction" />
      </div>
    </div>
    <div
      v-if="state.dialogThankYouFlag"
      class="common_dialog_container thankyou"
    >
      <div class="dialog_wrapper">
        <a href="javascript:;" class="close" @click="closeThankYou"></a>
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
        <a href="javascript:;" class="close" @click="closePrize"></a>
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
  wheelCanvas: {},
  remUnit: "",
  alreadyReleasedPrize: false,
  alreadyReceivedPrize: false,
  rotateDuration: 3600,
  colorDictionary: ["#E6E6E6", "#1D88C2"],
  textColorDictionary: ["#1D88C2", "#E6E6E6"],
  dotsColorDictionary: ["#ffd800", "#fe9166"],
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
    }
  ],
  userInfo: {
    id: "",
  },
  prizeData: {},
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


    const init=(source)=>{
      // console.log(FrameAnimation)
    }
    const getUserInfo=(source)=>{
      state.userInfo = JSON.parse(sessionStorage.getItem("userInfo"));
    }
    const closeThankYou=(source)=>{
      this.dialogThankYouFlag = false;
    }
    const closePrize=(source)=>{
      this.dialogPrizeFlag = false;
    }
   const  getPrizeList=(source)=>{
      state.loading = true;
      // this.drawCanvas();
      // this.getCachedCircleNumber();
      global.$http
        .get(global.$baseUrl + this.drawlistRequest, {
          params: {
            u_id: global.$store.state.user.userInfo,
          }
        })
        .then((response: any) => {
          console.log("getPrizeList+++++++=", response);
          state.loading = false;
          this.wheelData = [];
          // this.activityInfo = response.activityInfo;

          // if (JSON.parse(sessionStorage.getItem('dailyLimit') === 'null')) {
          // 	this.dailyLimit = response.activityInfo.dailyLimit;
          // }
          response.data.forEach((item: any, index: number) => {
            this.wheelData.push({
              name: item.prize_Name,
              // image: item.rewardImage !== null ? item.rewardImage + '-style_100x100' : '',
              // image: 'https://pic5.40017.cn/01/000/79/0a/rBLkBVpVuxmAUQqmAAARnUFXcFc487.png',
              value: item.id,
            });
          });

          this.drawCanvas();
          this.getCachedCircleNumber();
        })
        .catch((error: any) => {
          console.log(error);
          state.loading = false;
          global.$vux.confirm.show({
            showCancelButton: false,
            title: error.data.message,
            onConfirm() {}
          });
        });
    }
  const   drawCanvas=(source)=>{
      console.log(state.remUnit);
      // console.log(state.canvasWidth)
      // console.log(this.wheelData)
      this.wheelCanvas = document.getElementById("wheelcanvas");
      let ctx = this.wheelCanvas.getContext("2d");
      let ctx2 = this.wheelCanvas.getContext("2d");

      let baseAngle = (Math.PI * 2) / this.wheelData.length;
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
      // 		ctx.fillStyle = i % 2 === 0 ? this.dotsColorDictionary[0] : this.dotsColorDictionary[1];
      // 		ctx.fill();
      // 	}
      // };
      // showDots();
      // setInterval(() => {
      // 	showDots();
      // 	this.dotsColorDictionary = this.dotsColorDictionary.reverse();
      // } 1000);

      let imageSequence = [];

      this.wheelData.forEach((item: any, index: number) => {
        let imageObj = new Image();
        imageObj.width = "150";
        imageObj.height = "150";
        // imageObj.src = this.$replaceProtocol(this.wheelData[index].image);
        imageObj.transparency = 0.2;
        imageSequence.push(imageObj);
      });

      this.wheelData.forEach((item: any, index: number) => {
        let angle = baseAngle * index;

        if (this.checkLowestCommonDivisorWith2(this.wheelData.length)) {
          angle = angle - Math.PI;
        } else {
          angle = angle - Math.PI + baseAngle;
        }

        ctx.beginPath();
        ctx.moveTo(canvasWidth / 2, canvasHeight / 2);
        ctx.lineWidth = 3;
        if (this.checkLowestCommonDivisorWith2(this.wheelData.length)) {
          ctx.arc(
            canvasWidth / 2,
            canvasHeight / 2,
            state.remUnit * 5.4,
            angle + baseAngle - Math.PI / this.wheelData.length,
            angle - Math.PI / this.wheelData.length,
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
        ctx.fillStyle = this.colorDictionary[index % 2];

        ctx.save();
        ctx.fill();

        // imageSequence[index].onload = () => {};

        setTimeout(() => {
          let translateX, translateY;
          if (this.checkLowestCommonDivisorWith2(this.wheelData.length)) {
            translateX =
              canvasWidth * 0.5 +
              Math.cos(
                angle +
                  baseAngle / 2 -
                  Math.PI / 2 -
                  Math.PI / this.wheelData.length
              ) *
                state.remUnit *
                5;
            translateY =
              canvasHeight * 0.5 +
              Math.sin(
                angle +
                  baseAngle / 2 -
                  Math.PI / 2 -
                  Math.PI / this.wheelData.length
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
          ctx.fillStyle = this.textColorDictionary[index % 2];
          ctx.translate(translateX, translateY);
          ctx.rotate(angle);
          // ctx.fillText(this.wheelData[index].name, -ctx.measureText(this.wheelData[index].name).width / 2, 22);
          const offset = state.remUnit / 1.7;
          this.wheelData[index].name
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
        } 500);

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
      } 1000);

      this.getCachedCircleNumber();
    }
   const  drawPrize() {
      state.loading = true;
      // 本地调试代码
      console.log(Math.random());
      console.log(Math.ceil((this.wheelData.length - 1) * Math.random()));
      console.log(
        this.wheelData.find(
          (item, index) =>
            index === Math.ceil((this.wheelData.length - 1) * Math.random())
        )
      );
      // let index = 0
      // this.wheelData.forEach((item1, index1) => {
      // 	if (index1 === Math.ceil((this.wheelData.length - 1) * Math.random())) {
      // 		index = index1
      // 		console.log('match+++++++', item1)
      // 		console.log('match+++++++', index)

      // 	}
      // })
      // if (!this.alreadyReleasedPrize) {
      // 	this.rotateWheel(index).then(() => {

      // 		this.alreadyReleasedPrize = true;

      // 		this.dailyLimit = Number(this.dailyLimit) > 0 ? Number(this.dailyLimit) - 1 : this.dailyLimit;

      // 	}).catch(error => {
      // 	})
      // }

      console.log(" userInfo.value.id", userInfo.value);
      global.$http
        .post(this.drawRequest, {
          u_id: global.$store.state.user.userInfo,
        })
        .then((response: any) => {
          console.log(response);
          state.loading = false;
          response = response.data;
          this.prizeData = response;
          this.rewardCode = response.rewardCode;
          if (response.drawCount < 2) {
            this.wheelData.forEach((item1, index1) => {
              if (item1.value === response.id) {
                this.rotateWheel(index1).then(() => {
                  this.alreadyReleasedPrize = true;
                  if (response.id < 5) {
                    this.dialogPrizeFlag = true;
                  } else {
                    this.dialogThankYouFlag = true;
                  }
                });
              }
            });
          } else {
            if (response.id < 5) {
              this.dialogPrizeFlag = true;
            } else {
              this.dialogThankYouFlag = true;
            }
          }
        })
        .catch((error: any) => {
          state.loading = false;
          console.log(error);
        });
    }
   const  checkLowestCommonDivisorWith2=(source)=>{
      let flag = true;
      for (let i = 2; i < source; i++) {
        source = source / 2;
        if (source !== 2) {
          flag = source % 2 === 0;
        }
      }
      return flag;
    }
   const  getCachedCircleNumber=()=>{
      if (
        sessionStorage.getItem("actualRotate") !== undefined &&
        sessionStorage.getItem("actualRotate") !== 0
      ) {
        this.actualRotate = sessionStorage.getItem("actualRotate");
        let offsetAngle = this.actualRotate % 360;
        // alert(offsetAngle)
        // this.wheelCanvas.style.transform = 'rotate(' + this.actualRotate + 'deg)';
        this.wheelCanvas.style.transform = "rotate(" + offsetAngle + "deg)";
      }
    }
   const  rotateWheel(offset) {
      return new Promise((resolve, reject) => {
        console.log(111, offset);

        //因为canvas绘图顺序为顺时针，旋转顺序也为顺时针的话，旋转过后的个数会从最大值往最小值数，所以索性对偏移的个数进行取反

        offset = this.wheelData.length - offset - 4;
        let initRotateAngle = 3600;
        let unitAngle = 360 / this.wheelData.length;
        console.log(222, initRotateAngle + unitAngle * offset);
        this.actualRotate = initRotateAngle + unitAngle * offset;
        // alert(this.actualRotate)
        this.wheelCanvas.style.transform =
          "rotate(-" + this.actualRotate + "deg)";

        sessionStorage.setItem("actualRotate", this.actualRotate);
        if (!this.rotatingFlag) {
          this.rotatingFlag = true;
          this.wheelCanvas.style.transition =
            "all " + this.rotateDuration / 1000 + "s ease";
          this.wheelCanvas.style.transform =
            "rotate(" + this.actualRotate + "deg)";
          setTimeout(() => {
            this.rotatingFlag = false;
            this.wheelCanvas.style.transition = "rotate 0s ease";
            resolve();
          } this.rotateDuration);
        } else {
          reject();
        }
      });
    }


onMounted(() => {
  setTimeout(() => {
    init();
  } 100);
  nextTick(() => {
    state.remUnit = Number(
      document.getElementsByTagName("html")[0].style.fontSize.replace("px", "")
    );
  });
  getUserInfo();
});
</script>

<style scoped></style>
