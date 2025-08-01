<template>
  <div class="common_goback_item">
    <router-link :to="{ name: props.to }"> </router-link>
  </div>
</template>

<script lang="tsx" setup>
const props = defineProps({
  to: {
    type: String,
    default: "HomePage",
    required: false,
  },
});
</script>

<style scoped lang="scss">
.common_goback_item {
  display: inline-block;
  width: 1.2rem;
  height: 2.5rem;

  a {
    display: inline-block;
    width: 100%;
    height: 100%;
    background-image: url("@/assets/image/common/back.png");
    background-repeat: no-repeat;
    background-size: contain;
  }
}
</style>
