const app = {
  namespaced: true,
  state: {
    title: "aaa",
    accessToken: "",
    // accessToken: 'eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJhZG1pbiIsImNyZWF0ZWQiOjE1NDI2MTYxMTYzMzcsImV4cCI6MTU0Mjg3NTMxNiwidXNlciI6IntcImlkXCI6MSxcIm5hbWVcIjpcImFkbWluXCIsXCJuaWNrXCI6XCJhZG1pblwiLFwic3RhdHVzXCI6XCJhY3RpdmVcIixcImVtYWlsXCI6bnVsbCxcInBob25lXCI6bnVsbCxcImxhc3RQYXNzd29yZFJlc2V0RGF0ZVwiOm51bGwsXCJzZXNzaW9uS2V5XCI6bnVsbH0ifQ.BdNUqdSAYGwfR-SxWqupWpWb5AQUhIMGIpF2K7yFSthCr3--3LSjCYTAT9Pf9quNoJAXlQzGZklg1tvdOgSFQg',
    // loginId: '',
    loginId: "18260045855",
    shareUrl: "http://essilorevent.digital-support.cn/h5/",

    bgmUrl:
      "http://pp-jgxzq.oss-cn-qingdao.aliyuncs.com/ctzcf/ctzcf_entrance.mp3",
    speechUrl:
      "http://pp-jgxzq.oss-cn-qingdao.aliyuncs.com/ctzcf/ctzcf_speech.mp3",
    daishangerjiUrl:
      "http://pp-jgxzq.oss-cn-qingdao.aliyuncs.com/music/daishangerji.mp3",

    userInfo: {
      id: "",
    },
    filmInfoData: {},
    WeixinJSBridgeInitFlag: false,
    WeixinJSBridgeReadyFlag: false,
    bgmFlag: false,
    speechFlag: false,
    openEntrance: false,
    currentLoadingPercentage: 0,
    brandList: [
      {
        taskType: 4,
        prizeId: 14,
        code: "f_four",
        name: "wanxin",
        brandName: "万新",
        positionNumber: "1G02-1J02",
        enableSrc: require("@/assets/image/interactionlogo/logo_wanxin_00000.png"),
        disabledSrc: require("@/assets/image/interactionlogo/logo_wanxin_disabled_00000.png"),
        active: false,
      },
      {
        taskType: 5,
        prizeId: 14,
        code: "f_five",
        name: "yolioptical",
        brandName: "优立",
        positionNumber: "1T40-1U43",
        enableSrc: require("@/assets/image/interactionlogo/logo_yolioptical_00000.png"),
        disabledSrc: require("@/assets/image/interactionlogo/logo_yolioptical_disabled_00000.png"),
        active: false,
      },
      {
        taskType: 6,
        prizeId: 14,
        code: "f_six",
        name: "chemilens",
        brandName: "凯米",
        positionNumber: "1P32",
        enableSrc: require("@/assets/image/interactionlogo/logo_chemilens_00000.png"),
        disabledSrc: require("@/assets/image/interactionlogo/logo_chemilens_disabled_00000.png"),
        active: false,
      },
      {
        taskType: 7,
        prizeId: 14,
        code: "f_seven",
        name: "seeworld",
        brandName: "视悦",
        positionNumber: "1S22-1T27",
        enableSrc: require("@/assets/image/interactionlogo/logo_seeworld_00000.png"),
        disabledSrc: require("@/assets/image/interactionlogo/logo_seeworld_disabled_00000.png"),
        active: false,
      },
      {
        taskType: 8,
        prizeId: 14,
        code: "f_eight",
        name: "creasky",
        brandName: "奥天",
        positionNumber: "2C30",
        enableSrc: require("@/assets/image/interactionlogo/logo_creasky_00000.png"),
        disabledSrc: require("@/assets/image/interactionlogo/logo_creasky_disabled_00000.png"),
        active: false,
      },
      {
        taskType: 9,
        prizeId: 14,
        code: "f_nine",
        name: "newtianhongoptical",
        brandName: "新天鸿",
        positionNumber: "4B62-4C73",
        enableSrc: require("@/assets/image/interactionlogo/logo_newtianhongoptical_00000.png"),
        disabledSrc: require("@/assets/image/interactionlogo/logo_newtianhongoptical_disabled_00000.png"),
        active: false,
      },
      {
        taskType: 10,
        prizeId: 14,
        code: "f_ten",
        name: "bolon",
        brandName: "雅瑞",
        positionNumber: "1F48-1G55",
        enableSrc: require("@/assets/image/interactionlogo/logo_bolon_00000.png"),
        disabledSrc: require("@/assets/image/interactionlogo/logo_bolon_disabled_00000.png"),
        active: false,
      },
    ],
  },
  mutations: {
    openEntrance(state: any, payload: any) {
      state.openEntrance = true;
    },
    updateLoadingPercentage(state: any, payload: any) {
      state.currentLoadingPercentage = payload;
    },
    chooseFlagList(state: any, payload: any) {
      state.flagList[payload].chosen = !state.flagList[payload].chosen;
    },
    chooseBg(state: any, payload: any) {
      state.backgroundList.forEach((item: any, index: number) => {
        state.backgroundList[index].active = false;
      });
      state.backgroundList[payload].active = true;
    },
    turnOffWinningPrizeChance(state: any, payload: any) {
      state.winningPrizeChcation.href = url;
    },
    setAccessToken(state: any, payload: any) {
      state.accessToken = payload;
    },
    setUserLoginId(state: any, payload: any) {
      state.loginId = payload;
    },
    setUserInfo(state: any, payload: any) {
      state.userInfo.value = payload;
    },
    setFilmInfoData(state: any, payload: any) {
      state.filmInfoData = payload;
    },
    setWeixinJSBridgeReadyFlag(state: any, payload: any) {
      state.WeixinJSBridgeReadyFlag = payload;
    },
    setBrandActive(state: any, payload: any) {
      state.brandList.value[payload.index].active = payload.active;
    },
  },
  actions: {},
};

export default app;
