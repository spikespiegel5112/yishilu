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
      goToNextflag: false,
      userinfo:{}
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
    this.userinfo= JSON.parse(this.$webStorage.getItem('userInfo'));
    this.addscancount();
  },
  methods: {
    addscancount(){
      this.$http.get(this.$baseUrl + "h5.wxuser.pageaccess", { params: { u_id: this.userinfo.id,scan_type:0 } }).then(response => {
      	console.log(response)
      }).catch(error => {
      	console.log(error)
      })
    },
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
