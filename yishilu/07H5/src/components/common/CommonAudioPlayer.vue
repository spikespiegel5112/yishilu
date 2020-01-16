<template>
  <div class="common_musicbutton_wrapper" :style="invisibleStyle">
    <div class="test">
      <!--      {{this.audioInstance.src}}-->
    </div>
    <a :class="{active:iconRotateFlag}" @click="playEvent">
      <!--      <audio id="commonAudio"></audio>-->
      <!--{{invisible}}-->
      <!--{{iconRotateFlag}}-->

    </a>
    <p v-if="loading" class="loading">音乐载入中</p>
  </div>
</template>

<script>
  export default {
    name: "CommonMusic",
    props: {
      invisible: {
        type: Boolean,
        default: false,
        required: false
      },
      play: {
        type: Boolean,
        default: false,
        required: false
      },
      name: {
        type: String,
        default: '',
        required: true
      },
      trigger: {
        type: Boolean,
        default: false,
        required: false
      },
      url: {
        type: String,
        default: '',
        required: false
      },
      autoPlay: {
        type: String,
        default: 'autoplay',
        required: false
      },
      loop: {
        type: Boolean,
        default: false,
        required: false
      },
      icon: {
        type: Boolean,
        default: true,
        required: false
      }
    },
    data() {
      return {
        iconRotateFlag: false,
        audioInstance: {},
        audioInstanceArr: [],
        musicReady: false,
        loading: false,
        innerPlay: false,
        innerName: '',
        WeixinJSBridgeReadyFlag: false,
        sourceInstanceArr: [],
        audioSrcList: []
      }
    },
    computed: {
      invisibleStyle() {
        // alert(this.invisible)
        return {
          display: this.invisible ? 'none' : 'block'
        }
      }
    },
    watch: {
      play(value) {
        // debugger

        this.iconRotateFlag = value;
        this.innerPlay = value;

      },
      async innerPlay(value) {

        let chosenInstanceArr = this.audioInstanceArr.filter(item => item.name === this.name)
        if (chosenInstanceArr.length === 0) {
          console.warn(`所选音频${this.name}不存在`)
          return;
        }
        let instance = this.audioInstanceArr.filter(item => item.name === this.name)[0].instance;

        console.log('name', this.name)
        console.log('instance', instance.src)
        console.log('audioInstanceArr', this.audioInstanceArr)


        this.controlMusic(this.innerPlay, instance);
        this.$emit('update:play', this.innerPlay)
      },
      innerName(value) {
        console.log('innerName', this.audioInstanceArr.filter(item => item.name === value))
        // if (this.audioInstanceArr.filter(item => item.name === value).length === 0) {
        //   debugger
        //   this.initMusic();
        //
        // }
      },
      name(value) {
        // alert(value)
        // debugger

      },
      url(value) {
        // debugger
      }
    },
    beforeCreate() {
      // window.WeixinJSBridge=undefined
    },
    created() {
      // this.play=false;
      // window.AudioContext = window.AudioContext || window.webkitAudioContext;
      console.log('url', this.url)

      console.log(this.name)


      // alert('created')
      this.initMusic();

      this.$emit('loading', this.loading);

    },
    mounted() {
      // this.$vux.confirm.show({
      //   showCancelButton: false,
      //   title: '$checkDevice' + this.$checkDevice() + this.audioInstance.stop(),
      //   onConfirm() {
      //   }
      // });
      console.log(this.invisible)
    },
    beforeDestroy() {
      // WeixinJSBridge = undefined;
      // this.audioInstance.src = null;
    },
    methods: {
      playEvent() {
        this.innerPlay = !this.innerPlay;


      },
      initMusic() {
        console.log('initMusic', this.play)
        // alert('initMusic+++'+this.play)
        let create = () => {
          // this.audioInstance = document.createElement('audio');
          // this.audioInstance = document.getElementById('commonAudio')
          // debugger
          this.audioInstance = this.initInstance();
          console.log('create', this.audioInstance)
          // let parentElement = document.querySelector('.common_musicbutton_wrapper')
          // debugger
          // parentElement.appendChild(this.audioInstance)
          this.audioInstance.setAttribute('id', this.name)
          this.audioInstanceArr.push({
            name: this.name,
            instance: this.audioInstance
          });
        };

        if (this.$checkEnvironment() === 'wechat') {
          // alert('wechat')
          // alert(WeixinJSBridge)
          // console.log('WeixinJSBridge', WeixinJSBridge)
          this.jsBridgeReady().then(async () => {

            create();
          })

        } else {
          // debugger

          create();

        }


      },
      initInstance() {
        // debugger
        let that = this;
        // this.audioInstance = document.getElementById("music");
        // let instance = document.createElement('audio');
        let instance = new Audio();
        let musicReady = false;
        let sourceInstance = document.createElement('source');
        // document.getElementById('commonAudio').appen0dChild(this.audioInstance);
        instance.src = this.url;
        // instance.autoplay=true;
        // instance.muted = true;

        // instance.preload = 'auto';
        // debugger
        instance.loop = this.loop;
        console.log('instance', instance)
        // alert(instance.src)
        // alert(this.$checkDevice())

        // instance.onload(() => {
        //   alert('onload')
        //
        // })
        console.log('initInstance', instance)

        // alert(this.$checkEnvironment())

        instance.addEventListener("ended", event => {
          this.$vux.confirm.show({
            showCancelButton: false,
            title: 'ended++++' + this.$checkDevice(),
            onConfirm() {
            }
          });

          if (this.loop) {
            instance.play();

          }
        }, false);

        if (this.$checkDevice() === 'ios') {
          alert('instanceReady ios+++' + instance.src)

          try {
            instance.load();

          } catch (err) {
            console.log('load err', err)
          }
          // instance.addEventListener('load', () => {
          //   alert('onload')
          //
          // })
          this.loading = false;

          musicReady = true;
          // alert('musicReady+++' + musicReady)
          that.$emit('ready', musicReady)
          that.$emit('loading', this.loading)
        } else {
          instance.addEventListener("canplay", event => {
            alert('instanceReady+++' + instance.src)
            this.loading = false;
            console.log('canplay', event)
            musicReady = true;
            that.$emit('ready', musicReady)
            that.$emit('loading', this.loading)
          }, false);
        }

        return instance;


      },
      jsBridgeReady() {
        return new Promise((resolve, reject) => {
          // alert('check')
          if (typeof WeixinJSBridge === 'undefined') {
            alert('WeixinJSBridge undefined')
            if (document.addEventListener) {
              document.addEventListener('WeixinJSBridgeReady', () => {
                resolve(WeixinJSBridge)
                alert('jsbridge ready1')

                console.log('WeixinJSBridge', JSON.stringify(WeixinJSBridge))
              }, false);
            }
          } else {
            alert('WeixinJSBridge exist+++' + WeixinJSBridge.toString())
            console.log('WeixinJSBridge exist', JSON.stringify(WeixinJSBridge))
            // if (document.addEventListener) {
            //   document.addEventListener('WeixinJSBridgeReady', ()=>{
            //     resolve(WeixinJSBridge)
            //   }, false);
            // }
            resolve(WeixinJSBridge)
          }
        })
      },
      controlMusic(playFlag, audioInstance) {
        // debugger
        if (playFlag === true) {
          console.log('playFlag', playFlag)
          console.log('audioInstance', audioInstance)
          console.log('audioInstance.play()', audioInstance.play() instanceof Promise)

          if (this.$checkDevice() === 'ios') {

            audioInstance.muted = false;
            // audioInstance.play().then(resolve => {
            //   console.log('play resolve', resolve)
            //   this.iconRotateFlag = playFlag;
            // }).catch(err => {
            //   console.log('play err', err)
            // })
            audioInstance.addEventListener("canplay", event => {
              audioInstance.play().then(resolve => {
                console.log('play resolve', resolve)
                this.iconRotateFlag = playFlag;
              }).catch(err => {
                console.log('play err', err)
              })

            })
          } else {
            setTimeout(() => {
              audioInstance.muted = false;

              // audioInstance.play().then(resolve => {
              //   console.log('play resolve', resolve)
              //   this.iconRotateFlag = playFlag;
              // }).catch(err => {
              //   console.log('play err', err)
              // })
              this.iconRotateFlag = playFlag;
            }, 500);


          }

          this.$emit('update:play', playFlag)
        } else {

          audioInstance.pause();
          this.iconRotateFlag = playFlag;
          this.$emit('update:play', playFlag)

        }
      }
    }
  }
</script>

<style scoped>

</style>
