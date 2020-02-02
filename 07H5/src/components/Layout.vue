<template>
	<div class="common_main_container">
		<transition name="fade">
			<router-view v-if="userInfoFlag"></router-view>	
			<CommonLoading v-else :loading="true" />
		</transition>
	</div>
</template>

<script>
export default {
	name: "Layout",
	components: {},
	data() {
		return {
			code: "",
			environment: '',
			userInfoFlag: false

		};
	},
	computed: {

	},
	watch: {
	},
	created() {

	},
	mounted() {
		this.$remResizing({
			fontSize: 20,
			threshold: 768,
			aligncenter: true,
		});
		console.log('$webStorage:', this.$webStorage.type);
		this.sharePage();
		// this.getUserInfoCache();
		if (this.$checkEnvironment() === 'wechat') {
			// this.testlogin();//本地测试	
			this.getUserInfo();
		} else {
			this.testlogin();//本地测试	
		}

	},
	methods: {
		testlogin() {
			this.$http.get(this.$baseUrl + "wx.login.user.byopenid", { params: { openid: "oPxr9wlKa8Gbr-dxJwWx4GSqG_1g" } }).then(response => {
				if (response.data) {
					this.$webStorage.setItem('userInfo', JSON.stringify(response.data));
					this.$store.commit('setUserInfo', response.data);
					setTimeout(() => {
						this.userInfoFlag = true
					}, 200)

				}
			}).catch(error => {
				console.log(error);
			});
		},
		auto_login() {
			this.$http.post(
				'wx.login.openid', {
				code: this.code,
			}).then(r => {
				console.log(r);
				if (r.state != 200) {
					location.href = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxb2602761a856bbea&redirect_uri=" + location.href + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
				} else {
					if (r.data) {
						this.$webStorage.setItem('userInfo', JSON.stringify(r.data));
						// this.userInfoFlag = true

					} else {
						//留在当前页
					}
				}
			}).catch(error => {
				console.log(error);
			});
		},
		getParameter(key) {
			var url = location.href;
			var paraString = url.substring(url.indexOf('?') + 1, url.length).split('&');
			var paraObj = {};
			for (var i = 0, len = paraString.length; i < len; i++) {
				var j = paraString[i];
				paraObj[j.substring(0, j.indexOf('=')).toLowerCase()] = j.substring(j.indexOf('=') + 1, j.length);
			}
			var returnValue = paraObj[key.toLowerCase()];
			if (typeof (returnValue) === 'undefined') {
				return '';
			} else {
				return returnValue;
			}
		},
		// 设置sharePage分享信息
		sharePage() {
			let params = {
				share: true, // true可以分享；false不可以分享
				data: {
					title: `依视路•见未来•创视纪`, // 分享标题
					desc: '2020第十二届中国（上海）国际眼睛业展览会邀请函', // 分享描述
					link: this.$store.state.shareUrl, // 分享链接
					imgUrl: 'http://img5.imgtn.bdimg.com/it/u=4294203307,2960810096&fm=26&gp=0.jpg' // 分享图标
				}
			};
			this.$wxsdk.initConfig(params);
		},
		getUserInfoCache() {
			if (!this.$isEmpty(this.$webStorage.getItem('userInfo'))) {
				this.$store.commit('setUserInfo', JSON.parse(this.$webStorage.getItem('userInfo')));
			}
		},
		getUserInfo() {
			this.code = this.getParameter('code');

			let userinfo = this.$webStorage.getItem('userInfo');
			if (userinfo) {

				userinfo = JSON.parse(userinfo);
				this.$http.get(this.$baseUrl + "wx.login.user.byopenid", { params: { openid: userinfo.openid } }).then(response => {
					if (response.data) {
						this.$webStorage.setItem('userInfo', JSON.stringify(response.data));
						this.userInfoFlag = true
					} else {
						// location.href = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxb2602761a856bbea&redirect_uri=" +
						// 	encodeURIComponent(location.href) + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
						console.log(response.data);
					}
				}).catch(error => {
					console.log(error);
				});
			} else {
				if (this.code) {

					this.auto_login();
				} else {
					location.href = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxb2602761a856bbea&redirect_uri=" +
						encodeURIComponent(location.href) + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
				}
			}
		}
	}
}
</script>

<style scoped>
</style>
