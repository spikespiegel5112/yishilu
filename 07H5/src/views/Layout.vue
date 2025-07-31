<template>
  <div class="common_main_container layout">
    <transition name="fade">
      <router-view v-if="state.userInfo.valueFlag"></router-view>
      <CommonLoading v-else :loading="true" />
    </transition>
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
  code: "",
  environment: "",
  userInfoFlag: false,
  userInfo: {
    id: "",
  },
});

const getUserInfoCache = () => {
  const userInfoCache = JSON.parse(sessionStorage.getItem("userInfo"));
  global.$store.commit("setUserInfo", userInfoCache);
  state.userInfo.valueFlag = true;
};

const testlogin = () => {
  const userInfoCache = sessionStorage.getItem("userInfo");
  if (!userInfoCache) {
    global.$http
      .get(global.$baseUrl + "wx.login.user.byopenid", {
        params: { openid: "oPxr9wlKa8Gbr-dxJwWx4GSqG_1g" },
      })
      .then((response: any) => {
        if (response.data) {
          sessionStorage.setItem("userInfo", JSON.stringify(response.data));
          global.$store.commit("setUserInfo", response.data);
          
          setTimeout(() => {
            state.userInfo.valueFlag = true;
          }, 200);
        }
      })
      .catch((error:any) => {
        console.log(error);
      });
  } else {
    global.$store.commit("setUserInfo", response.data);
  }
};

const auto_login = () => {
  global.$http
    .post("wx.login.openid", {
      code: state.code,
    })
    .then((response: any) => {
      console.log(response);
      // if (response.state != 200) {
      // 	location.href = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxb2602761a856bbea&redirect_uri=" + location.href + "&response_type=code&scope=snsapi_userInfo&state=STATE#wechat_redirect";
      // } else {

      // }
      if (response.data) {
        sessionStorage.setItem("userInfo", JSON.stringify(response.data));
        global.$store.commit("setUserInfo", response.data);
        // state.userInfo.valueFlag = true
        getUserInfoCache(response.data);
      } else {
        //留在当前页
      }
    })
    .catch((error:any) => {
      console.log(error);
    });
};
const getParameter = (key) => {
  var url = location.href;
  var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
  var paraObj = {};
  for (var i = 0, len = paraString.length; i < len; i++) {
    var j = paraString[i];
    paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(
      j.indexOf("=") + 1,
      j.length
    );
  }
  var returnValue = paraObj[key.toLowerCase()];
  if (typeof returnValue === "undefined") {
    return "";
  } else {
    return returnValue;
  }
};
const getUserInfo = () => {
  state.code = getParameter("code");

  let userInfo = sessionStorage.getItem("userInfo");
  if (userInfo) {
    userInfo = JSON.parse(userInfo);
    global.$http
      .get(global.$baseUrl + "wx.login.user.byopenid", {
        params: { openid: userInfo.openid },
      })
      .then((response: any) => {
        if (response.data) {
          state.userInfo.valueFlag = true;
        } else {
          // location.href = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxb2602761a856bbea&redirect_uri=" +
          // 	encodeURIComponent(location.href) + "&response_type=code&scope=snsapi_userInfo&state=STATE#wechat_redirect";
          console.log(response.data);
        }
      })
      .catch((error:any) => {
        console.log(error);
      });
  } else {
    if (state.code) {
      auto_login();
    } else {
      location.href =
        "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxb2602761a856bbea&redirect_uri=" +
        encodeURIComponent(location.href) +
        "&response_type=code&scope=snsapi_userInfo&state=STATE#wechat_redirect";
    }
  }
};

// 设置sharePage分享信息
const sharePage = () => {
  let params = {
    share: true, // true可以分享；false不可以分享
    data: {
      title: `依视路•见未来•创视纪`, // 分享标题
      desc: "2020第十二届中国（上海）国际眼睛业展览会邀请函", // 分享描述
      link: global.$store.state.shareUrl, // 分享链接
      imgUrl:
        "http://img5.imgtn.bdimg.com/it/u=4294203307,2960810096&fm=26&gp=0.jpg", // 分享图标
    },
  };
  global.$wxsdk.initConfig(params);
};

onMounted(() => {
  global.$remResizing({
    fontSize: 20,
    threshold: 768,
    aligncenter: true,
  });
  console.log("layout page++++++");
  console.log("$webStorage:", sessionStorage.type);
  getUserInfoCache();

  sharePage();

  // if (this.$checkEnvironment() === 'wechat') {
  // 	console.log('environment+++++ wechat');
  // 	// this.testlogin();//本地测试
  // 	this.getUserInfo();
  // } else {
  // 	this.testlogin();//本地测试
  // }
});
</script>

<style scoped></style>
