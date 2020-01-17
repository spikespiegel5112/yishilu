<template>
	<div class="entrance_main_container">
		<div class="content">
			<img src="@/image/entrance/entrance_00000.png" alt />
		</div>
	</div>
</template>

<script>
// import { getUserInfo } from '../../api/auth.js'

export default {
  name: "Entrance",
  components: {
  },
  data() {
    return {
      goToNextflag: false
    };
  },
  computed: {
  },
  watch: {
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
   this.goToNextPage()

  },
  methods: {
    goToNextPage(){
            const duration=3000
      setTimeout(()=>{
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
