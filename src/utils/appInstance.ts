import { createApp } from "vue";
import App from "@/App.vue";
import router from "@/router";
import utils from "@/utils/utils";
import service from "@/utils/service";

const app = createApp(App);

import dayjs from "dayjs";
import "dayjs/locale/zh-cn";
dayjs.locale("zh-cn");

import Swiper from "swiper";
import "swiper/css";

import { store } from "@/store";

import "@/style/common.scss";

app.config.globalProperties.$dayjs = dayjs;
app.config.globalProperties.$router = router;
app.config.globalProperties.$http = service;
app.config.globalProperties.$Swiper = Swiper;

app.use(router).use(store).use(utils);

export default app;
