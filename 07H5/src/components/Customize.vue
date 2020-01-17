<template>
	<div class="customize_main_container">
		<div class="common_title_item">
			<img src="@/image/common/title_00000.png" alt />
			<img src alt />
		</div>
		<div class="content">
			<div class="common_subtitle_item">
				<img src="@/image/customize/slide_item_customize_00000.png" alt />
			</div>
			<div class="longpicture_wrapper">
				<div class="swiper-container">
					<ul class="swiper-wrapper">
						<li class="swiper-slide">
							<a href="javascript:;" @click="checkDetail(1)">
								<span></span>
							</a>
							<img src="@/image/customize/long_picture_00000.jpg" alt />
						</li>
					</ul>
				</div>
			</div>
			<div class="hint">
				<p>请左右拖动查看全景照片</p>
			</div>
			<div class="common_goback_wrapper">
				<CommonGoBack />
			</div>
		</div>
		<div v-if="dialogFlag" class="dialog_container">
			<div class="dialog_wrapper">
				<a href="javascript:;" class="close" @click="close"></a>
				<div class="content">
					<div class="picture">
						<img src="@/image/customize/dialog_picture_00000.jpg" alt />
					</div>
					<div class="desc">
						<div class="title">dsadasdas</div>
						<div class="main">
							<p>dasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsada</p>

							<p>dasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsada</p>
							<p>dasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsadadasdasdsada</p>
						</div>
					</div>
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
			dialogFlag: false
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
		setTimeout(() => {
			this.init();

		}, 100)

	},
	methods: {
		init() {
			// console.log(FrameAnimation)
			const swiper = new Swiper('.swiper-container', {
				direction: 'horizontal',
				slidesPerView: 'auto',
				freeMode: true,
				// scrollbar: {
				// 	el: '.swiper-scrollbar',
				// },
				mousewheel: true,
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
		checkDetail(index) {
			this.dialogFlag = true
		},
		close() {
			this.dialogFlag = false

		}
	}
}
</script>

<style scoped>
</style>
