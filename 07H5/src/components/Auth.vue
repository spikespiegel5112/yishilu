<template>
	<div class="common_auth_cotainer">
		<div class="content">
			<CommonLoading :loading="true"></CommonLoading>
			<div class="title">auth...</div>
		</div>
	</div>
</template>

<script>
export default {
	data() {
		return {
			getUserInfoRequest: 'api/security/login/info',
			environment: '',
			userInfo: null
		}
	},
	created() {
		// this.$vux.confirm.show({
		//   showCancelButton: false,
		//   title: '$checkDevice+++' + this.$checkDevice(),
		//   onConfirm() {
		//   }
		// });
		// debugger
		// alert('aaa')
		console.log('environment', this.environment)

		this.environment = this.$isEmpty(this.$webStorage.getItem('environment')) ? this.$checkEnvironment() : this.$webStorage.getItem('environment');
		this.userInfo = this.$webStorage.getItem('userInfo')
		const authToken = this.$webStorage.getItem('authToken')



		// debugger
		console.log('location+++', location)
		if (this.environment === 'wechat') {
			// if (this.environment === 'others') {
			alert('begin auth+++++' + this.environment)
			alert('this.userInfo+++++' + this.userInfo)

			const code = this.getParameter('code')
			if (this.$isEmpty(this.userInfo) || this.userInfo === 'null') {
				debugger
				if (this.$isEmpty(code)) {
					debugger
					alert('isEmpty code+++++' + code)

					location.href = this.getOAuthUrl()
				} else {
					console.log('hascode++++++', code)
					debugger
					this.getWeChatUserInfoByCode(code).then(response => {
						console.log('hascode resolve++++++', code)
						debugger
						this.redirectToBackRoute();
					}).catch(error => {
						console.log('hascode reject++++++', code)
						location.href = this.getOAuthUrl()
					})

				}

			}


		} else {
			console.log('begin auth+++++' + this.environment)
			this.$webStorage.setItem('environment', 'others');
			this.getUserInfoByOpenId("oPxr9wlKa8Gbr-dxJwWx4GSqG_1g").then(response => {
				if (response.data) {
					this.$webStorage.setItem('userInfo', JSON.stringify(response.data));
					this.$store.commit('setUserInfo', response.data)
					// this.userInfoFlag = true
					this.redirectToBackRoute();
				}
			}).catch(error => {
				console.log(error);
			})


		}
	},
	mounted() {
		alert('auth')
	},
	methods: {
		getWeChatUserInfoByCode(code) {
			return new Promise((resolve, reject) => {
				this.$http.post('wx.login.openid', { code: code }).then(response => {
					console.log('getWeChatUserInfoByCode++++', response);
					if (response.state != 200) {
						reject()
						// location.href = this.getOAuthUrl()
					}
					if (response.data) {
						const openId = response.data.openId
						this.getUserInfoByOpenId(openId).then(response => {
							if (response.data) {
								this.$webStorage.setItem('userInfo', JSON.stringify(response.data));
								this.$store.commit('setUserInfo', response.data)
								this.userInfoFlag = true
								resolve(response.data)
							}
						}).catch(error => {
							console.log(error);
							reject(error)
						})

					} else {
						//留在当前页
					}
				}).catch(error => {
					console.log(error);
					reject(error)
				});
			})
		},
		getOAuthUrl() {
			console.log('getOAuthUrl++++++', location)
			return "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxb2602761a856bbea&redirect_uri=" + encodeURIComponent(location.href) + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
		},
		// 获取用户信息
		redirectToBackRoute() {
			console.log('location+++++', location)
			let backRoute = this.$webStorage.getItem('backRoute');
			let url = '';
			console.log(location.href.indexOf('.html'))

			if (location.href.indexOf('.html') > 0) {

				url = location.href.split('.html')[0] + '.html#' + backRoute;
			} else {

				url = location.origin + location.pathname + '#' + backRoute
			}

			// alert(this.environment + ' environment url is ' + url)
			setTimeout(() => {
				location.replace(url)
			}, 100)
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
		testlogin() {
			return new Promise((resolve, reject) => {
				this.getUserInfoByOpenId("oPxr9wlKa8Gbr-dxJwWx4GSqG_1g").then(response => {
					if (response.data) {
						this.$webStorage.setItem('userInfo', JSON.stringify(response.data));
						this.userInfoFlag = true
						resolve(response)
					}
				}).catch(error => {
					console.log(error);
					reject(error)
				})
			})

		},
		getUserInfoByOpenId(openId) {
			return new Promise((resolve, reject) => {
				this.$http.get(this.$baseUrl + "wx.login.user.byopenid", { params: { openid: openId } }).then(response => {
					debugger
					if (response.data) {
						this.$webStorage.setItem('userInfo', JSON.stringify(response.data));
						this.userInfoFlag = true
						resolve(response)
					}
				}).catch(error => {
					console.log(error);
					reject(error)
				});
			})
		},
		isEmpty(value) {
			return typeof (value) === 'undefined' || value === null || value === ''
		},
	}
}
</script>
