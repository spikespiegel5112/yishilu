<template>
  <div class="common_main_container">
    <transition name="fade">
      <router-view></router-view>
    </transition>
  </div>
</template>

<script>
export default {
  name: "Layout",
  components: {},
  data() {
    return {
    };
  },
  computed: {
    
  },
  watch: {
  },
  created() {

  },
  mounted() {
    this.$remResizing({
      fontSize: 20,
      threshold: 750,
      aligncenter: true,
    });
    console.log('$webStorage:', this.$webStorage.type);
    this.sharePage();

    this.getUserInfoCache();

  },
  methods: {
    // 设置sharePage分享信息
    sharePage() {
      let params = {
        share: true, // true可以分享；false不可以分享
        data: {
          title: `春天再出发！立个flag，做好自己的事`, // 分享标题
          desc: '一步一个脚印，一棒接着一棒往前走', // 分享描述
          link: this.$store.state.shareUrl, // 分享链接
          imgUrl: 'http://pp-jgxzq.oss-cn-qingdao.aliyuncs.com/ctzcf/ctzcf_shortcut.jpg' // 分享图标
        }
      };
      this.$wxsdk.initConfig(params);
    },
    getUserInfoCache() {
      if (!this.$isEmpty(this.$webStorage.getItem('userInfo'))) {
        this.$store.commit('setUserInfo', JSON.parse(this.$webStorage.getItem('userInfo')));
      }
    },
  }
}
</script>

<style scoped>
</style>
