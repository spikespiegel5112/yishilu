<template>
  <div class="entrance_main_container">
    <!--<div class="common_bg_item" :style="bgStyle"></div>-->

    <!--    <transition name="fade" :appear="true">
          <div v-show="loadingDataFlag" class="entrance_loading_wrapper" @click="openEntrance">
            <div class="camera_wrapper" :class="{'active':currentLoadingPercentage!==100}">
              <div class="camera">
                <div class="icon">
                  <img alt="" src="../image/entrance/loading.gif"/>
                </div>
                <span v-if="currentLoadingPercentage!==100" class="percentage">{{currentLoadingPercentage}}%</span>
                &lt;!&ndash;{{currentLoadingPercentage}}&ndash;&gt;
                <span v-else class="percentage">点击</span>
              </div>
            </div>
          </div>
        </transition>-->

    <!--    <CommonAudioPlayer :play.sync="playFlag" :url="$store.state.speechUrl" name="speech"-->
    <!--                       @ready="onSpeechReady"></CommonAudioPlayer>-->
    <transition name="fade" :appear="true" :leave="true">
      <div class="entrance_animation_container" :style="animationStyle">
        <!--        {{checkEnvironment}}-->
        <!--        {{speechReadyFlag}}-->
        <!--        {{WeixinJSBridge}}-->
        <!--<div class="animation" >-->
        <canvas id="prologue" :width="canvasWidth" :height="canvasHeight">
          <!--<canvas id="prologue">-->
          您的浏览器不支持canvas
        </canvas>
      </div>
    </transition>

    <div v-if="false" class="entrance_speech_wrapper">
      <ul class="speech">
        <li v-for="(item, index) in speechList" :class="{active:item.active,}">
          <img alt="" :src="item.image"/>
        </li>
      </ul>
    </div>
    <transition name="fade" :appear="true" :leave="true">
      <div v-show="!loadingDataFlag&&!beginFlag" class="title">
        <img alt="" src="../image/entrance/yinianzhijizaiyuchun_00000.png"/>
      </div>
      <!--<div v-show="loadingDataFlag" class="title">-->
      <!--<img src="../image/entrance/yinianzhijizaiyuchun_00000.png"/>-->
      <!--</div>-->
    </transition>

    <div class="entrance_button_wrapper">
      <transition name="fade" :appear="true" :leave="true">
        <a v-show="!loadingDataFlag&&!beginFlag" :class="{active:beginFlag}" @click="playAnimation"
           class="entrance_beginbutton"></a>
      </transition>
    </div>
    <transition name="fade" :appear="true" :leave="true">
      <div v-show="!loadingDataFlag&&!beginFlag" class="common_logo_wrapper">
        <span class="common_logo_item"></span>
      </div>

      <!--<div v-show="loadingDataFlag" class="common_logo_wrapper">-->
      <!--<span class="common_logo_item"></span>-->
      <!--</div>-->
    </transition>

  </div>
</template>

<script>
  // import { getUserInfo } from '../../api/auth.js'
  // import ion from '../js/ion-sound.js'
  // var ion =require("exports?window.ion!../js/ion-sound.js")
  // const ion =require('exports-loader?window.ion!../js/ion-sound.js');

  import FrameAnimation from '../js/FrameAnimation'
  // import CommonAudioPlayer from "./common/CommonAudioPlayer";

  export default {
    name: "Entrance",
    components: {
      // CommonAudioPlayer
    },
    data() {
      return {
        speechReadyFlag: false,
        animationLoadingFlag: true,
        rulesFlag: false,
        playMusicFlag: false,
        frameAnimationInstance: {},
        remUnit: 0,
        canvasWidth: '0',
        canvasHeight: '0',
        invisible: true,
        currentLoadingPercentage: 0,
        animateFinishedFlag: false,
        userInfo: {},
        // playFlag: true,
        playFlag: false,
        muteFlag2: true,
        totalPeopleNumber: '',
        iosReadyToPlayFlag: false,
        beginFlag: false,
        playShow: false, // 播放按钮
        loadingDataFlag: false,
        audioInstance1: {},
        audioInstance2: {},
        speechList: [{
          image: require('../image/entrance/speech_1.png'),
          active: false,
          delay: 1000
        }, {
          image: require('../image/entrance/speech_2.png'),
          active: false,
          delay: 2000
        }, {
          image: require('../image/entrance/speech_3.png'),
          active: false,
          delay: 1000
        }, {
          image: require('../image/entrance/speech_4.png'),
          active: false,
          delay: 2000
        }, {
          image: require('../image/entrance/speech_5.png'),
          active: false,
          delay: 1000
        }],
        ready: false
      }
    },
    computed: {
      allLoaded() {
        return !this.speechReadyFlag && !this.animationLoadingFlag;
      },
      frameLoaded() {
        return !this.animationLoadingFlag;
      },
      bgStyle() {
        return {
          backgroundImage: 'url(' + require('../image/entrance/entrance_bg_00000.jpg') + ')'
        }
      },
      animationStyle() {
        return {
          zIndex: this.beginFlag ? 0 : '-2'
        }
      },
      checkEnvironment() {
        return this.$checkEnvironment();
      }
    },
    watch: {
      allLoaded(value) {
        if (value === true) {
          // if (this.$checkDevice() === 'ios') {
          //   this.iosReadyToPlayFlag = true;
          // } else {
          //   this.frameAnimationInstance.play();
          //   this.playFlag = false;
          // }


        }
      },
      remUnit(value) {
        this.$nextTick(() => {
          let windowWidth = document.body.clientWidth.toString().replace('px', '');
          this.canvasWidth = document.body.clientWidth + 'px';
          this.canvasHeight = document.body.clientHeight + 'px';
        })
      },
    },

    mounted() {
      setTimeout(() => {
        this.ready = true;
      }, 2000);

      // this.$vux.loading.show({
      //   text: '音乐加载中'
      // });
      // var preHandler=function(e){e.preventDefault();},// 注意此处代码片段必须这样提出来已保证传入下边两个事件的处理程序一样才生效，分别写到事件处理程序中不生效。
      // document.addEventListener('touchmove', me.preHandler,false);//阻止默认滑动事件
      // document.addEventListener('touchmove', function(e){e.preventDefault();}, false);
      this.sequenceFramesWidth = 750;
      this.sequenceFramesHeight = 1624;
      this.$autoHeight({
        target: '.entrance_main_container',
        force: true
      });
      this.$nextTick(() => {
        this.remUnit = Number(document.getElementsByTagName('html')[0].style.fontSize.replace('px', ''))


      });


      this.init();

    },
    methods: {
      init() {
        // console.log(FrameAnimation)
        let prologueFramesUrl = () => {
          let result = [];
          let prologueFrameBaseUrl = index => {
            index = ('000' + index).slice(-3);
            return 'http://pp-jgxzq.oss-cn-qingdao.aliyuncs.com/ctzcf_entrance/entrance_00' + index + '.jpg'
          };
          for (let i = 0; i <= 209; i++) {
            result.push(prologueFrameBaseUrl(i))
          }
          // console.log('prologueFramesUrl', result)

          return result;
        };
        // console.log(prologueFramesUrl())


        this.frameAnimationInstance = new FrameAnimation({
          canvasTargetId: 'prologue', // 目标画布对象ID（必须）
          framesUrl: prologueFramesUrl(), // 每一帧的url（必须）
          audioObject: null, // 音频的对象（优先级高于ION）
          audioIonName: '1', // ION音频的名字（优先级高于路径）
          // audioUrl: "https://pp-jgxzq.oss-cn-qingdao.aliyuncs.com/music/VCG23115669%7E1.mp3", // 音频路径（优先级最低）
          slicing: 'widthBased',
          align: 'bottom',
          audioUrl: "", // 音频路径（优先级最低）
          height: this.sequenceFramesHeight, // 图片的高度（必须）
          width: this.sequenceFramesWidth, // 图片的宽度（必须）
          onStart: null, // 加载开始回调函数，传入参数total
          onComplete: this.playFinished, // 播放完毕回调
          onLoading: this.handleLoading, // 播放完毕回调
          onLoad: this.handleLoaded, // 播放完毕回调
          onLoadingFinished: this.handleLoaded,
          loop: false, // 是否循环
          frequency: 10, // 每秒帧数
        });
        console.log(this.frameAnimationInstance);
        this.frameAnimationInstance.initialize(() => {


        })
      },
      speechControl() {
        let that = this;

        let index1 = 0;
        let trigger = delay => {
          setTimeout(() => {
            if (index1 === 4) {
              for (let i = 0; i < 4; i++) {
                that.speechList[i].active = false;
              }
              that.speechList[4].active = true;
              // debugger
            } else {
              that.speechList[index1].active = true;

            }
            index1++;
            if (index1 < that.speechList.length) {
              trigger(that.speechList[index1].delay)
            }
          }, delay)
        };
        trigger();

      },
      playFinished() {
        // alert('finished')
        this.animateFinishedFlag = true;
        // this.playMusic();\
        // this.speechControl();

        setTimeout(() => {
          this.$router.push({
            name: 'edit'
          })
        }, 2000)

      },
      handleLoading(index) {
        // debugger
        // console.log(this.frameAnimationInstance.frameLength)
        // console.log(index)

        // this.currentLoadingPercentage = Math.ceil(100 * index / this.frameAnimationInstance.frameLength)
        this.$store.commit('updateLoadingPercentage', Math.ceil(100 * index / this.frameAnimationInstance.frameLength))
      },
      handleLoaded() {
        // debugger
        let that = this;
        this.animationLoadingFlag = false;
        // if (this.$checkEnvironment() === 'wechat') {
        //   this.playAnimation();
        // } else {
        //   this.playShow = true;
        // }

      },
      playAnimation() {
        if (!this.beginFlag) {
          console.log('start');
          this.playFlag = true;
          this.beginFlag = true;
          this.$nextTick(() => {
            this.muteFlag2 = false;

          });
          this.animateFinishedFlag = false;
          // this.animationLoadingFlag = false;
          setTimeout(() => {
            this.frameAnimationInstance.play();
            this.iosReadyToPlayFlag = false;
          }, 1000);

          let baobaoAudioPlayback = new this.$baobaoAudioPlayback({
            src: this.$store.state.speechUrl,
            loop: false,
            icon: false,
            el: '.entrance_main_container',
          });
          baobaoAudioPlayback.play();

            // this.audioInstance2 = new Audio();
            // this.audioInstance2.src = this.$store.state.speechUrl;
            // // this.audioInstance1.loop = true;
            // this.audioInstance2.play();

        }


      },
      playMusic() {
        // debugger
        // this.playMusicFlag = true;
        this.playFlag = true;

      },
      checkPersonalCenter() {
        this.$router.push({
          name: 'personalCenter'
        })
      },
      openEntrance() {
        if (this.frameLoaded) {
          this.loadingDataFlag = false;
          // this.playMusic();
          // debugger

          this.$store.commit('openEntrance')
        }


      },
      onSpeechReady(state) {
        // debugger
        this.speechReadyFlag = true;
        this.$vux.loading.hide();
      },
      initShare() {
        let params = {
          share: true, // true可以分享；false不可以分享
          data: {
            title: `春天再出发！立个flag，做好自己的事`, // 分享标题
            desc: '一步一个脚印，一棒接着一棒往前走', // 分享描述
            link: `${this.$store.state.shareUrl}#/entrance`, // 分享链接
            imgUrl: 'http://pp-jgxzq.oss-cn-qingdao.aliyuncs.com/ctzcf/ctzcf_shortcut.jpg' // 分享图标
          }
        };
        this.$wxsdk.initConfig(params)
      },
    }
  }
</script>

<style scoped>

</style>
