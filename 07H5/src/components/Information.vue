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
							<img src="@/image/information/slide_item_2_00000.png" alt />
						</li>
						<li class="swiper-slide">
							<img src="@/image/information/slide_item_3_00000.png" alt />
						</li>
						<li class="swiper-slide">
							<img src="@/image/information/slide_item_4_00000.png" alt />
						</li>
					</ul>
				</div>
			</div>
			<div class="position">
				<img src="@/image/information/position_00000.png" alt />
			</div>
			<div class="common_navigation_item">
				<a href="http://api.map.baidu.com/direction?origin=22.54605355,114.02597366&destination=22.609562,114.029243&mode=driving&region=深圳&output=html" ></a>
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
		getLocation() {
			wx.openLocation({
				latitude: 0, // 纬度，浮点数，范围为90 ~ -90
				longitude: 0, // 经度，浮点数，范围为180 ~ -180。
				name: '天安门', // 位置名
				address: '', // 地址详情说明
				scale: 1, // 地图缩放级别,整形值,范围从1~28。默认为最大
				infoUrl: '' // 在查看位置界面底部显示的超链接,可点击跳转
			});


		}
	}
}
</script>

<style scoped>
</style>
