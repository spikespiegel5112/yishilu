<template>
	<div class="interactionlogo_main_container">
		<div class="common_logo_wrapper">
			<div class="common_logo_item"></div>
		</div>
		<div class="common_title_item">
			<img src="@/image/common/title_00000.png" alt />
		</div>
		<!-- <div class="bg"></div> -->
		<div class="content">
			<div class="common_subtitle_item">
				<img src="@/image/interactionlogo/subtitle_00000.png" alt />
			</div>
			<div class="rules">
				<ol>
					<li>请前往下方7个依视路集团子品牌展台，进行扫码打卡签到；</li>
					<li>您可点击下方Logo查看相关展台展位号，扫码完成后该展台Logo将被点亮；</li>
					<li>完成任意2个Logo点亮将赢得一次额外的抽奖机会，数量有限领完即止。</li>
				</ol>
			</div>
			<div class="status_wrapper">
				<ul>
					<li
						v-for="(item, index) in logoList"
						:key="item.name"
						:class="item.active?'active'+' '+item.name:item.name"
						@click="receivetask(index)"
					>
						<a href="javascript:;">
							<div class="enabled">
								<img :src="item.enableSrc" />
							</div>
							<div class="disabled">
								<img :src="item.disabledSrc" />
							</div>
						</a>
					</li>
				</ul>
				<div class="hint">
					<p>*排名不分先后</p>
				</div>
			</div>
			<div class="navigation_wrapper">
				<div class="common_navigation_item">
					<a href="javascript:;" @click="checkStatus">点击抽奖</a>
				</div>
				<div class="common_navigation_item">
					<a href="javascript:;" @click="checkStatus">我的奖品</a>
				</div>
			</div>
			<div class="common_goback_wrapper">
				<CommonGoBack to="lotteryDraw" />
			</div>
		</div>
		<div v-if="dialogNotYetFlag" class="common_dialog_container notyet">
			<div class="dialog_wrapper">
				<a href="javascript:;" class="close" @click="closeDialog"></a>
				<div class="content">
					<p>请前往相关展台扫码点亮</p>
					<p>任意两个Logo进行抽奖</p>
				</div>
			</div>
		</div>
	</div>
</template>

<script>

export default {
	name: "Information",
	components: {},
	data() {
		return {
			goToNextflag: false,
			dialogNotYetFlag: false,
			dialogYanFlag: false,
			dialogShiFlag: false,
			dialogGuangFlag: false,
			status: false,
			yan_enabled: "enabled",
			shi_enabled: "enabled",
			guang_enabled: "enabled",
			userinfo: {},
			task_type: 0,
			logoList: [{
				name: 'wanxin',
				enableSrc: require('@/image/interactionlogo/logo_wanxin_00000.png'),
				disabledSrc: require('@/image/interactionlogo/logo_wanxin_disabled_00000.png'),
				active: false
			}, {
				name: 'yolioptical',
				enableSrc: require('@/image/interactionlogo/logo_yolioptical_00000.png'),
				disabledSrc: require('@/image/interactionlogo/logo_yolioptical_disabled_00000.png'),
				active: false
			}, {
				name: 'chemilens',
				enableSrc: require('@/image/interactionlogo/logo_chemilens_00000.png'),
				disabledSrc: require('@/image/interactionlogo/logo_chemilens_disabled_00000.png'),
				active: false
			}, {
				name: 'seeworld',
				enableSrc: require('@/image/interactionlogo/logo_seeworld_00000.png'),
				disabledSrc: require('@/image/interactionlogo/logo_seeworld_disabled_00000.png'),
				active: false
			}, {
				name: 'creasky',
				enableSrc: require('@/image/interactionlogo/logo_creasky_00000.png'),
				disabledSrc: require('@/image/interactionlogo/logo_creasky_disabled_00000.png'),
				active: false
			}, {
				name: 'newtianhongoptical',
				enableSrc: require('@/image/interactionlogo/logo_newtianhongoptical_00000.png'),
				disabledSrc: require('@/image/interactionlogo/logo_newtianhongoptical_disabled_00000.png'),
				active: false
			}, {
				name: 'bolon',
				enableSrc: require('@/image/interactionlogo/logo_bolon_00000.png'),
				disabledSrc: require('@/image/interactionlogo/logo_bolon_disabled_00000.png'),
				active: false
			}]
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
		this.userinfo = JSON.parse(this.$webStorage.getItem('userInfo'));
		console.log("this.$webStorage.getItem('userInfo')++++++", this.$webStorage.getItem('userInfo'))
		//task_type    1:眼  2:视  3:光
		this.task_type = this.getParameter('task_type');
		console.log('this.task_type+++', this.task_type);
		if (this.task_type) {
			this.lighttask();
			this.addscancount();
		} else {
			this.getlighten();
		}
	},
	methods: {
		lighttask() {  //点亮任务
			this.$http.post('h5.user.light.task', {
				User_Id: this.userinfo.id,
				Task_Type: this.task_type
			}).then(r => {
				console.log('lighttask+++', r)
				this.$vux.confirm.show({
					showCancelButton: false,
					title: r.msg,
					onConfirm() { }
				});
				this.getlighten();
			}).catch(error => {
				console.log(error)
			})
		},
		getlighten() {  //获取用户点亮的任务
			this.$http.get(this.$baseUrl + "h5.get.wxuser.lighten", { params: { u_id: this.userinfo.id } }).then(response => {
				if (response.data) {
					this.yan_enabled = response.data.f_eye ? "disabled" : "enabled";
					this.shi_enabled = response.data.f_regard ? "disabled" : "enabled";
					this.guang_enabled = response.data.f_light ? "disabled" : "enabled";
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
		addscancount() {//增加扫码次数
			this.$http.get(this.$baseUrl + "h5.wxuser.pageaccess", { params: { u_id: this.userinfo.id, scan_type: this.task_type } }).then(response => {
				console.log(response)
			}).catch(error => {
				console.log(error)
			})
		},
		receivetask(type) {  //领取任务
			switch (type) {
				case 1:
					this.dialogYanFlag = true;
					break;
				case 2:
					this.dialogShiFlag = true;
					break;
				case 3:
					this.dialogGuangFlag = true;
					break;
				default:
					break;
			}
		},
		checkStatus() {
			this.dialogNotYetFlag = true

			this.$router.push({
				name: 'lotteryDrawLogo'
			})
		},
		closeDialog() {
			this.dialogFlag = false
			this.dialogNotYetFlag = false
			this.dialogYanFlag = false
			this.dialogShiFlag = false
			this.dialogGuangFlag = false
		}

	}
}
</script>

<style scoped>
</style>
