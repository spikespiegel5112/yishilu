<template>
  <div class="interactionlogo_main_container">
    <div class="common_logo_wrapper">
      <div class="common_logo_item"></div>
    </div>
    <div class="common_title_item">
      <img src="@/image/common/title_00000.png" alt />
    </div>
    <!-- <div class="bg"></div> -->
    <div class="content">
      <div class="common_subtitle_item">
        <img src="@/image/interactionlogo/subtitle_00000.png" alt />
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
            <a href="javascript:;" @click="checkBrandInfo(item)">
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
          <a href="javascript:;" @click="checkStatus">点击抽奖</a>
        </div>
        <div class="common_navigation_item">
          <a href="javascript:;" @click="checkPrize">我的奖品</a>
        </div>
      </div>
      <div class="common_goback_wrapper">
        <CommonGoBack to="lotteryDraw" />
      </div>
    </div>
    <div v-if="dialogNotYetFlag" class="common_dialog_container notyet">
      <div class="dialog_wrapper">
        <a href="javascript:;" class="close" @click="closeDialog"></a>
        <div class="content">
          <p>请前往相关展台扫码点亮</p>
          <p>任意两个 Logo 进行抽奖</p>
        </div>
      </div>
    </div>
    <div v-if="dialogPositionFlag" class="common_dialog_container position">
      <div class="dialog_wrapper">
        <a href="javascript:;" class="close" @click="closeDialog"></a>
        <div class="content">
          <p class="brandname">{{ currentBrandData.brandName }}</p>
          <p>展位号</p>
          <p class="number">{{ currentBrandData.positionNumber }}</p>
          <p class="hint">*奖品数量有限，领完即止</p>
        </div>
      </div>
    </div>
    <div v-if="dialogLightenFlag" class="common_dialog_container lighten">
      <div class="dialog_wrapper">
        <a href="javascript:;" class="close" @click="closeDialog"></a>
        <div class="content">
          <p>您已点亮</p>
          <p class="brandname">{{ currentBrandData.brandName }}</p>
        </div>
      </div>
    </div>
    <div v-if="dialogPrizeListFlag" class="common_dialog_container prizelist">
      <div class="dialog_wrapper">
        <a href="javascript:;" class="close" @click="closeDialog"></a>
        <div class="content">
          <p class="title" @click="closeDialog">我的奖品</p>
          <div class="list">
            <ul>
              <li v-for="(item, index) in prizeList" :key="index">
                {{ item.name !== "" ? item.index + "." + item.name : "" }}
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "InteractionLogo",
  components: {},
  data() {
    return {
      getLighten2Request: "h5.get.wxuser.lighten2",
      getDrawprizelist2Request: "h5.get.wxuser.drawprizelist2",
      getTaskRequest: "h5.user.light.task",
      goToNextflag: false,
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
      ],
      currentBrandData: {
        brandName: "",
        positionNumber: "",
      },
    };
  },
  computed: {
    navigatorStyle() {
      return {
        "background-image":
          "url(" + require("@/image/homepage/navigator_00000.png") + ")",
      };
    },
    userInfo() {
      return this.$store.state.userInfo;
    },
    brandList() {
      return this.$store.state.brandList;
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
  },

  mounted() {
    console.log("this.$store.state++++", this.$store.state.userInfo);
    console.log("this.$store.state++++", this.userInfo);
    this.init();
  },
  methods: {
    init() {
      this.task_type = this.getParameter("task_type");
      console.log("this.task_type+++", this.task_type);
      if (this.task_type) {
        this.lightTask();
        this.addscancount();
      } else {
        this.getLighten();
      }
      this.getPrizeList();
    },
    lightTask() {
      //点亮任务
      this.$http
        .post(this.$baseUrl + this.getTaskRequest, {
          User_Id: this.userInfo.id,
          Task_Type: this.task_type,
        })
        .then((response) => {
          console.log("lightTask+++", response);
          this.dialogLightenFlag = true;
          this.currentBrandData.brandName = this.brandList.find(
            (item) => item.taskType === Number(response.data),
          ).brandName;
          this.getLighten();
        })
        .catch((error) => {
          console.log(error);
        });
    },
    getLighten() {
      //获取用户点亮的任务
      console.log("getLighten+++++++", this.$store.state);
      this.$http
        .get(this.$baseUrl + this.getLighten2Request, {
          params: { u_id: this.userInfo.id },
        })
        .then((response) => {
          if (response.data) {
            this.brandList.forEach((item1, index1) => {
              Object.keys(response.data).forEach((item2, index2) => {
                if (item1.code === item2) {
                  this.$store.commit("setBrandActive", {
                    index: index1,
                    active: response.data[item2],
                  });
                  // this.brandList[index1].active = response.data[item2]
                }
              });
            });
          }
        })
        .catch((error) => {
          console.log(error);
        });
    },
    getPrizeList() {
      this.$http
        .get(this.$baseUrl + this.getDrawprizelist2Request, {
          params: {
            u_id: this.userInfo.id,
          },
        })
        .then((response) => {
          console.log("getPrizeList+++++", response);
          response = response.data;
          this.prizeList = this.prizeList.map((item, index) => {
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
          console.log("this.prizeList+++++", this.prizeList);
        })
        .catch((error) => {
          console.log(error);
        });
    },

    addscancount() {
      //增加扫码次数
      this.$http
        .get(this.$baseUrl + "h5.wxuser.pageaccess", {
          params: { u_id: this.userInfo.id, scan_type: this.task_type },
        })
        .then((response) => {
          console.log(response);
        })
        .catch((error) => {
          console.log(error);
        });
    },
    receivetask(type) {
      //领取任务
      switch (type) {
        case 1:
          this.dialogYanFlag = true;
          break;
        case 2:
          this.dialogShiFlag = true;
          break;
        case 3:
          this.dialogGuangFlag = true;
          break;
        default:
          break;
      }
    },
    checkStatus() {
      const lightenCount = this.brandList.filter((item) => item.active).length;
      let temp1 = this.prizeList.filter((item) => item.name !== "").length * 2;
      let temp2 =
        lightenCount -
        this.prizeList.filter((item) => item.name !== "").length * 2;
      console.log(lightenCount);
      console.log(temp1);
      console.log(temp2);
      const notYetFlag =
        lightenCount -
          this.prizeList.filter((item) => item.hasPrize).length * 2 <
        2;
      if (notYetFlag) {
        this.dialogNotYetFlag = true;
      } else {
        this.$router.push({
          name: "lotteryDrawLogo",
        });
      }
    },
    checkPrize() {
      this.closeDialog();
      this.dialogPrizeListFlag = true;
    },
    closeDialog() {
      this.dialogFlag = false;
      this.dialogPositionFlag = false;
      this.dialogNotYetFlag = false;
      this.dialogLightenFlag = false;
      this.dialogPrizeListFlag = false;
    },
    checkBrandInfo(data) {
      this.currentBrandData = this.brandList.find(
        (item) => item.taskType === data.taskType,
      );
      this.dialogPositionFlag = true;
    },
    getParameter(key) {
      var url = location.href;
      var paraString = url
        .substring(url.indexOf("?") + 1, url.length)
        .split("&");
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
    },
  },
};
</script>

<style scoped></style>
