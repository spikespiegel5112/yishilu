<template>
  <Layout style="height: 100%" class="main">
    <Sider
      hide-trigger
      collapsible
      :width="220"
      :collapsed-width="70"
      v-model="collapsed"
      class="left-sider"
      :style="{ overflow: 'hidden' }"
    >
      <side-menu
        accordion
        ref="sideMenu"
        :active-name="$route.name"
        :collapsed="collapsed"
        @on-select="turnToPage"
        :menu-list="menuList"
      >
        <!-- 需要放在菜单上面的内容，如Logo，写在side-menu标签内部，如下 -->
        <div class="logo-con">
          <img
            v-show="!collapsed"
            class="max-logo"
            :src="maxLogo"
            key="max-logo"
          />
          <img
            v-show="collapsed"
            class="mini-logo"
            :src="minLogo"
            key="min-logo"
          />
        </div>
      </side-menu>
    </Sider>
    <Layout>
      <Header class="header-con">
        <header-bar
          :collapsed="collapsed"
          @on-coll-change="handleCollapsedChange"
        >
          <!-- <user :message-unread-count="unreadCount" :user-avatar="userAvatar"/> 包含未读消息-->
          <user :user-avatar="userAvatar" />
          <!-- <language
            v-if="$config.useI18n"
            @on-lang-change="setLocal"
            style="margin-right: 10px;"
            :lang="local"
          /> -->
          <!-- <error-store v-if="$config.plugin['error-store'] && $config.plugin['error-store'].showInHeader" :has-read="hasReadErrorPage" :count="errorCount"></error-store> -->
          <fullscreen v-model="isFullscreen" style="margin-right: 20px" />
        </header-bar>
      </Header>
      <Content class="main-content-con">
        <Layout class="main-layout-con">
          <div class="tag-nav-wrapper">
            <tags-nav
              :value="$route"
              @input="handleClick"
              :list="tagNavList"
              @on-close="handleCloseTag"
            />
          </div>
          <Content class="content-wrapper">
            <keep-alive :include="cacheList">
              <router-view />
            </keep-alive>
            <ABackTop
              :height="100"
              :bottom="80"
              :right="50"
              container=".content-wrapper"
            ></ABackTop>
          </Content>
        </Layout>
      </Content>
      <Footer
        style="text-align: center; padding: 0px 50px; padding-bottom: 10px"
        >&copy;2019-2020 上海楠林科技 All rights reserved.</Footer
      >
      <audio ref="music" id="audio" loop>
        <source src="./components/user/9833.mp3" type="audio/mpeg" />
      </audio>
      <audio ref="music_notice" id="audio" loop>
        <source src="./components/user/2478.mp3" type="audio/mpeg" />
      </audio>
    </Layout>
  </Layout>
</template>
<script>
import SideMenu from "./components/side-menu";
import HeaderBar from "./components/header-bar";
import TagsNav from "./components/tags-nav";
import User from "./components/user";
import ABackTop from "./components/a-back-top";
import Fullscreen from "./components/fullscreen";
import Language from "./components/language";
import ErrorStore from "./components/error-store";
import { mapMutations, mapActions, mapGetters } from "vuex";
import { getNewTagList, routeEqual } from "@/libs/util";
import routers from "@/router/routers";
import minLogo from "@/assets/images/logo_mini.jpg";
import maxLogo from "@/assets/images/pnglogo.png";
import axios from "axios";
import "./main.less";
export default {
  name: "Main",
  components: {
    SideMenu,
    HeaderBar,
    Language,
    TagsNav,
    Fullscreen,
    ErrorStore,
    User,
    ABackTop,
  },
  data() {
    return {
      messageUnreadCount: 0,
      alarmcount: 0,
      collapsed: false,
      minLogo,
      maxLogo,
      isFullscreen: false,
      userinfo: {},
      recive_id: 0,
      recive_type: 0,
      timer: null,
      bjmp3: "",
    };
  },
  computed: {
    ...mapGetters(["errorCount"]),
    tagNavList() {
      return this.$store.state.app.tagNavList;
    },
    tagRouter() {
      return this.$store.state.app.tagRouter;
    },
    userAvatar() {
      return this.$store.state.user.avatarImgPath;
    },
    cacheList() {
      const list = [
        "ParentView",
        ...(this.tagNavList.length
          ? this.tagNavList
              .filter((item) => !(item.meta && item.meta.notCache))
              .map((item) => item.name)
          : []),
      ];
      return list;
    },
    menuList() {
      if (JSON.parse(sessionStorage.getItem("comco_userInfo")) != null) {
        return JSON.parse(sessionStorage.getItem("comco_userInfo")).menulist;
      }

      // return this.$store.getters.menuList
    },
    local() {
      return this.$store.state.app.local;
    },
    hasReadErrorPage() {
      return this.$store.state.app.hasReadErrorPage;
    },
    unreadCount() {
      return this.$store.state.user.unreadCount;
    },
  },
  methods: {
    ...mapMutations([
      "setBreadCrumb",
      "setTagNavList",
      "addTag",
      "setLocal",
      "setHomeRoute",
      "detailsPageClear",
      "detailsPage",
      "closeTag",
    ]),
    ...mapActions(["handleLogin", "getUnreadMessageCount"]),
    // 		ClearDetailsPage(){
    // 			this.detailsPageClear('');//设置vuex里的页面详情状态值
    // 		},
    turnToPage(route) {
      this.detailsPageClear(""); //设置vuex里的页面详情状态值
      let { name, params, query } = {};
      if (typeof route === "string") name = route;
      else {
        name = route.name;
        params = route.params;
        query = route.query;
      }
      if (name.indexOf("isTurnByHref_") > -1) {
        window.open(name.split("_")[1]);
        return;
      }
      this.$router.push({
        name,
        params,
        query,
      });
    },
    handleCollapsedChange(state) {
      this.collapsed = state;
    },
    handleCloseTag(res, type, route) {
      if (type !== "others") {
        if (type === "all") {
          this.turnToPage(this.$config.homeName);
        } else {
          if (routeEqual(this.$route, route)) {
            this.closeTag(route);
          }
        }
      }
      this.setTagNavList(res);
    },
    handleClick(item) {
      this.turnToPage(item);
      if (item.meta.title == "每周菜单-详情设置") {
        this.detailsPage("菜单管理"); //设置vuex里的页面详情状态值
      }
    },
    alarm() {
      this.$router.push({
        path: "/attendance_manage/service_alarm",
      });
    },
    message() {
      this.$router.push({
        path: "/community_service/message_page",
      });
    },
  },
  watch: {
    $route(newRoute) {
      const { name, query, params, meta } = newRoute;
      this.addTag({
        route: { name, query, params, meta },
        type: "push",
      });
      this.setBreadCrumb(newRoute);
      this.setTagNavList(getNewTagList(this.tagNavList, newRoute));
      this.$refs.sideMenu.updateOpenName(newRoute.name);
    },
  },
  mounted() {
    /**
     * @description 初始化设置面包屑导航和标签导航
     */
    this.setTagNavList();
    this.setHomeRoute(routers);
    const { name, params, query, meta } = this.$route;
    this.addTag({
      route: { name, params, query, meta },
    });
    this.setBreadCrumb(this.$route);
    // 设置初始语言
    this.setLocal(this.$i18n.locale);
    // 如果当前打开页面不在标签栏中，跳到homeName页
    if (!this.tagNavList.find((item) => item.name === this.$route.name)) {
      this.$router.push({
        name: this.$config.homeName,
      });
    }
    // 获取未读消息条数
    // this.getUnreadMessageCount()
  },
  distroyed: function () {
    clearInterval(this.timer);
    this.timer = null;
  },
  created() {
    this.userinfo = this.$global.getUserinfo();
  },
};
</script>
