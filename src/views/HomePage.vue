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
    <div
      v-if="state.collectionNotYetFlag"
      class="common_dialog_container collection"
    >
      <div class="dialog_wrapper">
        <a href="javascript:;" class="close" @click="closeDialog"></a>
        <div class="content">
          <p class="title">活动尚未开始，敬请期待</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { defineComponent } from "vue";
import HelloWorld from "@/components/HelloWorld.vue"; // @ is an alias to /src
</script>
