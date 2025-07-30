<template>
  <router-view></router-view>
</template>

<script>
// import {getAndStoreUserInfo} from '../api/auth.js'

export default {
  name: "app",
  components: {},
  data() {
    return {};
  },
  mounted() {},
  methods: {
    // 设置sharePage分享信息
    getUserInfoCache() {
      if (!this.$isEmpty(this.$webStorage.getItem("userInfo"))) {
        this.$store.commit(
          "setUserInfo",
          JSON.parse(this.$webStorage.getItem("userInfo")),
        );
      }
    },
  },
};
</script>

<style lang="less">
/*@import '~vux/src/styles/reset.less';*/

// body {
//   background-color: #fbf9fe;
// }

@group-title-margin-top: 0px;
</style>

<style lang="scss">
@import "sass/reset.scss";
@import "sass/common.scss";
@import "sass/main.scss";
</style>

<style lang="css">
@import "../node_modules/swiper/dist/css/swiper.min.css";
</style>
