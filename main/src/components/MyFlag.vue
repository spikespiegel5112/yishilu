<template>
  <!--<div class="common_main_container" :style="{backgroundImage:$store.state.backgroundList.filter(item => item.active===true)[0].src}">-->
  <div class="common_main_container">


    <div class="myflag_container">
      <div class="common_bg_item" :style="bgStyle"></div>

      <div class="myflag_wrapper">

        <div class="main">
          <CommonRope/>

          <div class="myflag_title_wrapper">
            <img class="selected" src="../image/myflag/myflag_title_00000.png"/>
          </div>
          <div class="chain">
            <img src="../image/myflag/chain_00000.png"/>
          </div>
          <div class='myflag_flaglist_wrapper'>
            <ul>
              <li v-for="(item, index) in $store.state.flagList.filter(item=>item.chosen===true)">
                <a>
                  <label>{{index+1}}.</label>
                  <span class="content">{{item.content}}</span>
                  <span class="flag"></span>
                </a>
              </li>
            </ul>
            <div class="footer"></div>
          </div>
          <div class="myflag_note_wrapper">
            <ul>
              <li class="note" v-for="(item, index) in previewList">
                <CommonNode/>
                <a class="preview" @click="chooseBg(index)">
                  <img :src="item.src"/>
                </a>
              </li>
            </ul>
          </div>
          <div class="common_footer_wrapper">
            <router-link class="empty" :to="{name:'edit'}">返回</router-link>
            <router-link class="solid" :to="{name:'poster'}">下一步</router-link>
          </div>
        </div>

      </div>


    </div>
  </div>
</template>

<script>
  // import { getUserInfo } from '../../api/auth.js'
  // import ion from '../js/ion-sound.js'
  // var ion =require("exports?window.ion!../js/ion-sound.js")
  // const ion =require('exports-loader?window.ion!../js/ion-sound.js');

  import FrameAnimation from '../js/FrameAnimation'
  import CommonMusic from "./common/CommonAudioPlayer";

  export default {
    name: "MyFlag",
    components: {
      CommonMusic
    },
    data() {
      return {
        previewList: [{
          src: require('../image/myflag/note1_00000.png')
        }, {
          src: require('../image/myflag/note2_00000.png')
        }, {
          src: require('../image/myflag/note3_00000.png')
        }]
      }
    },
    computed: {
      allLoaded() {
        return !this.musicLoadingFlag && !this.animationLoadingFlag;
        // return !this.animationLoadingFlag;
      },
      flagList() {
        return this.$store.state.flagList
      },
      count() {
        return this.$store.state.flagList.filter(item => item.chosen).length;
      },
      // activeBg(){
      //   return
      // },
      bgStyle() {
        return {
          backgroundImage: 'url(' + this.$store.state.backgroundList.filter(item => item.active === true)[0].src + ')'
        }
      }
    },
    watch: {
      musicLoadingFlag(value) {
        // debugger
        if (!value) {
          this.$vux.loading.hide();
        }
      },
      allLoaded(value) {
        if (value === true) {
          // if (this.$checkDevice() === 'ios') {
          //   this.iosReadyToPlayFlag = true;
          // } else {
          //   this.frameAnimationInstance.play();
          //   this.muteFlag = false;
          // }
          this.frameAnimationInstance.play();
          this.muteFlag = false;

        }
      }
    },

    mounted() {

    },
    methods: {
      chooseFlag(data, index) {
        this.$store.commit('chooseFlagList', index)
      },
      chooseBg(index) {
        let imageObj = new Image();
        console.log( this.$store.state.backgroundList[index]);
        imageObj.src = this.$store.state.backgroundList[index].src;
        this.$vux.loading.show({
          text: 'Loading',
        });
        imageObj.onload = () => {
          this.$store.commit('chooseBg', index)
          this.$vux.loading.hide();

        }
      },
      initShare() {
        let params = {
          share: true, // true可以分享；false不可以分享
          data: {
            title: `春天再出发！立个flag，做好自己的事`, // 分享标题
            desc: '一步一个脚印，一棒接着一棒往前走', // 分享描述
            link: `${this.$store.state.shareUrl}#/bg_1_00000`, // 分享链接
            imgUrl: 'http://pp-jgxzq.oss-cn-qingdao.aliyuncs.com/ctzcf/ctzcf_shortcut.jpg' // 分享图标
          }
        };
        this.$wxsdk.initConfig(params)
      }

    }
  }
</script>

<style scoped>

</style>
