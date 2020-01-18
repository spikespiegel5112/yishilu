<template>
  <div class="interaction_main_container">
    <div class="common_logo_wrapper">
      <div class="common_logo_item"></div>
    </div>
    <div class="bg"></div>
    <div class="content">
      <div class="common_subtitle_item">
        <img src="@/image/interaction/subtitle_00000.png" alt />
      </div>
      <div class="interaction_status_wrapper">
        <ul>
          <li class="yan" @click="receivetask(1)">
            <a href="javascript:;">
              <img class="disabled" src="@/image/interaction/button_interaction_yan_00000.png" alt />
              <img :class="yan_enabled" src="@/image/interaction/button_interaction_yan_active_00000.png" alt />
            </a>
          </li>
          <li class="shi" @click="receivetask(2)">
            <a href="javascript:;">
              <img class="disabled" src="@/image/interaction/button_interaction_shi_00000.png" alt />
              <img :class="shi_enabled" src="@/image/interaction/button_interaction_shi_active_00000.png" alt />
            </a>
          </li>
          <li class="guang" @click="receivetask(3)">
            <a href="javascript:;">
              <img class="disabled" src="@/image/interaction/button_interaction_guang_00000.png" alt />
              <img :class="guang_enabled" src="@/image/interaction/button_interaction_guang_active_00000.png" alt />
            </a>
          </li>
        </ul>
      </div>
      <div class="rules">
        <p>游戏规则：</p>
        <p>点击上方图标，找到“眼-依视路”、“视-优视路”、“光-全视线”展位现场的互动二维码，使用微信的扫一扫功能，扫码点亮本页标签，即可参加抽奖活动，有机会获得暖心礼品一份。</p>
      </div>
      <div class="common_navigation_item">
        <a href="javascript:;" @click="checkStatus"></a>
      </div>
      <div class="common_goback_wrapper">
        <CommonGoBack />
      </div>
    </div>
    <div v-if="dialogNotYetFlag" class="common_dialog_container notyet">
      <div class="dialog_wrapper">
        <a href="javascript:;" class="close" @click="closeNotYet"></a>
        <div class="content">
          <p>您尚未完成全部打卡</p>
          <p>请全部完成后进行抽奖</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
  import Swiper from 'swiper';

  export default {
    name: "Information",
    components: {},
    data() {
      return {
        goToNextflag: false,
        dialogNotYetFlag: false,
        status: false,
        yan_enabled: "enabled",
        shi_enabled: "enabled",
        guang_enabled: "enabled",
        userinfo: {},
        task_type:0
      };
    },
    computed: {
      navigatorStyle() {
        return {
          'background-image': "url(" + require('@/image/homepage/navigator_00000.png') + ")"
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
    },

    mounted() {
      setTimeout(() => {
        this.init();

      }, 100);
      this.userinfo = JSON.parse(this.$webStorage.getItem('userInfo'));
       this.task_type=  this.getParameter('task_type');
       console.log(this.task_type);
       if(this.task_type){
         this.lighttask();
       }
       else{
         this.getlighten();
       }
    },
    methods: {
      lighttask(){  //点亮任务
        var _this = this;
        _this.$http.post(
          'h5.user.light.task', {
            User_Id: _this.userinfo.id,
            Task_Type:this.task_type
          }).then(r => {
          _this.$vux.confirm.show({
            showCancelButton: false,
            title: r.msg,
            onConfirm() {}
          });
          _this.getlighten();
        }).catch(error => {
          console.log(error)
        })
      },
      getlighten(){  //获取用户点亮的任务
        var _this = this;
        this.$http.get(this.$baseUrl + "h5.get.wxuser.lighten", { params: { u_id: this.userinfo.id } }).then(response => {
        	if (response.data) {
           _this.yan_enabled=response.data.f_eye?"disabled":"enabled";
           _this.shi_enabled=response.data.f_regard?"disabled":"enabled";
           _this.guang_enabled=response.data.f_light?"disabled":"enabled";
        	}
        }).catch(error => {
        	console.log(error)
        })
      },
      getParameter(key) {
      	var url = location.href
      	var paraString = url.substring(url.indexOf('?') + 1, url.length).split('&')
      	var paraObj = {}
      	for (var i = 0, len = paraString.length; i < len; i++) {
      		var j = paraString[i]
      		paraObj[j.substring(0, j.indexOf('=')).toLowerCase()] = j.substring(j.indexOf('=') + 1, j.length)
      	}
      	var returnValue = paraObj[key.toLowerCase()]
      	if (typeof (returnValue) === 'undefined') {
      		return ''
      	} else {
      		return returnValue
      	}
      },
      receivetask(type) {  //领取任务
        let confirmmsg = "";
        switch (type) {
          case 1:
            confirmmsg = "确定领取眼任务吗";
            break;
          case 2:
            confirmmsg = "确定领取视任务吗";
            break;
          case 3:
            confirmmsg = "确定领取光任务吗";
            break;
          default:
            break;
        }
        var _this = this;
        this.$vux.confirm.show({
          showCancelButton: true,
          title: confirmmsg,
          onConfirm() {
            _this.$http.post(
              'h5.user.receive.task', {
                User_Id: _this.userinfo.id,
                Task_Type: type
              }).then(r => {
              console.log(r);
              _this.$vux.confirm.show({
                showCancelButton: false,
                title: r.msg,
                onConfirm() {}
              })
            }).catch(error => {
              console.log(error)
            })
          }
        });
      },
      init() {
        // console.log(FrameAnimation)
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
      checkStatus() {
        if(this.yan_enabled=="disabled"&&this.shi_enabled=="disabled"&&this.guang_enabled=="disabled"){
          this.$router.push({
            name: 'lotteryDraw'
          })
        }else{
            this.dialogNotYetFlag = true
        }
      },
      closeNotYet() {
        this.dialogNotYetFlag = false
      }

    }
  }
</script>

<style scoped>
</style>
