<template>
  <Card style="width: 50%; margin: 0 auto">
    <Form
      :model="formItem"
      ref="formValidate"
      :rules="ruleValidate"
      :label-width="200"
    >
      <FormItem label="主图1:" class="uploadList">
        <div
          class="demo-upload-list"
          v-show="this.ImgStatus_Main_1 === 'finished'"
        >
          <img :src="formItem.main_1" style="width: 50px; height: 50px" />
          <div class="demo-upload-list-cover">
            <Icon
              type="ios-eye-outline"
              @click.native="handleView(formItem.main_1)"
            ></Icon>
          </div>
        </div>
        <Upload
          ref="upload"
          :show-upload-list="false"
          :on-success="ImgSuccess_Main_1"
          :format="['jpg', 'jpeg', 'bmp', 'png']"
          :max-size="10240"
          :on-format-error="handleFormatError"
          :on-exceeded-size="handleMaxSize"
          type="drag"
          action="https://jz.ah-rgkj.com/api/upload.file"
          style="display: inline-block; width: 58px"
          v-model="formItem.main_1"
        >
          <div style="width: 58px; height: 58px; line-height: 58px">
            <Icon type="ios-camera" size="20"></Icon>
          </div>
        </Upload>
      </FormItem>
      <FormItem label="主图2:" class="uploadList">
        <div
          class="demo-upload-list"
          v-show="this.ImgStatus_Main_2 === 'finished'"
        >
          <img :src="formItem.main_2" style="width: 50px; height: 50px" />
          <div class="demo-upload-list-cover">
            <Icon
              type="ios-eye-outline"
              @click.native="handleView(formItem.main_2)"
            ></Icon>
          </div>
        </div>
        <Upload
          ref="upload"
          :show-upload-list="false"
          :on-success="ImgSuccess_Main_2"
          :format="['jpg', 'jpeg', 'bmp', 'png']"
          :max-size="10240"
          :on-format-error="handleFormatError"
          :on-exceeded-size="handleMaxSize"
          type="drag"
          action="https://jz.ah-rgkj.com/api/upload.file"
          style="display: inline-block; width: 58px"
          v-model="formItem.main_2"
        >
          <div style="width: 58px; height: 58px; line-height: 58px">
            <Icon type="ios-camera" size="20"></Icon>
          </div>
        </Upload>
      </FormItem>
      <FormItem label="主图3:" class="uploadList">
        <div
          class="demo-upload-list"
          v-show="this.ImgStatus_Main_3 === 'finished'"
        >
          <img :src="formItem.main_3" style="width: 50px; height: 50px" />
          <div class="demo-upload-list-cover">
            <Icon
              type="ios-eye-outline"
              @click.native="handleView(formItem.main_3)"
            ></Icon>
          </div>
        </div>
        <Upload
          ref="upload"
          :show-upload-list="false"
          :on-success="ImgSuccess_Main_3"
          :format="['jpg', 'jpeg', 'bmp', 'png']"
          :max-size="10240"
          :on-format-error="handleFormatError"
          :on-exceeded-size="handleMaxSize"
          type="drag"
          action="https://jz.ah-rgkj.com/api/upload.file"
          style="display: inline-block; width: 58px"
          v-model="formItem.main_3"
        >
          <div style="width: 58px; height: 58px; line-height: 58px">
            <Icon type="ios-camera" size="20"></Icon>
          </div>
        </Upload>
      </FormItem>
      <FormItem label="中奖图:" class="uploadList">
        <div
          class="demo-upload-list"
          v-show="this.ImgStatus_Draw_Img === 'finished'"
        >
          <img :src="formItem.draw_Img" style="width: 50px; height: 50px" />
          <div class="demo-upload-list-cover">
            <Icon
              type="ios-eye-outline"
              @click.native="handleView(formItem.draw_Img)"
            ></Icon>
          </div>
        </div>
        <Upload
          ref="upload"
          :show-upload-list="false"
          :on-success="ImgSuccess_Draw_Img"
          :format="['jpg', 'jpeg', 'bmp', 'png']"
          :max-size="10240"
          :on-format-error="handleFormatError"
          :on-exceeded-size="handleMaxSize"
          type="drag"
          action="https://jz.ah-rgkj.com/api/upload.file"
          style="display: inline-block; width: 58px"
          v-model="formItem.draw_Img"
        >
          <div style="width: 58px; height: 58px; line-height: 58px">
            <Icon type="ios-camera" size="20"></Icon>
          </div>
        </Upload>
      </FormItem>
      <FormItem label="评选图:" class="uploadList">
        <div
          class="demo-upload-list"
          v-show="this.ImgStatus_Selection_Img === 'finished'"
        >
          <img
            :src="formItem.selection_Img"
            style="width: 50px; height: 50px"
          />
          <div class="demo-upload-list-cover">
            <Icon
              type="ios-eye-outline"
              @click.native="handleView(formItem.selection_Img)"
            ></Icon>
          </div>
        </div>
        <Upload
          ref="upload"
          :show-upload-list="false"
          :on-success="ImgSuccess_Selection_Img"
          :format="['jpg', 'jpeg', 'bmp', 'png']"
          :max-size="10240"
          :on-format-error="handleFormatError"
          :on-exceeded-size="handleMaxSize"
          type="drag"
          action="https://jz.ah-rgkj.com/api/upload.file"
          style="display: inline-block; width: 58px"
          v-model="formItem.selection_Img"
        >
          <div style="width: 58px; height: 58px; line-height: 58px">
            <Icon type="ios-camera" size="20"></Icon>
          </div>
        </Upload>
      </FormItem>
      <FormItem label="尾图" class="uploadList">
        <div
          class="demo-upload-list"
          v-show="this.ImgStatus_Last_Img === 'finished'"
        >
          <img :src="formItem.last_Img" style="width: 50px; height: 50px" />
          <div class="demo-upload-list-cover">
            <Icon
              type="ios-eye-outline"
              @click.native="handleView(formItem.last_Img)"
            ></Icon>
          </div>
        </div>
        <Upload
          ref="upload"
          :show-upload-list="false"
          :on-success="ImgSuccess_Last_Img"
          :format="['jpg', 'jpeg', 'bmp', 'png']"
          :max-size="10240"
          :on-format-error="handleFormatError"
          :on-exceeded-size="handleMaxSize"
          type="drag"
          action="https://jz.ah-rgkj.com/api/upload.file"
          style="display: inline-block; width: 58px"
          v-model="formItem.last_Img"
        >
          <div style="width: 58px; height: 58px; line-height: 58px">
            <Icon type="ios-camera" size="20"></Icon>
          </div>
        </Upload>
      </FormItem>
      <FormItem label="红包金额" prop="red_Envelopes">
        <Input
          style="width: 50%"
          :number="true"
          v-model="formItem.red_Envelopes"
          placeholder="请输入红包金额"
        ></Input
        >元
      </FormItem>
      <FormItem>
        <Button type="primary" @click="sendparameter">保存</Button>
      </FormItem>
    </Form>
    <Modal title="查看大图" v-model="visible">
      <img :src="imgName" v-if="visible" style="width: 100%" />
    </Modal>
  </Card>
</template>
<script>
export default {
  data() {
    return {
      userinfo: {},
      ImgStatus_Main_1: "",
      ImgStatus_Main_2: "",
      ImgStatus_Main_3: "",
      ImgStatus_Draw_Img: "",
      ImgStatus_Selection_Img: "",
      ImgStatus_Last_Img: "",
      imgName: "",
      visible: false,
      formItem: {},
      ruleValidate: {},
    };
  },
  methods: {
    handleView(name) {
      this.imgName = name;
      this.visible = true;
    },
    ImgSuccess_Main_1(res) {
      var _this = this;
      _this.formItem.main_1 = res.data;
      _this.ImgStatus_Main_1 = "finished";
    },
    ImgSuccess_Main_2(res) {
      var _this = this;
      _this.formItem.main_2 = res.data;
      _this.ImgStatus_Main_2 = "finished";
    },
    ImgSuccess_Main_3(res) {
      var _this = this;
      _this.formItem.main_3 = res.data;
      _this.ImgStatus_Main_3 = "finished";
    },
    ImgSuccess_Draw_Img(res) {
      var _this = this;
      _this.formItem.draw_Img = res.data;
      _this.ImgStatus_Draw_Img = "finished";
    },
    ImgSuccess_Selection_Img(res) {
      var _this = this;
      _this.formItem.selection_Img = res.data;
      _this.ImgStatus_Selection_Img = "finished";
    },
    ImgSuccess_Last_Img(res) {
      var _this = this;
      _this.formItem.last_Img = res.data;
      _this.ImgStatus_Last_Img = "finished";
    },
    handleFormatError(file) {
      this.$Notice.warning({
        title: "请检查文件格式",
        desc:
          "File format of " +
          file.name +
          " is incorrect, please select jpg or png.",
      });
    },
    handleMaxSize(file) {
      this.$Notice.warning({
        title: "请检查文件大小,不能大于10M",
        desc: "File  " + file.name + " is too large, no more than 10M.",
      });
    },
    sendparameter() {
      var _this = this;
      _this.$axios.post("add.update.setting", _this.formItem).then((p) => {
        if (p.data.state == 200) {
          _this.$Message.success("保存成功");
        } else {
          _this.$Message.warning(p.data.msg);
        }
      });
    },
    getbraceletDeviceparameter() {
      var _this = this;
      this.$axios
        .get("get.sys.setting", {
          params: {},
        })
        .then((p) => {
          if (p.data.data) {
            _this.formItem = p.data.data;
            if (_this.formItem.main_1) {
              _this.ImgStatus_Main_1 = "finished";
            }
            if (_this.formItem.main_2) {
              _this.ImgStatus_Main_2 = "finished";
            }
            if (_this.formItem.main_3) {
              _this.ImgStatus_Main_3 = "finished";
            }
            if (_this.formItem.draw_Img) {
              _this.ImgStatus_Draw_Img = "finished";
            }
            if (_this.formItem.selection_Img) {
              _this.ImgStatus_Selection_Img = "finished";
            }
            if (_this.formItem.last_Img) {
              _this.ImgStatus_Last_Img = "finished";
            }
          } else {
          }
        });
    },
  },
  created() {
    this.userinfo = this.$global.getUserinfo();
    this.getbraceletDeviceparameter();
  },
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
