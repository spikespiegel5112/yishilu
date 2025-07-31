<template>
	<div class="entrance_main_container">
		<div class="content">
			<CommonLoading :loading="loading" />
			<img :src="entranceImage" alt />
		</div>
	</div>
</template>

<script>

export default {
	name: "Entrance",
	components: {
	},
	data() {
		return {
			goToNextflag: false,
			countdownFlag: false,
			loading: true,
			userinfo: {},
			entranceImage: require('@/image/entrance/entrance_00000.png'),
			entranceImagePath: require('@/image/entrance/entrance_00000.png')
		};
	},
	computed: {
	},
	watch: {
		countdownFlag(value) {
			if (value) {
				this.goToNextPage()
			}
		},
		goToNextflag(value) {

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
		this.userinfo = JSON.parse(this.$webStorage.getItem('userInfo'));
		this.getImage()

		this.addscancount();
	},
	methods: {
		getImage() {
			const imageObject = new Image()
			imageObject.src = this.entranceImagePath
			imageObject.onload = () => {
				this.loading = false
				this.countdownFlag = true
			}
		},
		addscancount() {
			this.$http.get(this.$baseUrl + "h5.wxuser.pageaccess", { params: { u_id: this.userinfo.id, scan_type: 0 } }).then(response => {
				console.log(response)
			}).catch(error => {
				console.log(error)
			})
		},
		goToNextPage() {
			const duration = 3000
			setTimeout(() => {
				this.$router.push({
					name: 'homePage'
				});
			}, duration)
		},
		init() {
			// console.log(FrameAnimation)

		},

	}
}
</script>

<style scoped>
</style>
