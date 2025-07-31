<template>
  <div>
   
  </div>
</template>

<script>
import echarts from "echarts";
import InforCard from "_c/info-card";
import CountTo from "_c/count-to";
import { ChartPie, ChartBar } from "_c/charts";
import Example from "./example.vue";
export default {
  name: "home",
  components: {
    InforCard,
    CountTo,
    ChartPie,
    ChartBar,
    Example
  },
  data() {
    var self = this;
    return {
      date_range:"",
      dateline_range:"",
      getpieData: "",
      getbarata:"",
      getlinedata:"",
      userinfo: "",
      start_date:"",
      end_date:"",
      judicial_id: -1,
      topdata: {},
      dom: null,
      inforCardData: [],
      pieData: [],
      barData: {
        Mon: 13253,
        Tue: 34235,
        Wed: 26321,
        Thu: 12340,
        Fri: 24643,
        Sat: 1322,
        Sun: 1324
      }
    };
  },
  mounted() {
  },
  methods: {
  },
  created() {
    this.userinfo = this.$global.getUserinfo();
  }
};
</script>

<style lang="less">
.count-style {
  font-size: 30px;
}
</style>
