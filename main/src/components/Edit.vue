<template>
  <div class="common_main_container">
    <div class="edit_container">
      <div class="common_bg_item" :style="bgStyle"></div>

      <CommonRope/>
      <div class="edit_wrapper">
        <div class="main">
          <CommonEditBg/>
          <div class="edit_title_wrapper">
            <img class="title" src="../image/edit/flag_title_00000.png"/>
            <a @click="checkSelected">
              <img class="selected" src="../image/edit/selected_00000.png"/>
              <span class="count">{{count}}</span>
            </a>

          </div>
          <div class='edit_flaglist_wrapper'>
            <ul>
              <li v-for="(item, index) in $store.state.flagList.filter((item,index)=>index<pagination+6).slice(-6)"
                  :class="{'chosen':item.chosen}"
                  @click="chooseFlag(item, pagination+index)">
                <a>
                  <!--<label>{{pagination+index+1}}.</label>-->
                  <span class="content">{{item.content}}</span>
                  <span class="flag"></span>
                </a>
              </li>
            </ul>
            <div class="change">
              <a @click="change" class="common_button_item">换一换</a>

            </div>
          </div>
        </div>

        <div class="common_footer_wrapper">
          <router-link class="empty" :to="{name:'entrance'}">返回</router-link>
          <a class="solid" @click="nextStep">下一步</a>
          <!--<router-link class="solid" :to="{name:'myFlag'}">下一步</router-link>-->
        </div>


      </div>
    </div>
    <div v-show="checkSelectedFlag" class="edit_selectedflaglist_container">
      <div class='edit_flaglist_wrapper edit_selectedflaglist_wrapper'>
        <div class="main">
          <ul>
            <li v-for="(item, index) in $store.state.flagList.filter((item,index)=>item.chosen)"
                :class="{'chosen':item.chosen}"
                @click="deselectFlag(index)">
              <a>
                <label>{{index+1}}.</label>
                <span class="content">{{item.content}}</span>
                <span class="delete"></span>
              </a>
            </li>
          </ul>
        </div>

        <div class="close">
          <a @click="closeChosenDialog" class="common_button_item">关闭</a>

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
    name: "Edit",
    components: {
      CommonMusic
    },
    data() {
      return {
        pagination: 0,
        checkSelectedFlag: false
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
      bgStyle() {
        return {
          backgroundImage: 'url(' + this.$store.state.backgroundList.filter(item => item.active === true)[0].src + ')'
        }
      },
      currentFlagList() {

        if (index < (index + pagination)) {

        }
      },
      remUnit() {
        return Number(document.getElementsByTagName('html')[0].style.fontSize.replace('px', ''))
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
      },
      checkSelectedFlag(value) {
        let html = document.querySelectorAll('html')[0];
        value === true ? html.style.overflow = 'hidden' : html.style.overflow = 'visible';
      }
    },

    mounted() {
      console.log('availHeight', screen.availHeight)
      console.log('sss', document.querySelector('.edit_wrapper').clientHeight);

      // 自适应高度
      // if (screen.availHeight < document.querySelector('.edit_wrapper').clientHeight + 200) {
      //   this.$autoHeight({
      //     target: '.edit_flaglist_wrapper ul',
      //     // offset: -342,
      //     offset: -(this.remUnit * 14.1),
      //     force: true
      //   });
      // }

      // 不限制高度
      if (screen.availHeight > document.querySelector('.edit_wrapper').clientHeight) {
        this.$autoHeight({
          target: '.edit_container',
          offset: 0,
          // offset: -(this.remUnit * 14.1),
          force: true
        });
      }


      console.log()
    },
    methods: {
      chooseFlag(data, index) {
        let limit = 4;
        this.$store.commit('chooseFlagList', index);

        if (this.$store.state.flagList.filter(item => item.chosen).length > limit) {
          // this.$vux.toast.text(`FLAG数不能大于${limit}`, 'top', 1000)
          this.$vux.toast.show({
            text: `最多选择${limit}个`,
            position: 'center',
            time: 1000,
            type: 'cancel'
          });
          this.$store.commit('chooseFlagList', index);
        } else {
          this.$clickSound()

        }

      },
      nextStep() {
        if (this.$store.state.flagList.filter(item => item.chosen).length === 0) {
          this.$vux.toast.show({
            text: `请至少选择1个`,
            position: 'center',
            time: 1000,
            type: 'cancel'
          });
        } else {
          this.$router.push({
            name: 'myFlag'
          })
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
      },
      change() {
        let flagListLength = this.$store.state.flagList.length;
        if (this.pagination + 6 < flagListLength) {
          if (flagListLength - this.pagination >= 6) {
            this.pagination = this.pagination + 6;
          } else {
            this.pagination = this.pagination + (flagListLength - this.pagination)
          }
        } else {
          this.pagination = 0;
        }
      },
      checkSelected() {
        this.checkSelectedFlag = true;
      },
      deselectFlag(index) {
        // debugger
        let count = -1;
        this.$store.state.flagList.forEach((item2, index2) => {
          if (item2.chosen === true) {
            count++;
            if (count === index) {
              this.$store.state.flagList[index2].chosen = false;
            }
          }
        })
        this.$clickSound()
      },
      closeChosenDialog() {
        this.checkSelectedFlag = false;
      }

    }
  }
</script>

<style scoped>

</style>
