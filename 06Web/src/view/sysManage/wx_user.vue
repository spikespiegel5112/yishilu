<template>
  <div>
    <Row class="margin_bottom_15">
       <!-- <Col span="6">
        <Button type="primary" @click="addManagerInfo">新增用户</Button>
      </Col> -->
       <Col span="5">
        <Button type="primary" @click="clearprize">清空抽奖记录</Button>
      </Col>
      <Col span="8"  offset="11">
        <Input
          search
          enter-button="搜索"
          @on-search="getuserPagedList(1)"
          v-model="searchkey"
          placeholder="手机号、姓名、微信昵称"
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
          v-if="data[index].prize_State==1&&data[index].f_Prize==1"
        >核销</Button>
      </template>
    </Table>
     <Modal :styles="{width:'50%'}" v-model="isModal" :title="modalTitle" :mask-closable="false">
      <Form ref="formValidate" :model="currentItemInfo" :rules="ruleValidate" :label-width="90">
        <Row>
          <Col span="12">
            <FormItem label="姓名:" prop="Name">
              <Input v-model="currentItemInfo.name" placeholder="请输入姓名"></Input>
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="24">
           <FormItem label="手机号:" prop="Phone">
              <Input v-model="currentItemInfo.phone" placeholder="请输入手机号"></Input>
            </FormItem>
          </Col>
        </Row>
      </Form>
      <div slot="footer">
        <Button @click="cancel">取消</Button>
        <Button type="primary" @click="submitData" >确定</Button>
      </div>
    </Modal>
    <div>
      <Page
        :total="total"
        @on-page-size-change="pageSizeChange"
        @on-change="getuserPagedList"
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
      userinfo: {},
       pagesize: 10,
         total: 0,
         searchkey:"",
         tableLoading:false,
         currentPageIndex:1,
           isModal: false,
      modalTitle: "",
      opType: "",
      currentItemInfo:{},
      columns: [
        {
          title: "头像",
          key: "avatar",
          align: "center",
          render: (h, params) => {
            return h("img", {
              attrs: {
                src:
                  params.row.avatar ||
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
                    params.row.avatar ||
                      "https://www.m12315.com/Content/CodeSearchImg/icon_tx_u653.svg"
                  );
                }
              }
            });
          }
        },
        {
          title: "昵称",
          key: "nickName",
          align: "center",
        },
        {
          title: "openid",
          key: "openid",
          align: "center"
        },
        {
          title: "手机号",
          key: "phone",
          align: "center"
        },
        {
          title: "姓名",
          key: "name",
          align: "center"
        },
        {
          title: "首页扫码次数",
          key: "home_ScanCount",
          align: "center"
        },
        {
          title: "眼扫码次数",
          key: "eye_ScanCount",
          align: "center"
        },
        {
          title: "视扫码次数",
          key: "regard_ScanCount",
          align: "center"
        },
        {
          title: "光扫码次数",
          key: "light_ScanCount",
          align: "center"
        },
         {
          title: "是否中奖",
          key: "f_Prize",
          align: "center",
           render: (h, params) => {
            return h("span", params.row.f_Prize == 0 ? "未参与" : "已中奖");
          }
        },
         {
          title: "奖项",
          key: "prize_Name",
          align: "center"
        },
         {
          title: "中奖时间",
          key: "prize_Time",
          align: "center"
        },
         {
          title: "是否核销",
          key: "prize_State",
          align: "center",
           render: (h, params) => {
            return h("span", params.row.prize_State == 1? "未核销" : "已核销");
          }
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
        name: [
          {
            required: true,
            validator: this.$global.name,
            trigger: "blur"
          },
        ],
        phone: [
          {
            required: true,
            validator: this.$global.tel,
            trigger: "blur"
          }
        ]
      }
    };
  },
  methods: {
      modifyManagerInfo(minfo) {
        var _this=this;
      this.currentItemInfo = this.$copy(this.data[minfo]);
      this.$Modal.confirm({
        title: "系统提示",
        content:
          ( "确认核销[") +
          "<b style='color:red'>" +
          _this.currentItemInfo.prize_Name +
          "</b>]",
        onOk: function() {
          _this.$axios
            .get("change.wxuser.prizestate",  {params: {u_id: _this.currentItemInfo.id}})
            .then(p => {
              if (p.data.state == 200) {
                _this.$Message.success("核销成功！");
                _this.getuserPagedList(_this.currentPageIndex);
              } else {
                _this.$Message.warning(p.data.msg);
              }
            });
        }
      });  

    },
     addManagerInfo() {
      this.modalTitle = "用户新增";
      this.opType = "Add";
      this.currentItemInfo = {};
      this.$refs["formValidate"].resetFields();
      this.isModal = !this.isModal;
    },
    submitData(){
      var _this=this;
      this.$refs["formValidate"].validate(valid => {
        if (valid) {
          _this.$axios
            .post("add.update.wxuser", _this.currentItemInfo)
            .then(p => {
              if (p.data.state == 200) {
                _this.isModal = !_this.isModal;
                _this.$Message.success("操作成功！");
                _this.getuserPagedList(_this.currentPageIndex);
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
    //获取
    pageSizeChange(pagesize) {
      this.pagesize = pagesize || 10;
      this.getuserPagedList(1);
    },
    clearprize(){
      var _this=this;
       this.$axios
        .get("clear.wxuser.prizestate", {
          params: {
          }
        })
        .then(p => {
            if (p.data.state == 200) {
               _this.getuserPagedList(_this.currentPageIndex);
              _this.$Message.success("清空成功！");
            }
            else{
              _this.$Message.error("清空失败！");
            }
        });
    },
    getuserPagedList(pageindex) {
      this.data = [];
      this.currentPageIndex = pageindex || 1;
      this.$axios
        .get("get.wxuser.pagelist", {
          params: {
            searchkey:this.searchkey,
            PageIndex: this.currentPageIndex,
            PageSize: this.pagesize
          }
        })
        .then(p => {
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
     cancel() {
      this.isModal = false;
    }
  },
  created() {
    this.userinfo = this.$global.getUserinfo();
    this.getuserPagedList();
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
