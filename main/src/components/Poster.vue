<template>
  <div class="common_main_container">

    <div v-show="originalPageFlag" class="poster_myflag_container">
      <div class="poster_canvas_wrapper">
        <div v-show="false" id="qrcode" class="qrcode"></div>
      </div>


    </div>
    <div v-if="generateImageFlag1" class="poster_generated_wrapper"></div>
  </div>
</template>

<script>
  import QRCode from 'qrcodejs2'
  import html2canvas from 'html2canvas'

  export default {
    name: "Poster",
    components: {},
    data() {
      return {
        originalPageFlag: true,
        generateImageFlag1: false,
        generateImageFlag2: false,
        posterHeightStyle: {},
        qrCode: {}

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
      rem() {
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

      // this.$vux.loading.show({
      //   text: '图片生成中',
      // });
      this.$nextTick(() => {
        this.generateSnapshot1();

      });
      // this.getPosterHeight();
    },
    methods: {
      getDate() {
        let result;
        let dateInstance = new Date();
        return `${dateInstance.getFullYear()}/${dateInstance.getMonth() + 1}/${dateInstance.getDate()}   ${dateInstance.getHours()}:${('0' + dateInstance.getMinutes()).slice(-2)}`;
      },

      async generateSnapshot1() {
        // this.$store.commit('chooseFlagList', 3)
        // this.$store.commit('chooseFlagList', 5)
        // this.$store.commit('chooseFlagList', 7)
        // this.$store.commit('chooseFlagList', 10)

        // this.$store.commit('chooseFlagList', 1)
        // this.$store.commit('chooseFlagList', 2)
        // this.$store.commit('chooseFlagList', 4)
        // this.$store.commit('chooseFlagList', 6)
        // this.$store.commit('chooseFlagList', 8)
        // this.$store.commit('chooseFlagList', 9)
        // this.$store.commit('chooseFlagList', 11)
        // this.$store.commit('chooseFlagList', 12)
        // this.$store.commit('chooseFlagList', 13)

        const canvas = document.createElement('canvas');
        let rem = this.rem;
        let ctxBg = canvas.getContext('2d');
        let ctxLine = canvas.getContext('2d');
        let ctxText = canvas.getContext('2d');
        let ctxSignature = canvas.getContext('2d');
        let ctxRectangle = canvas.getContext('2d');
        let ctxFooter = canvas.getContext('2d');


        const getImage = (src) => {
          return new Promise((resolve, reject) => {
            let imageObj = new Image();
            imageObj.src = src;
            imageObj.onload = data => {
              resolve(imageObj);
              return imageObj;
            }
          })
        };
        let unitBlockHeight = 2.5 * rem;
        let lineHeight = 0.4 * unitBlockHeight;
        let padding = 0.2 * unitBlockHeight;

        let bgWidth = 16 * rem;
        console.log(this.$store.state.backgroundList)
        let bgHeight = 22 * rem + this.$store.state.flagList.filter(item => item.chosen === true).length * 2.7 * rem;
        canvas.width = bgWidth;
        canvas.height = bgHeight;

        let bg = await getImage(this.$store.state.backgroundList.filter(item => item.active === true)[0].src);
        ctxBg.drawImage(bg, 0, 0, bgWidth, bgHeight);

        let header = await getImage(require('../image/poster/poster_header_00000.png'));
        ctxLine.drawImage(header, 1 * rem, 0, 14 * rem, 10 * rem);


        let textYAxisOffset;
        let rectangleYAxisOffset;
        let strokeYAxisOffset;
        let signatureOffset;
        let rectangleYAxisBeginningPoint;
        textYAxisOffset = rectangleYAxisOffset = strokeYAxisOffset = rectangleYAxisBeginningPoint = 9.1 * rem;

        this.$store.state.flagList.filter(item => item.chosen).forEach((item, index) => {
          textYAxisOffset += 1.3 * rem;


          // debugger
          ctxText.fillStyle = '#000';
          ctxText.textAlign = "start";
          ctxText.font = 0.8 * rem + 'px SimSun';
          let textContent = item.content;
          let currentLineLength = 0;
          let contentLineCount = 1;
          let textLineArr = [];

          let writeEachLine = data => {
            let result = [];
            let lineLength = 0;
            let lineLengthThreshold = 27;
            let lineContent = '';
            let character = data.split('');
            let lineIndex = 0;
            character.forEach((item2, index2) => {
              lineLength += item2.match(/[\uff00-\uffff\u4e00-\u9fa5]/g) ? 2 : 1;
              if (lineLength < lineLengthThreshold) {
                lineContent += item2;
                // console.log(result.join('').concat([lineContent]))
                if (result.join('').concat([lineContent]).split('').length === character.length) {
                  result.push(lineContent)
                }
              } else if (lineLength === lineLengthThreshold) {
                lineContent += item2;

                result.push(lineContent)

                lineLength = 0;
                lineContent = '';
              } else {
                lineIndex++;
                lineContent += item2;

                result.push(lineContent)

                lineLength = 0;
                lineContent = '';
                // lineContent += item2;
              }
            });
            return result;
          };

          // ctxLine.fillStyle = '#fff';
          // ctxLine.fillRect(1 * rem, contentHeight, 14 * rem, 3 * rem);
          if (ctxText.measureText(textContent).width > 11.5 * rem) {
            contentLineCount = Math.ceil(ctxText.measureText(textContent).width / (11.5 * rem));

            console.log('measureText', ctxText.measureText(textContent).width)
            textLineArr = writeEachLine(textContent)

            console.log(textLineArr)
            // debugger

            console.log('textLineArr', textLineArr)
            console.log('currentLineLength', currentLineLength)
            console.log('contentLineCount', contentLineCount)

          } else {
            textLineArr = writeEachLine(textContent)

          }


          rectangleYAxisOffset = textYAxisOffset - 0.4 * rem;

          ctxRectangle.fillStyle = '#fff';
          ctxRectangle.fillRect(1 * rem, rectangleYAxisOffset, 14 * rem, 1 * rem + lineHeight * textLineArr.length);

          strokeYAxisOffset = rectangleYAxisOffset;
          ctxLine.beginPath();
          ctxLine.strokeStyle = '#f2cb84';
          ctxLine.lineWidth = '1';

          strokeYAxisOffset += padding;
          ctxLine.moveTo(1.1 * rem, strokeYAxisOffset);

          textLineArr.forEach((item3, index3) => {
            strokeYAxisOffset += lineHeight;
            ctxLine.moveTo(1.1 * rem, strokeYAxisOffset);
          });
          strokeYAxisOffset += padding;
          ctxLine.moveTo(1.1 * rem, strokeYAxisOffset);
          ctxLine.lineTo(14.5 * rem, strokeYAxisOffset);
          ctxLine.stroke();


          textLineArr.forEach((item3, index3) => {
            ctxText.fillStyle = '#000';

            console.log('textYAxisOffset', textYAxisOffset)
            if (index3 === 0) {
              ctxText.fillText(`${index + 1}.`, 2 * rem, textYAxisOffset + lineHeight);
            }
            ctxText.fillText(`${item3}`, 3 * rem, textYAxisOffset + lineHeight);
            textYAxisOffset += lineHeight;

          });
          textYAxisOffset -= 0.3 * rem

        });

        signatureOffset = strokeYAxisOffset;
        ctxLine.save();
        ctxLine.fillStyle = '#fff';
        ctxLine.fillRect(1 * rem, signatureOffset, 14 * rem, 2 * lineHeight + 3 * padding);
        ctxSignature.fillStyle = '#000';
        // debugger
        signatureOffset += 3 * padding;
        ctxSignature.textAlign = "right";
        ctxSignature.font = 0.8 * rem + 'px SimSun';
        let signature = this.$store.state.userInfo.fullName;
        let dateInstance = new Date();
        let date = `${dateInstance.getFullYear()}/${dateInstance.getMonth() + 1}/${dateInstance.getDate()}   ${dateInstance.getHours()}:${('0' + dateInstance.getMinutes()).slice(-2)}`;
        ctxSignature.fillText(signature, 14 * rem, signatureOffset);
        ctxSignature.fillText(date, 14 * rem, signatureOffset + lineHeight);
        signatureOffset += lineHeight * 2;

        let contentHeight = signatureOffset;
        let footer = await getImage(require('../image/poster/poster_footer_00000.png'));
        ctxFooter.drawImage(footer, 1 * rem, contentHeight, 14 * rem, 7 * rem);

        let footerLogo = await getImage(require('../image/poster/poster_footer_logo_00000.png'));
        ctxFooter.drawImage(footerLogo, 5.7 * rem, contentHeight + 2.2 * rem, 9 * rem, 4 * rem);

        ctxFooter.drawImage(this.qrCode._el.getElementsByTagName('canvas')[0], 1.5 * rem, contentHeight + 2.2 * rem, 4 * rem, 4 * rem);


        let posterImgElement = document.createElement('img');
        posterImgElement.src = canvas.toDataURL();

        document.querySelector('.poster_canvas_wrapper').appendChild(posterImgElement)

      },
      generateQRCode() {
        this.qrCode = new QRCode(document.getElementById('qrcode'), {
          text: `${this.$store.state.shareUrl}#/entrance`,
          width: 125,
          height: 125,
          colorDark: "#000000",
          colorLight: "#ffffff",
          correctLevel: QRCode.CorrectLevel.H
        });
      },
      getPosterHeight() {
        let el = document.querySelector('.poster_myflag_container')
        let result = el.clientHeight - this.rem * 10;

        console.log('dsds', result);
        // document.querySelector('.poster_myflag_wrapper').style.height = result + 'px';
        // debugger
        // this.posterHeightStyle = {
        //   height: result - this.rem * 10 + 'px'
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
