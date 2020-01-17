using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NLS.WeChatConfiguration
{
    public class WeiXinHelper
    {
        public static IHostingEnvironment _Env;

        public WeiXinHelper(IHostingEnvironment Env)
        {
            _Env = Env;
        }
    

        public class Access_token_basics
        {
            public string access_token { get; set; }
        }

        public class WXResultModel
        {
            public string access_token { get; set; }
            public string openid { get; set; }
            public int expires_in { get; set; }
            public string refresh_token { get; set; }

            public string scope { get; set; }
        }


        #region 获取 网络 或者 基础Access_Token

        ///<summary>
        ///通过微信Code获取微信OpenId 和 access_token   获取网络Access_Token
        ///</summary>
        public static async Task<WXResultModel> GetAccessToken(string code)
        {
            var url = @"https://api.weixin.qq.com/sns/oauth2/access_token?"
                         + $"appid=wxb2602761a856bbea&secret=5a9c6ceb85d8a05414fc7a73547d5044&code="
                          + $"{code}&grant_type=authorization_code";

            string response_result = string.Empty;
            using (HttpClient httpclient = new HttpClient())
            {
                var response = await httpclient.GetAsync(url);
                byte[] Byte_Array_Result = response.Content.ReadAsByteArrayAsync().Result;
                response_result = Encoding.UTF8.GetString(Byte_Array_Result);
            }
            if (response_result == null) { return null; }
            WXResultModel wxresultmodel = JsonConvert.DeserializeObject<WXResultModel>(response_result);
            return wxresultmodel;
        }

        ///<summary>
        ///通过微信Code获取微信OpenId 和 access_token   获取基础Access_Token
        ///</summary>
        public static async Task<string> GetAccessToken_basics()
        {
            var url = @"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&"
                         + $"appid=wxb2602761a856bbea&secret=5a9c6ceb85d8a05414fc7a73547d5044";

            string response_result = string.Empty;
            using (HttpClient httpclient = new HttpClient())
            {
                var response = await httpclient.GetAsync(url);
                byte[] Byte_Array_Result = response.Content.ReadAsByteArrayAsync().Result;
                response_result = Encoding.UTF8.GetString(Byte_Array_Result);
            }
            if (response_result == null) { return null; }
            Access_token_basics wxresultmodel = JsonConvert.DeserializeObject<Access_token_basics>(response_result);
            return wxresultmodel.access_token;
        }




        #endregion

        #region 获取用户信息（网络授权机制）（UnionID机制）
        /// <summary>
        /// 获取用户信息（UnionID机制） 没关注不能获取用户信息
        /// </summary>
        /// <param name="access_token">基础access_token</param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static async Task<WXUserInfo> GetWxUserInfo_UnionID(string access_token, string openid)
        {
            var url = @"https://api.weixin.qq.com/cgi-bin/user/info?" +
                $"access_token={access_token}&openid={openid}&lang=zh_CN";
            string response_result = string.Empty;
            using (HttpClient httpclient = new HttpClient())
            {
                var response = await httpclient.GetAsync(url);
                byte[] Byte_Array_Result = response.Content.ReadAsByteArrayAsync().Result;
                response_result = Encoding.UTF8.GetString(Byte_Array_Result);
            }
            if (response_result == null) { return null; }

            WXUserInfo userinfo = JsonConvert.DeserializeObject<WXUserInfo>(response_result);
            return userinfo;
        }


        /// <summary>
        /// 获取用户信息（网络授权机制） 
        /// </summary>
        /// <param name="access_token">基础access_token</param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static async Task<WXUserInfo> GetWxUserInfo(string access_token, string openid)
        {
            var url = @"https://api.weixin.qq.com/sns/userinfo?" +
                $"access_token={access_token}&openid={openid}&lang=zh_CN";
            string response_result = string.Empty;
            using (HttpClient httpclient = new HttpClient())
            {
                var response = await httpclient.GetAsync(url);
                byte[] Byte_Array_Result = response.Content.ReadAsByteArrayAsync().Result;
                response_result = Encoding.UTF8.GetString(Byte_Array_Result);
            }
            if (response_result == null) { return null; }

            WXUserInfo userinfo = JsonConvert.DeserializeObject<WXUserInfo>(response_result);
            return userinfo;
        }
        #endregion

        public static async Task<string> GetJsapi_ticket(string accesstoken)
        {

            string jsapi_ticket = string.Empty;
            string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";
            url = string.Format(url, accesstoken);
            string response_result = string.Empty;
            using (HttpClient httpclient = new HttpClient())
            {
                var response = await httpclient.GetAsync(url);
                byte[] Byte_Array_Result = response.Content.ReadAsByteArrayAsync().Result;
                response_result = Encoding.UTF8.GetString(Byte_Array_Result);
            }
            if (response_result == null) { return null; }

            string ticket = JsonConvert.DeserializeObject<dynamic>(response_result).ticket;
            return ticket;
        }

        public class WXUserInfo
        {

            public string subscribe { get; set; }
            public string openid { get; set; }
            public string nickname { get; set; }
            public int sex { get; set; }
            public string province { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string headimgurl { get; set; }
            public string[] privilege { get; set; }
            public string unionid { get; set; }
        }
    }
}
