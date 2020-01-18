<template>
	<div class="common_goback_item">
		<router-link :to="{name:to}" />
	</div>
</template>

<script>
export default {
	name: "CommonEditBg",
	props: {
		to: {
			type: String,
			default: 'homePage'
		}
	}
}
</script>

<style scoped>
</style>
