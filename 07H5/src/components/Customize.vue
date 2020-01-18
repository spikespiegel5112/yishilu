<template>
	<div class="customize_main_container">
		<div class="common_logo_wrapper">
			<div class="common_logo_item"></div>
		</div>
		<div class="common_title_item">
			<img src="@/image/common/title_00000.png" alt />
			<img src alt />
		</div>
		<div class="content">
			<div class="common_subtitle_item">
				<img src="@/image/customize/subtitle_00000.png" alt />
			</div>
			<div class="longpicture_wrapper">
				<div class="swiper-container">
					<ul class="swiper-wrapper">
						<li class="swiper-slide">
							<a class="point1" href="javascript:;" @click="checkDetail(0)">
								<span></span>
							</a>
							<a class="point2" href="javascript:;" @click="checkDetail(1)">
								<span></span>
							</a>
							<a class="point3" href="javascript:;" @click="checkDetail(2)">
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
		<div v-if="dialogFlag" class="common_dialog_container">
			<div class="dialog_wrapper">
				<a href="javascript:;" class="close" @click="close"></a>
				<div class="content">
					<div class="title">{{dialogData[dialogIndex].title}}</div>
					<div class="desc">
						<div class="picture">
							<img :src="dialogData[dialogIndex].image" alt />
						</div>
						<div class="main">
							<p v-for="item in dialogData[dialogIndex].paragraph">{{item}}</p>
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
			dialogFlag: false,
			dialogIndex: 0,
			dialogData: [{
				title: '成人视活力赋能',
				paragraph: ['店内场景的标准化指引', '明星产品推荐', '演示道具配置说明', '眼健康设备筛查验配等'],
				image: require('@/image/customize/dialog_picture_3_00000.jpg')
			}, {
				title: '中生代视耐力续航',
				paragraph: ['店内场景的标准化指引', '明星产品推荐', '演示道具配置说明', '眼健康设备筛查验配等'],
				image: require('@/image/customize/dialog_picture_2_00000.jpg')
			}, {
				title: '青少年视潜力管理',
				paragraph: ['店内场景的标准化指引', '明星产品推荐', '演示道具配置说明', '眼健康设备筛查验配等'],
				image: require('@/image/customize/dialog_picture_1_00000.jpg')
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
			this.dialogIndex = index
		},
		close() {
			this.dialogFlag = false

		}
	}
}
</script>

<style scoped>
</style>
