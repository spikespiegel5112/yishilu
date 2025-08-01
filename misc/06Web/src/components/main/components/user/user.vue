<template>
  <div class="user-avatar-dropdown">
    <Dropdown @on-click="handleClick">
      <Badge :dot="!!messageUnreadCount">
        <Avatar :src="userAvatar" />
      </Badge>
      <Icon :size="18" type="md-arrow-dropdown"></Icon>
      <DropdownMenu slot="list">
        <DropdownItem>{{ userinfo.userName }}</DropdownItem>
        <!-- <DropdownItem name="messageCenter">
          消息中心
          <Badge style="margin-left: 10px" :count="messageUnreadCount"></Badge>
        </DropdownItem> -->
        <DropdownItem name="logout">退出登录</DropdownItem>
      </DropdownMenu>
    </Dropdown>
    <audio ref="music" id="audio" loop>
      <source src="./9833.mp3" type="audio/mpeg" />
    </audio>
    <audio ref="music_notice" id="audio" loop>
      <source src="./2478.mp3" type="audio/mpeg" />
    </audio>
  </div>
</template>

<script>
import "./user.less";
import { mapActions } from "vuex";
export default {
  name: "User",
  data() {
    return {
      userinfo: {},
      bjmp3: "",
      recive_id: 0,
      recive_type: 0,
      timer: null,
      messageUnreadCount: 0,
      audio: {},
      tiemr: null,
      audiosrc: "./9833.mp3",
    };
  },
  props: {
    userAvatar: {
      type: String,
      default: "",
    },
  },
  methods: {
    ...mapActions(["handleLogOut"]),
    logout() {
      this.handleLogOut().then(() => {
        this.$router.push({
          name: "login",
        });
      });
    },
    handleClick(name) {
      switch (name) {
        case "logout":
          this.logout();
          break;
        case "messageCenter":
          this.message();
          break;
      }
    },
  },
  created() {
    this.userinfo = this.$global.getUserinfo();
  },
};
</script>
