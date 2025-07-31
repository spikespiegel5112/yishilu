using Newtonsoft.Json;
using NLS.ExtentionsCore.CommonUnits;
using System;
using System.Net;
using System.Text;

namespace NLS.WeChatConfiguration
{
    /// <summary>
    /// 微信公众号开发设置
    /// 0.1
    /// 调用方式
    /// new WeChatConfiguration().SetAppId(AppId).SetSecret(Secret).SetCallBackUrl(Url).GetWeChatConfig()
    /// </summary>
    public sealed class WeChatConfiguration
    {
        private static string _AppId { get; set; }

        /// <summary>
        ///设置AppId
        ///AppId可在微信公众平台获取
        /// </summary>
        public WeChatConfiguration SetAppId(string _appId)
        {
            if (string.IsNullOrWhiteSpace(_appId)) { throw new ArgumentNullException("AppId不可为空"); }
            _AppId = _appId;
            return this;
        }

        private static string _Secret { get; set; }

        /// <summary>
        /// 设置Secret
        /// Secret可在微信公众平台获取
        /// </summary>
        public WeChatConfiguration SetSecret(string _secret)
        {
            if (string.IsNullOrWhiteSpace(_secret)) { throw new ArgumentNullException("Secret不可为空"); }
            _Secret = _secret;
            return this;
        }

        private static string _Url { get; set; }

        /// <summary>
        /// 设置所需微信签名页面回调地址
        /// </summary>
        public WeChatConfiguration SetCallBackUrl(string _url)
        {
            if (string.IsNullOrWhiteSpace(_url)) { throw new ArgumentNullException("CallBackUrl不可为空"); }
            _Url = _url;
            return this;
        }

        /// <summary>
        /// 获取与设置的AppId、Secret、CallbackUrl对应的
        /// Token，Signature，Timestamp，RndStr
        /// 返回包含AppId，RndStr，Timestamp，Token，Signature
        /// 的匿名对象
        /// </summary>
        public object GetWeChatConfig()
        {
            if (string.IsNullOrWhiteSpace(_AppId)) { throw new ArgumentNullException("AppId不可为空"); }
            if (string.IsNullOrWhiteSpace(_Secret)) { throw new ArgumentNullException("Secret不可为空"); }
            if (string.IsNullOrWhiteSpace(_Url)) { throw new ArgumentNullException("CallBackUrl不可为空"); }

            string __Timestamp = CommonUnits.TimestampSince1970;

            if (string.IsNullOrWhiteSpace(__Timestamp)) { throw new SystemException("时间戳获取失败"); }

            string _randomString = CommonUnits.GetRndString(16, true, true, true, false);

            if (string.IsNullOrWhiteSpace(_randomString)) { throw new SystemException("随机串获取失败"); }

            string _token = GetAccessToken();

            if (string.IsNullOrWhiteSpace(_token)) { throw new SystemException("获取WXToken失败"); }

            string _ticket = GetJsapi_ticket(_token);

            if (string.IsNullOrWhiteSpace(_ticket)) { throw new SystemException("获取Ticket失败"); }

            string _signature = GetSignature(_ticket, _randomString, __Timestamp);

            return new
            {
                AppId = _AppId,
                RndStr = _randomString,
                Timestamp = __Timestamp,
                Token = _token,
                Signature = _signature
            };
        }

        /// <summary>
        /// 获取微信ACCESS_TOKEN
        /// </summary>
        /// <returns></returns>
        private static string GetAccessToken()
        {
            try
            {
                string _grant_type = "client_credential";
                string tokenUrl = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}", _grant_type, _AppId, _Secret);
                var strReturn = new WebClient().DownloadString(tokenUrl);
                WXRModel wXRModel = JsonConvert.DeserializeObject<WXRModel>(strReturn);
                if (wXRModel != null)
                {
                    return wXRModel.access_token;
                }
            }
            catch { }
            return string.Empty;
        }

        /// <summary>
        /// 获取微信Ticket
        /// </summary>
        private static string GetJsapi_ticket(string accesstoken)
        {
            try
            {
                string jsapi_ticket = string.Empty;
                string getUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";
                var strReturn = new WebClient().DownloadString(string.Format(getUrl, accesstoken)); //取得微信返回的json数据
                WXRModel wXRModel = JsonConvert.DeserializeObject<WXRModel>(strReturn);
                if (wXRModel != null)
                {
                    return wXRModel.ticket;
                }
            }
            catch { }
            return string.Empty;
        }

        /// <summary>
        /// 获取对应字符的SHA1
        /// 0.1
        /// Lv
        /// 2018/02/10
        /// </summary>
        private static string GetSignature(string jsapi_ticket, string noncestr, string timestamp)
        {
            var string1Builder = new StringBuilder();
            string1Builder.Append("jsapi_ticket=").Append(jsapi_ticket).Append("&")
                          .Append("noncestr=").Append(noncestr).Append("&")
                          .Append("timestamp=").Append(timestamp).Append("&")
                          .Append("url=").Append(_Url.IndexOf("#") >= 0 ? _Url.Substring(0, _Url.IndexOf("#")) : _Url);
            string string1 = string1Builder.ToString();
            return CommonUnits.SHA1(string1.ToString());
        }
    }

    internal class WXRModel
    {
        //{"errcode":40125,"errmsg":"invalid appsecret, view more at http:\/\/t.cn\/RAEkdVq hint: [NownNA00842974]"}
        public string errcode { get; set; }

        public string errmsg { get; set; }
        public string hint { get; set; }

        //{"access_token":"5_7ztn_6Pnw9xLGwtjnRf5ZU6WUfsHenXXFTTIKKogeXC3kG5kMmFAhNV-6mdWLKoapwvzNShZ0ZVfdyFNF6GuI403oxeDQhsaT3e7N3sj5WmSPV5F6bDK9dNEy5UJM1VG3V7pY8ErlkfhIyrWFXBdAHAUMQ","expires_in":7200}
        public string access_token { get; set; }

        public string ticket { get; set; }
        public string expires_in { get; set; }
    }
}