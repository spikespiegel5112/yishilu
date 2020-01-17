// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import axios from 'axios'
import router from './router'
import store from './store'
import iView from 'view-design'
import i18n from '@/locale'
import config from '@/config'
import importDirective from '@/directive'
import {
  directive as clickOutside
} from 'v-click-outside-x'
import installPlugin from '@/plugin'
import './index.less'
import '@/assets/icons/iconfont.css'
import TreeTable from 'tree-table-vue'
import VOrgTree from 'v-org-tree'
import 'v-org-tree/dist/v-org-tree.css'


// 实际打包时应该不引入mock
/* eslint-disable */
// if (process.env.NODE_ENV !== 'production') require('@/mock')
import Print from 'vue-print-nb'
import printJS from 'print-js'
　　Vue.use(Print);  //注册

import vSelectPage from "v-selectpage";
Vue.use(vSelectPage);

import Viewer from 'v-viewer'

Vue.use(Viewer)
Viewer.setDefaults({
  zIndexInline: 9999
})


Vue.use(iView, {
  i18n: (key, value) => i18n.t(key, value)
})

Vue.use(TreeTable)
Vue.use(VOrgTree)


import VueAMap from 'vue-amap';

Vue.use(VueAMap);



VueAMap.initAMapApiLoader({
  key: 'b5b643abe11f74efc02cadee54bf4c99',
  plugin: ['AMap.Scale', 'AMap.OverView', 'AMap.ToolBar', 'AMap.MapType', 'AMap.Geocoder','AMap.Map','AMap.MarkerClusterer','AMap.DistrictSearch','AMap.CircleEditor','AMap.MouseTool'],
  v: '1.4.15'
});

/**
 * @description 注册admin内置插件
 */
installPlugin(Vue)
/**
 * @description 生产环境关掉提示
 */
Vue.config.productionTip = false
/**
 * @description 全局注册应用配置
 */
Vue.prototype.$config = config
/**
 * 注册指令
 */
importDirective(Vue)
Vue.directive('clickOutside', clickOutside)


import VideoPlayer from 'vue-video-player'
require('vue-video-player/node_modules/video.js/dist/video-js.css')
require('vue-video-player/src/custom-theme.css')
Vue.use(VideoPlayer)





/**
 * @description 全局请求配置
 */
if (process.env.NODE_ENV !== 'production') {
  axios.defaults.baseURL = 'http://192.168.124.8:799/'; //开发
} else {
  axios.defaults.baseURL = 'http://essilorevent.digital-support.cn/api/'; //正式
}
// axios.defaults.headers.common['Authorization'] = 'AUTH_TOKEN';
// axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';
// 添加一个请求拦截器
axios.interceptors.request.use(function (config) {
  if(!config.showloading){
      // Do something before request is sent
  iView.Spin.show({//升级iview3.3后Icon不知为啥渲染不了；所以改用span标签 
    render: (h) => {
      return h('div', [
        h('span', {
          'class': 'ivu-icon ios-loading demo-spin-icon-load',
        }),
        h('div', 'Loading...')
      ])
    }
  });
  }

  //   var token = localStorage.getItem('Authorization')
  //   if (token) {
  //     config.headers['Authorization'] = token;
  //   }


  return config;
}, function (error) {
  // Do something with request error
  return Promise.reject(error);
});
// 添加一个响应拦截器
axios.interceptors.response.use(function (response) {
  // Do something with response data
  iView.Spin.hide();
  return response;
}, function (error) {
  // Do something with response error
  return Promise.reject(error);
});
Vue.prototype.$axios = axios;

//将当前对象，进行深拷贝
Vue.prototype.$copy = function (item) {
  return Object.assign({}, item)
}

//将当前数组，进行深拷贝
Vue.prototype.$copyarray = function (item) {
  return Object.assign([], item)
}

function change (t) {
  if (t < 10) {
    return "0" + t;
  } else {
    return t;
  }
}

Vue.prototype.$dateformat = function (time) {
  var year = time.getFullYear();
  var month = change(time.getMonth() + 1);
  var day = change(time.getDate());
  return year + '-' + month + '-' + day;
}

Vue.prototype.$global = {
  getUserinfo: () => {
    return JSON.parse(sessionStorage.getItem('comco_userInfo'));
  },
  numeric: (rule, value, callback) => {
    // if (!value) {
    //   return callback(new Error("请输入数字"));
    // }
    setTimeout(() => {
      if (!Number.isInteger(value)) {
        callback(new Error('请输入正确的值'));
      } else {
        // if (value < 0) {
        //   callback(new Error('值必须大于0'));
        // } else {
        //   callback();
        // }
        callback();
      }
    }, 100);
  },
  numfloate: (rule, value, callback) => {
    if (!value) {
      return callback(new Error("请输入数字"));
    }
    setTimeout(() => {
      if (value < 0) {
        callback(new Error('值必须大于0'));
      } else {
        callback();
      }
    }, 100);
  },
  tel: (rule, value, callback) => {
    if (!value) {
      return callback(new Error('电话不能为空'))
    }
    if (/^1[345789]\d{9}$/.test(value)) {
      callback()
    } else {
      callback(new Error('请正确填写电话号码'))
    }
  },
  id: function (rule, value, callback) {
    if (!value) {
      return callback(new Error('身份证不能为空'))
    }
    if (/(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/.test(value)) {
      callback()
    } else {
      callback(new Error('请输入正确的二代身份证号码'))
    }
  },
  email: function (rule, value, callback) {
    if (!value) {
      return callback(new Error('邮箱不能为空'))
    }
    if (/^(\w+\.?)*\w+@(?:\w+\.)\w+$/.test(value)) {
      callback()
    } else {
      callback(new Error('请输入正确的邮箱!'))
    }
  },
  name: function (rule, value, callback) {
    if (!value) {
      return callback(new Error('姓名不能为空'))
    }
    if (!/^[\u0391-\uFFE5A-Za-z]+$/.test(value)) {
      callback(new Error('只能输入汉字!'))
    } else if (value.length < 2 || value.length > 6) {
      callback(new Error('请输入2到6个字符!'))
    } else {
      callback()
    }
  },
  chunhanzi: function (rule, value, callback) {
    if (!/^[\u0391-\uFFE5A-Za-z]+$/.test(value)) {
      callback(new Error('只能输入汉字!'))
    } else {
      callback()
    }
  },
  noteshuzifu: function (rule, value, callback) {
    if (!value) {
      return callback(new Error('值不能为空'))
    }
    var pattern = new RegExp(
      "[`~!@#$%^&*()=|{}':;',\\[\\].<>《》/?~！@#￥……&*（）——|{}【】‘；：”“'。，、？]"
    );
    if (pattern.test(value)) {
      callback(new Error('不能包含特殊字符!'))
    }  else {
      callback()
    }
  },
  policenum: function (rule, value, callback) {
    if (!value) {
      return callback(new Error('值不能为空'))
    }
    if (!/^[A-Za-z0-9]{1,30}$/.test(value)) {
      callback(new Error('请输入字母或数字!'))
    }  else {
      callback()
    }
  },
  fax: (rule, value, callback) => {
    if (!value) {
      return callback(new Error('传真不能为空'))
    }
    if (/^(\d{3,4}-)?\d{7,8}$/.test(value)) {
      callback()
    } else {
      callback(new Error('请正确填写传真'))
    }
  },
  day: (rule, value, callback) => {
    if (!value) {
      return callback(new Error("请输入天号(1<=n<=31)"));
    }
    setTimeout(() => {
      if (!Number.isInteger(value)) {
        callback(new Error('请输入正确的值'));
      } else {
        if (value < 0 || value > 31) {
          callback(new Error('取值范围为1<=n<=31'));
        } else {
          callback();
        }
      }
    }, 100);
  },
  discountratio: (rule, value, callback) => {
    if (!value) {
      return callback(new Error("请输入折扣比(0<=n<=100)"));
    }
    setTimeout(() => {
      if (!Number.isInteger(value)) {
        callback(new Error('请输入正确的值'));
      } else {
        if (value < 0 || value > 100) {
          callback(new Error('取值范围为0<=n<=100'));
        } else {
          callback();
        }
      }
    }, 100);
  },
  isempty: (rule, value, callback) => {
    if (!value) {
      callback(new Error('请输入有效值'));
    } else {
      callback();
    }
  }
}


/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  i18n,
  store,
  render: h => h(App)
})
