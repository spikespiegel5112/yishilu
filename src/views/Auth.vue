<template>
  <div class="common_auth_cotainer">
    <div class="content">
      <CommonLoading :loading="true"></CommonLoading>
      <div class="title">auth...</div>
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
  getUserInfoRequest: "api/security/login/info",
  environment: "",
  userInfo: {},
  userInfoFlag: false,
});

const getWeChatUserInfoByCode = (code: string) => {
  return new Promise((resolve, reject) => {
    global.$http
      .post("wx.login.openid", { code: code })
      .then((response: any) => {
        console.log("getWeChatUserInfoByCode++++", response);
        if (response.state != 200) {
          reject();
          // location.href = getOAuthUrl()
        }
        if (response.data) {
          const openId = response.data.openid;
          getUserInfoByOpenId(openId)
            .then((response: any) => {
              if (response.data) {
                sessionStorage.setItem(
                  "userInfo",
                  JSON.stringify(response.data)
                );
                global.$store.commit("setUserInfo", response.data);
                state.userInfoFlag = true;
                resolve(response.data);
              }
            })
            .catch((error: any) => {
              console.log(error);
              reject(error);
            });
        } else {
          //留在当前页
        }
      })
      .catch((error: any) => {
        console.log(error);
        reject(error);
      });
  });
};
const getOAuthUrl = () => {
  console.log("getOAuthUrl++++++", location);
  return (
    "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxb2602761a856bbea&redirect_uri=" +
    encodeURIComponent(location.href) +
    "&response_type=code&scope=snsapi_userInfo&state=STATE#wechat_redirect"
  );
};
// 获取用户信息
const redirectToBackRoute = () => {
  console.log("location+++++", location);
  let backRoute = sessionStorage.getItem("backRoute");
  let url = "";
  console.log(location.href.indexOf(".html"));

  if (location.href.indexOf(".html") > 0) {
    url = location.href.split(".html")[0] + ".html#" + backRoute;
  } else {
    url = location.origin + location.pathname + "#" + backRoute;
  }

  // alert(state.environment + ' environment url is ' + url)
  setTimeout(() => {
    location.replace(url);
  }, 100);
};
const getParameter = (key: string) => {
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
const testlogin = () => {
  return new Promise((resolve, reject) => {
    getUserInfoByOpenId("oPxr9wlKa8Gbr-dxJwWx4GSqG_1g")
      .then((response: any) => {
        if (response.data) {
          sessionStorage.setItem("userInfo", JSON.stringify(response.data));
          state.userInfoFlag = true;
          resolve(response);
        }
      })
      .catch((error: any) => {
        console.log(error);
        reject(error);
      });
  });
};
const getUserInfoByOpenId = (openId: number) => {
  return new Promise((resolve, reject) => {
    global.$http
      .get(global.$baseUrl + "wx.login.user.byopenid", {
        params: { openid: openId },
      })
      .then((response: any) => {
        if (response.data) {
          sessionStorage.setItem("userInfo", JSON.stringify(response.data));
          state.userInfoFlag = true;
          resolve(response);
        }
      })
      .catch((error: any) => {
        console.log(error);
        reject(error);
      });
  });
};
const isEmpty = (value) => {
  return typeof value === "undefined" || value === null || value === "";
};

onMounted(() => {
  console.log("environment", state.environment);

  state.environment = global.$isEmpty(sessionStorage.getItem("environment"))
    ? global.$checkEnvironment()
    : sessionStorage.getItem("environment");
  state.userInfo = sessionStorage.getItem("userInfo");
  const authToken = sessionStorage.getItem("authToken");

  // debugger
  console.log("location+++", location);
  if (state.environment === "wechat") {
    // if (state.environment === 'others') {
    // alert('begin auth+++++' + state.environment)
    // alert('state.userInfo+++++' + state.userInfo)

    const code = getParameter("code");
    if (global.$isEmpty(state.userInfo) || state.userInfo === "null") {
      if (global.$isEmpty(code)) {
        // alert('isEmpty code+++++' + code)

        location.href = getOAuthUrl();
      } else {
        console.log("hascode++++++", code);
        getWeChatUserInfoByCode(code)
          .then((response: any) => {
            console.log("hascode resolve++++++", code);

            redirectToBackRoute();
          })
          .catch((error: any) => {
            console.log("hascode reject++++++", code);
            location.href = getOAuthUrl();
          });
      }
    }
  } else {
    console.log("begin auth+++++" + state.environment);
    sessionStorage.setItem("environment", "others");
    getUserInfoByOpenId("oPxr9wlKa8Gbr-dxJwWx4GSqG_1g")
      .then((response: any) => {
        if (response.data) {
          sessionStorage.setItem("userInfo", JSON.stringify(response.data));
          global.$store.commit("setUserInfo", response.data);
          // state.userInfoFlag = true
          redirectToBackRoute();
        }
      })
      .catch((error: any) => {
        console.log(error);
      });
  }
});
</script>
