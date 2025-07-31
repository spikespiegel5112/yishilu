<template>
  <Card shadow>
    <Row class="margin_bottom_15">
      <Col span="3">
        <Select
          placeholder="消息类型"
          :transfer="true"
          v-model="push_type"
          @on-change="changeartype()"
        >
          <Option key="0" value="0">全部</Option>
          <Option key="5" value="5">违规通知</Option>
          <Option key="6" value="6">待审批通知</Option>
        </Select>
      </Col>
      <Col span="3" style="padding-left: 25px">
        <Select
          :transfer="true"
          v-model="push_type_child"
          @on-change="getsysmsgpagelist(1)"
        >
          <Option
            v-for="item in pointtypelist"
            :key="item.key"
            :value="item.key"
            >{{ item.value }}</Option
          >
        </Select>
      </Col>
      <Col span="3" style="padding-left: 25px">
        <Button type="success" size="small" @click="allread()"
          >全部标记为已读</Button
        >
      </Col>
    </Row>
    <div>
      <div class="message-page-con message-category-con">
        <Menu width="auto" active-name="unread" @on-select="handleSelect">
          <MenuItem name="unread">
            <span class="category-title">未读消息</span
            ><Badge
              style="margin-left: 10px"
              :count="messageUnreadCount"
            ></Badge>
          </MenuItem>
          <MenuItem name="readed">
            <span class="category-title">已读消息</span
            ><Badge style="margin-left: 10px" class-name="gray-dadge"></Badge>
          </MenuItem>
          <!-- <MenuItem name="trash">
            <span class="category-title">回收站</span><Badge style="margin-left: 10px" class-name="gray-dadge" :count="messageTrashCount"></Badge>
          </MenuItem> -->
        </Menu>
      </div>
      <div class="message-page-con message-list-con" style="height: 100%">
        <Spin fix v-if="listLoading" size="large"></Spin>
        <Menu
          width="auto"
          active-name=""
          :class="titleClass"
          @on-select="handleView"
        >
          <MenuItem
            v-for="item in messageList"
            :name="item.id"
            :key="`msg_${item.id}`"
          >
            <Badge
              v-if="recive_type == 1"
              dot
              :count="item.f_judicial_pc_read == 0 ? 1 : 0"
            >
              <div>
                <p
                  class="msg-title"
                  style="
                    width: 200px;
                    overflow: hidden;
                    white-space: nowrap;
                    text-overflow: ellipsis;
                  "
                >
                  {{ item.p_name }}:{{ item.push_content }}
                </p>
                <Badge status="default" :text="item.create_date" />
              </div>
            </Badge>
            <Badge
              v-else-if="recive_type == 2"
              dot
              :count="item.f_police_pc_read == 0 ? 1 : 0"
            >
              <div>
                <p
                  class="msg-title"
                  style="
                    width: 200px;
                    overflow: hidden;
                    white-space: nowrap;
                    text-overflow: ellipsis;
                  "
                >
                  {{ item.p_name }}:{{ item.push_content }}
                </p>
                <Badge status="default" :text="item.create_date" />
              </div>
            </Badge>
          </MenuItem>
          <Page
            v-if="totalcount > 0"
            :current="pageindex"
            :total="totalcount"
            simple
            @on-change="getsysmsgpagelist"
          />
          <div v-if="totalcount == 0" style="text-align: center">暂无消息</div>
        </Menu>
      </div>

      <div
        class="message-page-con message-view-con"
        style="text-align: center; top: 20%"
      >
        <Spin fix v-if="contentLoading" size="large"></Spin>
        <div class="message-view-header">
          <h2 class="message-view-title">{{ showingMsgItem.push_msg }}</h2>
          <time class="message-view-time">{{
            showingMsgItem.create_date
          }}</time>
        </div>
        <div v-html="showingMsgItem.push_content"></div>
      </div>
    </div>
  </Card>
</template>

<script>
import { mapState, mapGetters, mapMutations, mapActions } from "vuex";
const listDic = {
  unread: "messageUnreadList",
  readed: "messageReadedList",
  trash: "messageTrashList",
};
export default {
  name: "message_page",
  data() {
    return {
      listLoading: true,
      contentLoading: false,
      currentMessageType: "unread",
      messageContent: "",
      violation_type: 0,
      pointtypelist: [],
      push_type: 0,
      push_type_child: 0,
      showingMsgItem: {},
      messageUnreadCount: 0,
      messageList: [],
      f_read: 0,
      userinfo: {},
      recive_id: 0,
      recive_type: 0,
      pageindex: 1,
      totalcount: 0,
    };
  },
  computed: {
    ...mapState({
      messageUnreadList: (state) => state.user.messageUnreadList,
      messageReadedList: (state) => state.user.messageReadedList,
      messageTrashList: (state) => state.user.messageTrashList,
      // messageList () {
      //   return this[listDic[this.currentMessageType]]
      // },
      titleClass() {
        return {
          "not-unread-list": this.currentMessageType !== "unread",
        };
      },
    }),
    ...mapGetters([]),
  },
  methods: {
    ...mapActions([
      "getContentByMsgId",
      "getMessageList",
      "hasRead",
      "removeReaded",
      "restoreTrash",
    ]),
    stopLoading(name) {
      this[name] = false;
    },
    handleSelect(name) {
      if (name == "readed") {
        this.f_read = 1;
      } else if (name == "unread") {
        this.f_read = 0;
      }
      this.showingMsgItem = {};
      this.getsysmsgpagelist();
      this.currentMessageType = name;
    },
    changeartype() {
      if (this.push_type == 5) {
        this.pointtypelist = [
          { key: 1, value: "超出规定范围" },
          { key: 2, value: "每日未签到违规" },
          { key: 3, value: "周报道未签到违规" },
          { key: 4, value: "集中学习未签到违规" },
          { key: 5, value: "逾期未销假违规" },
          { key: 6, value: "任务未完成" },
        ];
      } else if (this.push_type == 6) {
        this.pointtypelist = [
          { key: 1, value: "请假待审批" },
          { key: 2, value: "就医报告待审批" },
          { key: 3, value: "违规待处理通知" },
        ];
      } else if (this.push_type == 0) {
        this.push_type_child = 0;
        this.pointtypelist = [];
      }
      this.getsysmsgpagelist(1);
    },
    allread() {
      var _this = this;
      this.$Modal.confirm({
        title: "系统提示",
        content: "确定全部标记为已读吗?",
        onOk: function () {
          this.$axios
            .get("update.pc.pushmsg.allread", {
              params: {
                recive_id: _this.recive_id,
                recive_type: _this.recive_type,
                push_type: _this.push_type,
                push_type_child: _this.push_type_child,
              },
            })
            .then((p) => {
              _this.getsysmsgpagelist(1);
            });
        },
      });
    },
    getsysmsgpagelist(pageindex) {
      var _this = this;
      _this.messageList = [];
      this.pageindex = pageindex || 1;
      this.$axios
        .get("pc.get.pushmsg.pagelist", {
          params: {
            f_read: this.f_read,
            recive_id: this.recive_id,
            recive_type: this.recive_type,
            push_type: this.push_type,
            push_type_child: this.push_type_child,
            pagesize: 9,
            pageindex: this.pageindex,
          },
        })
        .then((p) => {
          _this.totalcount = p.data.data.Count;
          if (_this.f_read == 0) {
            _this.messageUnreadCount = p.data.data.Count;
          }
          _this.messageList = p.data.data.data;
          _this.listLoading = false;
        });
    },
    handleView(msg_id) {
      this.contentLoading = false;
      var _this = this;
      const item = this.messageList.find((item) => item.id === msg_id);
      const index = this.messageList.findIndex((item) => item.id === msg_id);
      var body = {
        recive_id: this.recive_id,
        recive_type: this.recive_type,
        ids: msg_id,
      };
      this.$axios.post("update.pc.pushmsg", body).then((p) => {
        if (_this.f_read == 0) {
          _this.messageUnreadCount = _this.messageUnreadCount - 1;
          if (_this.recive_type == 1) {
            _this.messageList[index].f_judicial_pc_read = 1;
          } else if (_this.recive_type == 2) {
            _this.messageList[index].f_police_pc_read = 1;
          }
        }
      });
      if (item) this.showingMsgItem = item;
    },
    removeMsg(item) {
      item.loading = true;
      const msg_id = item.msg_id;
      if (this.currentMessageType === "readed") this.removeReaded({ msg_id });
      else this.restoreTrash({ msg_id });
    },
  },
  mounted() {
    this.listLoading = true;
    this.userinfo = this.$global.getUserinfo();
    this.recive_id = this.userinfo.source_id;
    this.recive_type = this.userinfo.usertype;
    this.getsysmsgpagelist();
    // 请求获取消息列表
    // this.getMessageList().then(() => this.stopLoading('listLoading')).catch(() => this.stopLoading('listLoading'))
  },
};
</script>

<style lang="less">
.message-page {
  &-con {
    height: ~"calc(100vh - 176px)";
    display: inline-block;
    vertical-align: top;
    position: relative;
    &.message-category-con {
      border-right: 1px solid #e6e6e6;
      width: 200px;
    }
    &.message-list-con {
      border-right: 1px solid #e6e6e6;
      width: 230px;
    }
    &.message-view-con {
      position: absolute;
      left: 446px;
      top: 16px;
      right: 16px;
      bottom: 16px;
      overflow: auto;
      padding: 12px 20px 0;
      .message-view-header {
        margin-bottom: 20px;
        .message-view-title {
          display: inline-block;
        }
        .message-view-time {
          margin-left: 20px;
        }
      }
    }
    .category-title {
      display: inline-block;
      width: 65px;
    }
    .gray-dadge {
      background: gainsboro;
    }
    .not-unread-list {
      .msg-title {
        color: rgb(170, 169, 169);
      }
      .ivu-menu-item {
        .ivu-btn.ivu-btn-text.ivu-btn-small.ivu-btn-icon-only {
          display: none;
        }
        &:hover {
          .ivu-btn.ivu-btn-text.ivu-btn-small.ivu-btn-icon-only {
            display: inline-block;
          }
        }
      }
    }
  }
}
</style>
