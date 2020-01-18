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
					<li class="yan">
						<a href="javascript:;">
							<img class="disabled" src="@/image/interaction/button_interaction_yan_00000.png" alt />
							<img class="enabled" src="@/image/interaction/button_interaction_yan_active_00000.png" alt />
						</a>
					</li>

					<li class="shi">
						<a href="javascript:;">
							<img class="disabled" src="@/image/interaction/button_interaction_shi_00000.png" alt />
							<img class="enabled" src="@/image/interaction/button_interaction_shi_active_00000.png" alt />
						</a>
					</li>
					<li class="guang">
						<a href="javascript:;">
							<img class="disabled" src="@/image/interaction/button_interaction_guang_00000.png" alt />
							<img class="enabled" src="@/image/interaction/button_interaction_guang_active_00000.png" alt />
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
	components: {
	},
	data() {
		return {
			goToNextflag: false,
			dialogNotYetFlag: false,
			status: false
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

		}, 100)

	},
	methods: {
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
			if (this.status) {
				this.$router.push({
					name: 'lotteryDraw'
				})
			} else {
				this.$router.push({
					name: 'lotteryDraw'
				})
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
