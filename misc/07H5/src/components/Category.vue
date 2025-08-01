<template>
  <div class="category_main_container">
    <div class="common_logo_wrapper">
      <div class="common_logo_item"></div>
    </div>
    <div class="common_title_item">
      <img src="@/image/common/title_00000.png" alt />
      <img src alt />
    </div>
    <div class="content">
      <div class="common_subtitle_item">
        <img src="@/image/category/subtitle_00000.png" alt />
      </div>
      <div class="main">
        <div class="describe">
          从消费者需求出发，引导推动品类的发展，集眼科、视光学为一体，依视路为您提供保健、预防、矫治和康复的综合眼健康管理方案。
        </div>
        <img src="@/image/category/category_content_00000.png" alt="" />
      </div>
      <div class="common_goback_wrapper">
        <CommonGoBack />
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Information",
  components: {},
  data() {
    return {
      goToNextflag: false,
      dialogFlag: false,
      dialogIndex: 0,
    };
  },
  computed: {
    navigatorStyle() {
      return {
        "background-image":
          "url(" + require("@/image/homepage/navigator_00000.png") + ")",
      };
    },
  },
  watch: {
    goToNextflag(value) {
      if (value === true) {
        this.$router.push({
          name: "homepage",
        });
      }
    },
    remUnit(value) {
      this.$nextTick(() => {
        let windowWidth = document.body.clientWidth
          .toString()
          .replace("px", "");
        this.canvasWidth = document.body.clientWidth + "px";
        this.canvasHeight = document.body.clientHeight + "px";
      });
    },
  },

  mounted() {},
  methods: {},
};
</script>

<style scoped></style>
