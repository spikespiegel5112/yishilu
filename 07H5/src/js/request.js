import axios from 'axios';
import store from '../store/store';
import util from './util';
const baseUrl = process.env.NODE_ENV === 'production' ? 'http://essilorevent.digital-support.cn/api/' : 'http://192.168.124.8:799/';
// const baseUrl = process.env.NODE_ENV === 'production' ? 'https://api.pengpai.nplusgroup.com/' : 'http://api.pengpai.nplusgroup.com:81/';


// 创建axios实例
const service = axios.create({
  baseURL: baseUrl, // api的base_url
  timeout: 5000, // 请求超时时间
  // transformRequest: [function(data) {
  //   // Do whatever you want to transform the data
  //   let ret = '';
  //   for (const it in data) {
  //     ret += encodeURIComponent(it) + '=' + encodeURIComponent(data[it]) + '&'
  //   }
  //   return ret
  // }],
  headers: {
    // 'Content-Type': 'application/json;charset=utf-8'
  }
});

// request拦截器
service.interceptors.request.use(config => {
  let authToken = util.$webStorage.getItem('authToken');

  if (authToken !== undefined) {
    // console.log(store.state.accessToken)
    // config.data = true;//axios在requestData 为undefined时会自动去掉content-type的header
    config.headers['Accept'] = 'application/json';
    config.headers['Authorization'] = 'Bearer ' + authToken;// 请根据实际情况自行修改
    config.headers['Content-Type'] = 'application/json; charset=utf-8';

  }
  return config;
}, error => {
  // Do something with request error
  console.log(error); // for debug
  Promise.reject(error);
});

// respone拦截器
service.interceptors.response.use(
  response => {
    return response.data;
  },
  error => {
    console.log(error);
    const result_response = error.response;

    if (error.response && error.response.status === 401) {
      util.$webStorage.removeItem('authToken');
      location.reload();
      return false;
    }


    console.log('err' + error);// for debug
    // Message({
    //   message: error.message,
    //   type: 'error',
    //   duration: 5 * 1000
    // })
    return Promise.reject(error.response);
  }
);

export default service;

export { baseUrl };
