import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex);

let testPrefix = process.env.NODE_ENV === 'production' ? 'production' : 'test';

// alert(testPrefix === 'test' ? 'http://jgxzq.pengpai.nplusgroup.com/test/index.html' : `https://projects.thepaper.cn/jsp/jiaguo/index.html`)
const state = {
  title: 'aaa',
  accessToken: '',
  // accessToken: 'eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJhZG1pbiIsImNyZWF0ZWQiOjE1NDI2MTYxMTYzMzcsImV4cCI6MTU0Mjg3NTMxNiwidXNlciI6IntcImlkXCI6MSxcIm5hbWVcIjpcImFkbWluXCIsXCJuaWNrXCI6XCJhZG1pblwiLFwic3RhdHVzXCI6XCJhY3RpdmVcIixcImVtYWlsXCI6bnVsbCxcInBob25lXCI6bnVsbCxcImxhc3RQYXNzd29yZFJlc2V0RGF0ZVwiOm51bGwsXCJzZXNzaW9uS2V5XCI6bnVsbH0ifQ.BdNUqdSAYGwfR-SxWqupWpWb5AQUhIMGIpF2K7yFSthCr3--3LSjCYTAT9Pf9quNoJAXlQzGZklg1tvdOgSFQg',
  // loginId: '',
  loginId: '18260045855',
  shareUrl: testPrefix === 'test' ? 'http://h5.antisony.org' : `https://projects.thepaper.cn/jsp/jiaguo/spring/index.html#/entrance`,

  bgmUrl: 'http://pp-jgxzq.oss-cn-qingdao.aliyuncs.com/ctzcf/ctzcf_entrance.mp3',
  speechUrl: 'http://pp-jgxzq.oss-cn-qingdao.aliyuncs.com/ctzcf/ctzcf_speech.mp3',
  daishangerjiUrl: 'http://pp-jgxzq.oss-cn-qingdao.aliyuncs.com/music/daishangerji.mp3',

  userInfo: {},
  filmInfoData: {},
  WeixinJSBridgeInitFlag: false,
  WeixinJSBridgeReadyFlag: false,
  bgmFlag: false,
  speechFlag: false,
  openEntrance: false,
  currentLoadingPercentage: 0,

};

const mutations = {
  openEntrance(state, payload) {
    state.openEntrance = true;
  },
  updateLoadingPercentage(state, payload) {
    state.currentLoadingPercentage = payload;
  },
  chooseFlagList(state, payload) {
    state.flagList[payload].chosen = !state.flagList[payload].chosen;
  },
  chooseBg(state, payload) {
    state.backgroundList.forEach((item, index) => {
      state.backgroundList[index].active = false;
    });
    state.backgroundList[payload].active = true;
  },
  turnOffWinningPrizeChance(state, payload) {
    state.winningPrizeChcation.href = url
  },
  setAccessToken(state, payload) {
    state.accessToken = payload;
  },
  setUserLoginId(state, payload) {
    state.loginId = payload;
  },
  setUserInfo(state, payload) {
    // debugger
    state.userInfo = payload;
  },
  setFilmInfoData(state, payload) {
    state.filmInfoData = payload;
  },
  setWeixinJSBridgeReadyFlag(state, payload) {
    state.WeixinJSBridgeReadyFlag = payload;
  }

};
const actions = {};

export default new Vuex.Store({
  state,
  mutations
});
