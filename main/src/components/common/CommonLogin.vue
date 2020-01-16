<template>
  <div class="login_container">
    <div class="login_wrapper">
      <ul>
        <!--<li>-->
        <!--<span>昵称</span>-->
        <!--<input v-model="formData.nickName" type="text" placeholder="请输入您的昵称"/>-->
        <!--</li>-->
        <li v-for="item in formDictionary">
          <span>{{item.title}}</span>
          <input v-model="formData[item.modelName]" type="text" :placeholder="item.placeholder"/>
        </li>
      </ul>
      <p class="hint">
        该号码仅用于大片作者信息
      </p>
      <a class="confirm" @click="confirm"></a>
    </div>
  </div>
</template>

<script>
  export default {
    name: "CommonLogin",
    data() {
      return {
        formData: {
          nickName: '',
          phoneNumber: ''
        },
        formDictionary: [{
          // title: '昵称',
          // modelName: 'nickName'
          // placeholder: '请输入昵称'
        // }, {
          title: '手机号',
          modelName: 'phoneNumber',
          placeholder: '请输入手机号'
        }]
      }
    },
    mounted() {
      this.$autoHeight({
        target: '.login_container',
        force: true
      });
    },
    methods: {
      confirm() {
        this.validator().then(() => {
          this.$emit('confirm', this.formData)
        }).catch(error => {
          console.log(error)
        })
      },
      validator() {
        let that = this;
        return new Promise((resolve, reject) => {
          let result = true;
          this.formDictionary.forEach((item, index) => {
            if (this.$isEmpty(this.formData[item.modelName]) && result) {
              this.$vux.confirm.show({
                showCancelButton: false,
                title: '请输入' + item.title,
                onConfirm() {
                }
              });
              result = false;
            }
          });
          result ? resolve() : reject('验证失败');
        })

      }
    }
  }
</script>

<style scoped>

</style>
