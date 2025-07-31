<template>
  <div class="common_goback_item">
    <router-link :to="{ name: props.to }" />
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

<style scoped></style>
