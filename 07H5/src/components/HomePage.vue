<template>
	<div class="homepage_main_container">
		<div class="common_title_item">
			<img src="@/image/common/title_00000.png" alt />
			<img src alt />
		</div>
		<div class="content">
			<div class="bg" :style="navigatorStyle"></div>

			<!-- <img src="@/image/homepage/navigator_00000.png" alt /> -->
			<ul class="links">
				<li class="link1">
					<router-link href="javascript:;" :to="{name:'information'}" />
				</li>
				<li class="link2">
					<router-link href="javascript:;" :to="{name:'entrance'}" />
				</li>
				<li class="link3">
					<router-link href="javascript:;" :to="{name:'entrance'}" />
				</li>
				<li class="link4">
					<router-link href="javascript:;" :to="{name:'customize'}" />
				</li>
				<li class="link5">
					<router-link href="javascript:;" :to="{name:'entrance'}" />
				</li> 
			</ul>
		</div>
	</div>
</template>

<script>
// import { getUserInfo } from '../../api/auth.js'

export default {
  name: "Homepage",
  components: {
  },
  data() {
    return {
      goToNextflag: false
    };
  },
  computed: {
    navigatorStyle(){
      return{
        'background-image':"url("+require('@/image/homepage/navigator_00000.png')+")"
      }
    }
  },
  watch: {
    goToNextflag(value) {
      if (value === true) {
        this.$router.push({
          name: 'homepage'
        });

      }
    },
    remUnit(value) {
      this.$nextTick(() => {
        let windowWidth = document.body.clientWidth.toString().replace('px', '');
        this.canvasWidth = document.body.clientWidth + 'px';
        this.canvasHeight = document.body.clientHeight + 'px';
      });
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
      this.remUnit = Number(document.getElementsByTagName('html')[0].style.fontSize.replace('px', ''));


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
          return 'http://pp-jgxzq.oss-cn-qingdao.aliyuncs.com/ctzcf_entrance/entrance_00' + index + '.jpg';
        };
        for (let i = 0; i <= 209; i++) {
          result.push(prologueFrameBaseUrl(i));
        }
        // console.log('prologueFramesUrl', result)

        return result;
      };
      // console.log(prologueFramesUrl())
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
            trigger(that.speechList[index1].delay);
          }
        }, delay);
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
        });
      }, 2000);

    },
    handleLoading(index) {
      // debugger
      // console.log(this.frameAnimationInstance.frameLength)
      // console.log(index)

      // this.currentLoadingPercentage = Math.ceil(100 * index / this.frameAnimationInstance.frameLength)
      this.$store.commit('updateLoadingPercentage', Math.ceil(100 * index / this.frameAnimationInstance.frameLength));
    },
    handleLoaded() {
      // debugger
      let that = this;
      this.animationLoadingFlag = false;

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
      this.$wxsdk.initConfig(params);
    },
  }
}
</script>

<style scoped>
</style>
