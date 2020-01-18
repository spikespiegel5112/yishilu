<template>
	<div class="homepage_main_container">
		<div class="common_logo_wrapper">
			<div class="common_logo_item"></div>
		</div>
		<div class="common_title_item">
			<img src="@/image/common/title_00000.png" alt />
		</div>
		<div class="content">
			<div class="bg" :style="navigatorStyle"></div>

			<!-- <img src="@/image/homepage/navigator_00000.png" alt /> -->
			<ul class="links">
				<li class="link1">
					<router-link href="javascript:;" :to="{name:'information'}" />
				</li>
				<li class="link2">
					<router-link href="javascript:;" :to="{name:'interaction'}" />
				</li>
				<li class="link3">
					<router-link href="javascript:;" :to="{name:'category'}" />
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
		navigatorStyle() {
			return {
				'background-image': "url(" + require('@/image/homepage/navigator_00000.png') + ")"
			}
		}
	},
	watch: {
		remUnit(value) {
			this.$nextTick(() => {
				let windowWidth = document.body.clientWidth.toString().replace('px', '');
				this.canvasWidth = document.body.clientWidth + 'px';
				this.canvasHeight = document.body.clientHeight + 'px';
			});
		},
	},

	mounted() {
		// this.$autoHeight({
		//   target: '.entrance_main_container',
		//   force: true
		// });
		this.$nextTick(() => {
			this.remUnit = Number(document.getElementsByTagName('html')[0].style.fontSize.replace('px', ''));


		});

	},
	methods: {

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
