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
          <router-link href="javascript:;" :to="{ name: 'information' }" />
        </li>
        <li class="link2">
          <router-link href="javascript:;" :to="{ name: 'interaction' }" />
        </li>
        <li class="link3">
          <router-link href="javascript:;" :to="{ name: 'category' }" />
        </li>
        <li class="link4">
          <router-link href="javascript:;" :to="{ name: 'customize' }" />
        </li>
        <li class="link5">
          <a href="javascript:;" @click="checkCollection"></a>
        </li>
      </ul>
    </div>
    <div v-if="collectionNotYetFlag" class="common_dialog_container collection">
      <div class="dialog_wrapper">
        <a href="javascript:;" class="close" @click="closeDialog"></a>
        <div class="content">
          <p class="title">活动尚未开始，敬请期待</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "Homepage",
  components: {},
  data() {
    return {
      goToNextflag: false,
      collectionNotYetFlag: false,
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

  mounted() {
    // this.$autoHeight({
    //   target: '.entrance_main_container',
    //   force: true
    // });
    this.$nextTick(() => {
      this.remUnit = Number(
        document
          .getElementsByTagName("html")[0]
          .style.fontSize.replace("px", "")
      );
    });
  },
  methods: {
    checkCollection() {
      const destinationTimeStamp = this.$moment(
        new Date("2020-2-11")
      ).valueOf();
      const currentTimeStamp = Date.parse(new Date());
      console.log(destinationTimeStamp);
      console.log(currentTimeStamp);
      if (currentTimeStamp > destinationTimeStamp) {
        this.$router.push({
          name: "entrance",
        });
      } else {
        this.collectionNotYetFlag = true;
      }
    },
    closeDialog() {
      this.collectionNotYetFlag = false;
    },
  },
};
</script>

<style scoped></style>
