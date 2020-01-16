<template>
  <div class="common_main_container">

    <div v-show="originalPageFlag" class="poster_myflag_container">
      <div class="common_bg_item" :style="bgStyle"></div>

      <div class="poster_myflag_wrapper">
        <div class="main">
          <div class="background">
            <div></div>
          </div>
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
            <div class="footer">
              <div class="signature">
                <p>{{$store.state.userInfo.nickName}}</p>
                <p>立于{{getDate()}}</p>
              </div>
            </div>
          </div>
          <div class="chain">
            <img src="../image/myflag/chain_00000.png"/>
          </div>
          <div class="poster_footer_wrapper">
            <div id="qrcode" class="qrcode"></div>
            <div class="hint">
              <div class="flag">
                <label></label>
                <span>春天再出发</span>
              </div>
              <p>扫码立FLAG</p>

            </div>
            <div class="logo">
              <img src="../image/logo_00000.png"/>
            </div>
          </div>
        </div>
      </div>

    </div>
    <div v-if="generateImageFlag1" class="poster_generated_wrapper"></div>
  </div>
</template>

<script>
  import QRCode from 'qrcodejs2'
  import html2canvas from 'html2canvas'
  import Canvas2Image from '../js/Canvas2Image'

  export default {
    name: "Poster",
    components: {},
    data() {
      return {
        originalPageFlag: true,
        generateImageFlag1: false,
        generateImageFlag2: false,
        posterHeightStyle: {}
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
      remUnit() {
        return Number(document.getElementsByTagName('html')[0].style.fontSize.replace('px', ''))
      },

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
      console.log(this.$store.state.userInfo)
      this.generateQRCode();

      console.log(document.querySelector('.poster_myflag_container').scrollHeight)
      console.log(document.body.scrollHeight)
      window.scrollTo(0, 0);
      if (document.querySelector('.poster_myflag_container').scrollHeight < document.body.scrollHeight) {
        this.$autoHeight({
          target: '.poster_myflag_container',
          force: true
        });
      } else {
        this.$autoHeight({
          target: '.poster_myflag_wrapper',
          force: true
        });
      }

      // this.$vux.loading.show({
      //   text: '图片生成中',
      // });
      setTimeout(() => {
        this.$vux.loading.hide();
        this.$nextTick(() => {
          this.generateSnapshot1();

        });
      }, 1000);
      // this.getPosterHeight();
    },
    methods: {
      getDate() {
        let result;
        let dateInstance = new Date();
        return `${dateInstance.getFullYear()}/${dateInstance.getMonth() + 1}/${dateInstance.getDate()}   ${dateInstance.getHours()}:${('0' + dateInstance.getMinutes()).slice(-2)}`;
      },
      generateSnapshot1() {
        this.originalPageFlag = true;
        let that = this;
        // this.$vux.loading.show({
        //   text: '图片生成中'
        // });

        console.log(document.querySelector('.poster_myflag_container').clientHeight)
        html2canvas(document.body, {
          useCORS: true,
          logging: false,
          // width: document.querySelector('.poster_myflag_wrapper').clientWidth,
          // height: document.querySelector('.poster_myflag_container').clientHeigth
          // proxy: this.$baseUrl
        }).then(function (canvas) {
          console.log(canvas)
          // debugger
          console.log(document.body.scrollWidth)
          that.generateImageFlag1 = true;
          that.$nextTick(() => {
            let container = document.querySelector('.poster_generated_wrapper');
            console.log('container', container)
            // container.appendChild(canvas)

            console.log(Canvas2Image)

            let type = 'jpeg';

            let imgData = canvas.toDataURL(type);
            let imageResult = document.createElement('img');
            imageResult.src = imgData;
            container.appendChild(imageResult)
            that.originalPageFlag = false;

            that.$vux.toast.text('<p>图片生成成功</p><p>请长按保存转发</p>', 'top', 1000)

            // Canvas2Image.saveAsImage(canvas, document.body.scrollWidth, document.body.scrollHeight, 'jpeg')
            // Canvas2Image.saveAsPNG(canvas, document.body.scrollWidth, document.body.scrollHeight)
            // let img_data1 = Canvas2Image.saveAsJPEG(canvas, document.body.scrollWidth, document.body.scrollHeight);

            let _fixType = function (type) {
              type = type.toLowerCase().replace(/jpg/i, 'jpeg');
              let r = type.match(/png|jpeg|bmp|gif/)[0];
              return 'image/' + r;
            };
            // debugger


            // 加工image data，替换mime type
            imgData = imgData.replace(_fixType(type), 'image/octet-stream');
            console.log(canvas)

            // img_data1 = img_data1.split(';')[1];
            let filename = (new Date()).getTime() + '.jpg';

            // that.saveFile(imgData, filename);

            that.$vux.loading.hide();
          })
        });
      },
      generateQRCode() {
        let qrcode = new QRCode(document.getElementById('qrcode'), {
          text: `${this.$store.state.shareUrl}#/player?filmCode=${this.filmCode}&isPreview=false`,
          width: 125,
          height: 125,
          colorDark: "#000000",
          colorLight: "#ffffff",
          correctLevel: QRCode.CorrectLevel.H
        });
      },
      getPosterHeight() {
        let el = document.querySelector('.poster_myflag_container')
        let result = el.clientHeight - this.remUnit * 10;

        console.log('dsds', result);
        // document.querySelector('.poster_myflag_wrapper').style.height = result + 'px';
        // debugger
        // this.posterHeightStyle = {
        //   height: result - this.remUnit * 10 + 'px'
        // }
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
      }

    }
  }
</script>

<style scoped>

</style>
