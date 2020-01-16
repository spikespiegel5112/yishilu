<template>
  <div class="common_main_container">
    <transition name="fade" :appear="false" :leave="true">

      <!--      <div v-show="!openFlag&&$route.path==='/entrance'" class="entrance_loading_container">-->
      <div v-show="!openFlag" class="entrance_loading_container">
        <div class="entrance_loading_wrapper" @click="open">
          <div class="camera_wrapper" :class="{'active':currentLoadingPercentage!==100}">
            <div class="camera">
              <div class="icon">
                <img alt="" src="../image/entrance/loading.gif"/>
              </div>
              <span v-if="currentLoadingPercentage!==100" class="percentage">{{currentLoadingPercentage}}%</span>
              <!--{{currentLoadingPercentage}}-->
              <span v-else class="percentage">点击</span>
            </div>
          </div>
        </div>
      </div>

    </transition>
    <transition>
      <router-view></router-view>
    </transition>
  </div>
</template>

<script>
  // import baobaoAudioPlay from '../js/baobaoAudioPlay'
  export default {
    name: "Layout",
    components: {},
    data() {
      return {
        playFlag: false,
        bgmFlag: false,
        bgmReadyFlag: false,
        openFlag: false,
        name: 'entrance'
      }
    },
    computed: {
      openEntrance() {
        return this.$store.state.openEntrance;
      },
      readyFlag() {
        return this.$store.state.openEntrance && this.bgmReadyFlag;
      },
      stateCode() {
        return this.$route.query.state;
      },
      displayBgmIcon() {
        return this.$route.name !== 'poster';
      },
      currentLoadingPercentage() {
        // debugger
        return this.$store.state.currentLoadingPercentage
      }
    },
    watch: {
      openEntrance(value) {
        // debugger
        if (value) {

        }

        if (this.bgmReadyFlag === true) {
          this.playFlag = true;
        }

      },
    },
    created() {

    },
    mounted() {
      this.$remResizing({
        fontSize: 20,
        threshold: 640,
        aligncenter: true,
      });
      this.$autoHeight({
        target: '.entrance_loading_container',
        force: true
      });
      console.log('$webStorage:', this.$webStorage.type);
      this.sharePage();

      this.getUserInfoCache();

    },
    methods: {
      open() {
        if (this.currentLoadingPercentage === 100) {
          this.openFlag = true;
          let baobaoAudioPlayback = new this.$baobaoAudioPlayback({
            src: this.$store.state.bgmUrl,
            loop: true,
            el: '.entrance_main_container',
          });
          baobaoAudioPlayback.play()
          //   .then(result=>{
          //
          //   debugger
          // }).catch(error=>{
          //   debugger
          // })

          // let audioInstance2 = new Audio();
          // audioInstance2.src = this.$store.state.bgmUrl;
          // audioInstance2.loop = true;
          // audioInstance2.play();
        }
      },
      onMusicReady() {
        // debugger
        // alert('bgmReady')
        this.bgmReadyFlag = true;
      },
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
        this.$wxsdk.initConfig(params)
      },
      getUserInfoCache() {
        if (!this.$isEmpty(this.$webStorage.getItem('userInfo'))) {
          this.$store.commit('setUserInfo', JSON.parse(this.$webStorage.getItem('userInfo')))
        }
      },
    }
  }
</script>

<style scoped>

</style>
