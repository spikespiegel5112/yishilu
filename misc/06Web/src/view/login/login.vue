<style lang="less">
	@import './login.less';
</style>

<template>
	<div class="login">
		<div class="login-con">
			<Card icon="log-in" title="欢迎登录" :bordered="false">
				<div class="form-con">
					<login-form @on-success-valid="handleNewSubmit"></login-form>
					<p class="login-tip">请输入输入正确的用户名和密码</p>
				</div>
			</Card>
		</div>
	</div>
	
</template>

<script>
	import LoginForm from '_c/login-form'
	import {
		mapActions
	} from 'vuex'
	export default {
		components: {
			LoginForm
		},
		methods: {
			...mapActions([
				'handleLogin',
				'handleNewLogin',
				'getUserInfo'
			]),
			handleSubmit({
				userName,
				password
			}) {
				this.handleLogin({
					userName,
					password
				}).then(res => {
					this.getUserInfo().then(res => {
						this.$router.push({
							name: this.$config.homeName
						})
					})
				})
			},
			handleNewSubmit({
				userName,
				password
			}) {
				this.$axios.post('sys.user.login', {
					UserName: userName,
					PassWord: password
				}).then(p => {
					if (p.data.state == '200') {
						this.handleNewLogin(p.data);
						sessionStorage.setItem('comco_userInfo', JSON.stringify(p.data.data));
						this.$router.replace({
							name: this.$config.homeName
						})	
					} 
					else if (p.data.state == '110') {
						this.$Message.warning("登录失败，该账号被禁用");
					}
					else {
						this.$Message.warning("登录失败，用户名密码不匹配");
					}
				})

			}
		}
	}
</script>

<style>

</style>
