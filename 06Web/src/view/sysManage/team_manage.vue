<template>
  <div>
    <Row class="margin_bottom_15">
      <Col span="12">
        <Button type="primary" @click="addManagerInfo">新建战队</Button>
      </Col>
      <Col span="5" offset="7">
        <Input
          search
          enter-button="搜索"
          @on-search="getteamgedList(1)"
          v-model="searchkey"
          placeholder="战队名称"
        ></Input>
      </Col>
    </Row>
    <Table class="margin_bottom_15" border :columns="columns" :data="data">
      <template slot-scope="{ row }" slot="name">
        <strong>{{ row.name }}</strong>
      </template>
      <template slot-scope="{ row, index }" slot="action">
        <Button
          type="primary"
          size="small"
          style="margin-right: 5px"
          @click="modifyManagerInfo(index)"
        >编辑</Button>
        <Button type="error" style="margin-right: 5px" size="small" @click="delacount(index)">删除</Button>
        <Button
          type="warning"
          style="margin-right: 5px"
          size="small"
          @click="setAccountEnable(index)"
          v-if="data[index].f_Disable==0"
        >禁用</Button>
        <Button
          type="success"
          style="margin-right: 5px"
          size="small"
          @click="setAccountEnable(index)"
          v-else
        >启用</Button>
      </template>
    </Table>
    <Modal :styles="{width:'50%'}" v-model="isModal" :title="modalTitle" :mask-closable="false">
      <Form ref="formValidate" :model="currentItemInfo" :rules="ruleValidate" :label-width="90">
        <Row>
          <Col span="12">
            <FormItem label="战队名称:" prop="teamName">
              <Input v-model="currentItemInfo.teamName" placeholder="请输入战队名称"></Input>
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="12">
           <FormItem label="战队类型:" prop="team_Type">
              <Select placeholder="请选择战队类型" v-model="currentItemInfo.team_Type">
                 <Option  key="1" value="1">着装队</Option>
                  <Option  key="2" value="2">表演队</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="12">
            <FormItem label="战队封面:" prop="backgroundImg" class="uploadList">
              <div class="demo-upload-list" v-show="this.ImgStatus === 'finished'">
                <img :src="currentItemInfo.backgroundImg" style="width:50px;height:50px" />
                <div class="demo-upload-list-cover">
                  <Icon
                    type="ios-eye-outline"
                    @click.native="handleView(currentItemInfo.backgroundImg)"
                  ></Icon>
                </div>
              </div>
              <Upload
                ref="upload"
                :show-upload-list="false"
                :on-success="ImgSuccess"
                :format="['jpg','jpeg','bmp','png']"
                :max-size="10240"
                :on-format-error="handleFormatError"
                :on-exceeded-size="handleMaxSize"
                type="drag"
                action="https://jz.ah-rgkj.com/api/upload.file"
                style="display: inline-block;width:58px;"
                v-model="currentItemInfo.backgroundImg"
              >
                <div style="width: 58px;height:58px;line-height: 58px;">
                  <Icon type="ios-camera" size="20"></Icon>
                </div>
              </Upload>
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="24">
            <FormItem label="战队描述:" prop="Description">
              <Input
                type="textarea"
                :rows="4"
                v-model="currentItemInfo.description"
                placeholder="请输入战队描述"
              ></Input>
            </FormItem>
          </Col>
        </Row>
      </Form>
      <div slot="footer">
        <Button @click="cancel">取消</Button>
        <Button type="primary" @click="submitData" :disabled="isDisable">确定</Button>
      </div>
    </Modal>
    <Modal title="查看大图" v-model="visible">
      <img :src="imgName" v-if="visible" style="width: 100%" />
    </Modal>
    <div>
      <Page
        :total="total"
        @on-page-size-change="pageSizeChange"
        @on-change="getteamgedList"
        show-sizer
        show-elevator
        v-if="tableLoading==false"
      ></Page>
    </div>
  </div>
</template>

<script>
export default {
  name: "manager-index",
  data() {
    return {
      searchkey: "",
      tableLoading: true,
      isModal: false,
      modalTitle: "",
      total: 0,
      pagesize: 10,
      model10: [],
      visible: false,
      imgName: "",
      judicialList: [],
      judicialList2: [],
      judicial_id: 0,
      ImgStatus: "",
      currentPageIndex: 1,
      selecttypelist: [],
      currentItemInfo: {
        username: "",
        userpwd: "",
        rid: "",
        remark: "",
        roomid: 0
      },
      opType: "",
      userinfo: {},
      formItem: {
        remark: ""
      },
      isDisable: false,
      columns: [
        {
          title: "图片",
          key: "backgroundImg",
          fixed: "left",
          align: "center",
          render: (h, params) => {
            return h("img", {
              attrs: {
                src:
                  params.row.backgroundImg ||
                  "https://www.m12315.com/Content/CodeSearchImg/icon_tx_u653.svg",
                class: "img-thumbnail"
              },
              style: {
                maxWidth: "50px",
                margin: "5px 0",
                verticalAlign: "middle",
                cursor: "Position"
              },
              on: {
                click: () => {
                  this.handleView(
                    params.row.backgroundImg ||
                      "https://www.m12315.com/Content/CodeSearchImg/icon_tx_u653.svg"
                  );
                }
              }
            });
          }
        },
        {
          title: "战队名称",
          key: "teamName",
          align: "center",
          fixed: "left"
        },
        {
          title: "战队类型",
          key: "team_Type",
          align: "center",
           render: (h, params) => {
            return h("span", this.getteamtype(params.row.team_Type));
          }
        },
        {
          title: "战队描述",
          key: "description",
          align: "center"
        },
        {
          title: "总投票数",
          key: "votingTotal",
          align: "center"
        },
        {
          title: "创建时间",
          key: "createTime",
          align: "center"
        },
        {
          title: "操作",
          align: "center",
          fixed: "right",
          slot: "action"
        }
      ],
      data: [],
      ruleValidate: {
        TeamName: [
          {
            required: true,
            validator: this.$global.isempty,
            trigger: "blur"
          },
        ]
      }
    };
  },
  methods: {
    //获取
    pageSizeChange(pagesize) {
      this.pagesize = pagesize || 10;
      this.getteamgedList(1);
    },
    getteamgedList(pageindex) {
      this.data = [];
      this.currentPageIndex = pageindex || 1;
      this.$axios
        .get("get/team/pagelist", {
          params: {
            searchkey:this.searchkey,
            pageindex: this.currentPageIndex,
            pagesize: this.pagesize
          }
        })
        .then(p => {
            console.log(p.data.data.items)
          if (p.data.data.items) {
            this.total = p.data.data.count;
            if (this.total == 0) {
              this.tableLoading = true;
            } else {
              this.data = p.data.data.items;
              this.tableLoading = false;
            }
          }
        });
    },
    handleView(name) {
      this.imgName = name;
      this.visible = true;
    },
    setAccountEnable(iteminfo) {
      this.currentItemInfo = this.$copy(this.data[iteminfo]);
      var _this = this;
      this.$Modal.confirm({
        title: "系统提示",
        content:
          (_this.currentItemInfo.f_Disable == 0 ? "确认禁用[" : "确认启用[") +
          "<b style='color:red'>" +
          _this.currentItemInfo.teamName +
          "</b>]",
        onOk: function() {
            _this.currentItemInfo.f_Disable=_this.currentItemInfo.f_Disable == 0?1:0;
          _this.$axios
            .post("add.update.team", _this.currentItemInfo)
            .then(p => {
              if (p.data.state == 200) {
                _this.$Message.success("操作成功！");
                _this.getteamgedList(_this.currentPageIndex);
              } else {
                _this.$Message.warning(p.data.Msg);
              }
            });
        }
      });
    },
    delacount(index) {
      this.currentItemInfo = this.$copy(this.data[index]);
      var _this = this;
      this.$Modal.confirm({
        title: "系统提示",
        content:
          "是否确定删除[<b style='color:red'>" +
          _this.currentItemInfo.teamName +
          "</b>]",
        onOk: function() {
            _this.currentItemInfo.f_Del=1;
          _this.$axios
            .post("add.update.team", _this.currentItemInfo,
            )
            .then(p => {
              if (p.data.state == 200) {
                _this.$Message.success("操作成功！");
                _this.getteamgedList(_this.currentPageIndex);
              } else {
                _this.$Message.warning(p.data.data);
              }
            });
        }
      });
    },
    ImgSuccess(res) {
      var _this = this;
      _this.currentItemInfo.backgroundImg = res.data;
      _this.ImgStatus = "finished";
    },
    handleFormatError(file) {
      this.$Notice.warning({
        title: "请检查文件格式",
        desc:
          "File format of " +
          file.name +
          " is incorrect, please select jpg or png."
      });
    },
    handleMaxSize(file) {
      this.$Notice.warning({
        title: "请检查文件大小,不能大于10M",
        desc: "File  " + file.name + " is too large, no more than 10M."
      });
    },
    getteamtype(obj) {
      var str = "";
      switch (obj) {
        case 0:
          str = "全部";
          break;
        case 1:
          str = "着装队";
          break;
        case 2:
          str = "表演队";
          break;
        default:
          break;
      }
      return str;
    },
    modifyManagerInfo(minfo) {
      this.modalTitle = "战队编辑";
      this.opType = "Edit";
      this.$refs["formValidate"].resetFields();
      this.currentItemInfo = this.$copy(this.data[minfo]);
      console.log( this.currentItemInfo)
      if (this.currentItemInfo.backgroundImg) {
        this.ImgStatus = "finished";
      }
       this.currentItemInfo.team_Type=this.currentItemInfo.team_Type+"";
      this.model10 = [];
      //密码随意给个值
      this.isModal = !this.isModal;
    },
    addManagerInfo() {
      this.modalTitle = "战队新增";
      this.opType = "Add";
      this.model10 = [];
      this.currentItemInfo = {};
      this.currentItemInfo.team_Type="1";
      this.$refs["formValidate"].resetFields();
      this.ImgStatus = "";
      this.isModal = !this.isModal;
    },
    submitData() {
      var _this = this;
      this.isDisable = true;
      setTimeout(() => {
        this.isDisable = false;
      }, 1000);
      this.$refs["formValidate"].validate(valid => {
        if (valid) {
          _this.$axios
            .post("add.update.team", _this.currentItemInfo)
            .then(p => {
              if (p.data.state == 200) {
                _this.isModal = !_this.isModal;
                _this.$Message.success("操作成功！");
                _this.getteamgedList(_this.currentPageIndex);
              } else {
                _this.isModal = !_this.isModal;
                _this.$Message.warning(p.data.msg);
              }
            });
        } else {
          this.isModal = true;
          this.$Message.error("请检查数据格式");
        }
      });
    },
    cancel() {
      this.isModal = false;
    }
  },
  created() {
    this.userinfo = this.$global.getUserinfo();
    this.judicial_id = this.userinfo.source_id;
    this.getteamgedList();
  }
};
</script>
<style scoped>
.demo-upload-list {
  display: inline-block;
  width: 60px;
  height: 60px;
  text-align: center;
  line-height: 60px;
  border: 1px solid transparent;
  border-radius: 4px;
  overflow: hidden;
  background: #fff;
  position: relative;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
  margin-right: 4px;
}

.demo-upload-list img {
  width: 100%;
  height: 100%;
}

.demo-upload-list-cover {
  display: none;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.6);
}

.demo-upload-list:hover .demo-upload-list-cover {
  display: block;
}

.demo-upload-list-cover i {
  color: #fff;
  font-size: 20px;
  cursor: pointer;
  margin: 0 2px;
}
</style>
