<template>
	<div class="information_main_container">
		<div class="common_logo_wrapper">
			<div class="common_logo_item"></div>
		</div>
		<div class="common_title_item">
			<img src="@/image/common/title_00000.png" alt />
			<img src alt />
		</div>
		<div class="content">
			<div class="common_subtitle_item">
				<img src="@/image/information/subtitle_00000.png" alt />
			</div>
			<div class="frame_wrapper">
				<!-- <div class="frame"></div> -->
				<div class="swiper-container">
					<ul class="swiper-wrapper">
						<li class="swiper-slide">
							<img src="@/image/information/slide_item_00000.png" alt />
						</li>
						<li class="swiper-slide">
							<img src="@/image/information/slide_item_00000.png" alt />
						</li>
						<li class="swiper-slide">
							<img src="@/image/information/slide_item_00000.png" alt />
						</li>
					</ul>
				</div>
			</div>
			<div class="common_navigation_item">
				<router-link :to="{name:'customize'}" />
			</div>
			<div class="common_goback_wrapper">
				<CommonGoBack />
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
			goToNextflag: false
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
		remUnit(value) {
			this.$nextTick(() => {
				let windowWidth = document.body.clientWidth.toString().replace('px', '');
				this.canvasWidth = document.body.clientWidth + 'px';
				this.canvasHeight = document.body.clientHeight + 'px';
			});
		},
	},

	mounted() {

		this.init();

	},
	methods: {
		init() {
			// console.log(FrameAnimation)
			const swiper = new Swiper('.swiper-container', {
				loop: true, // 循环模式选项

			})
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
