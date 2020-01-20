<template>
	<div class="interaction_main_container">
		<div class="common_logo_wrapper">
			<div class="common_logo_item"></div>
		</div>
		<div class="common_title_item">
			<img src="@/image/common/title_00000.png" alt />
		</div>
		<!-- <div class="bg"></div> -->
		<div class="content">
			<div class="common_subtitle_item">
				<img src="@/image/interaction/subtitle_00000.png" alt />
			</div>
			<div class="interaction_status_wrapper">
				<ul>
					<li class="yan" @click="receivetask(1)">
						<a href="javascript:;">
							<img class="disabled" src="@/image/interaction/button_interaction_yan_00000.png" alt />
							<img
								:class="yan_enabled"
								src="@/image/interaction/button_interaction_yan_active_00000.png"
								alt
							/>
						</a>
					</li>
					<li class="shi" @click="receivetask(2)">
						<a href="javascript:;">
							<img class="disabled" src="@/image/interaction/button_interaction_shi_00000.png" alt />
							<img
								:class="shi_enabled"
								src="@/image/interaction/button_interaction_shi_active_00000.png"
								alt
							/>
						</a>
					</li>
					<li class="guang" @click="receivetask(3)">
						<a href="javascript:;">
							<img class="disabled" src="@/image/interaction/button_interaction_guang_00000.png" alt />
							<img
								:class="guang_enabled"
								src="@/image/interaction/button_interaction_guang_active_00000.png"
								alt
							/>
						</a>
					</li>
				</ul>
			</div>
			<div class="rules">
				<p>游戏规则：</p>
				<p>点击上方图标，找到“眼-依视路设备”、“视-优视路”、“光-全视线”展位现场的互动二维码，使用微信的扫一扫功能，扫码点亮本页标签，即可参加抽奖活动，有机会获得暖心礼品一份。</p>
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
				<a href="javascript:;" class="close" @click="closeDialog"></a>
				<div class="content">
					<p>您尚未完成全部打卡</p>
					<p>请全部完成后进行抽奖</p>
				</div>
			</div>
		</div>
		<div v-if="dialogYanFlag" class="common_dialog_container prompt">
			<div class="dialog_wrapper">
				<a href="javascript:;" class="close" @click="closeDialog"></a>
				<div class="content">
					<p class="title">
						<span>眼</span>依视路设备
					</p>
					<p class="desc">
						给大众更全面、更完善的眼健康
						<br />和视觉质量解决方案。
					</p>
					<p class="position">展位号：二号馆2G40-2</p>
				</div>
			</div>
		</div>
		<div v-if="dialogShiFlag" class="common_dialog_container prompt">
			<div class="dialog_wrapper">
				<a href="javascript:;" class="close" @click="closeDialog"></a>
				<div class="content">
					<p class="title">
						<span>视</span>依视路
					</p>
					<p class="desc">
						依视路改善视力，改善生活，
						<br />让清晰视觉变得触手可得
					</p>
					<p class="position">展位号：一号馆1A02</p>
				</div>
			</div>
		</div>
		<div v-if="dialogGuangFlag" class="common_dialog_container prompt">
			<div class="dialog_wrapper">
				<a href="javascript:;" class="close" @click="closeDialog"></a>
				<div class="content">
					<p class="title">
						<span>光</span>全视线
					</p>
					<p class="desc">
						全视线第八代感光变色镜片，
						<br />前沿科技，光彩上市
					</p>
					<p class="position">展位号：二号馆2F06</p>
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
			task_type: 0
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
		//task_type    1:眼  2:视  3:光
		this.task_type = this.getParameter('task_type');
		console.log(this.task_type);
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
			if (this.yan_enabled == "disabled" && this.shi_enabled == "disabled" && this.guang_enabled == "disabled") {
				this.$router.push({
					name: 'lotteryDraw'
				})
			} else {
				this.dialogNotYetFlag = true
			}
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
