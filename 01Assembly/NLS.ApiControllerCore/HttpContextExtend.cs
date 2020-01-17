using Microsoft.AspNetCore.Http;
using System.Linq;

namespace NLS.ApiControllerCore
{
    public static class HttpContextExtend
    {
        /// <summary>
        /// 获取传递过来的校验信息扩展
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetHeadParm(this HttpContext context, string parmName)
        {
            string checkInfo = "";
            var checkInfoByContext = context.Request.Headers[parmName];
            if (checkInfoByContext.Count > 0)
            {
                checkInfo = checkInfoByContext.FirstOrDefault();
            }
            return checkInfo;
        }

        /// <summary>
        /// 从请求上下文中获取Token信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetToken(this HttpContext context)
        {
            string token = "";
            var tokenByContext = context.Request.Headers["access-token"];
            if (tokenByContext.Count > 0)
            {
                token = tokenByContext.FirstOrDefault();
            }
            return token;
        }

        /// <summary>
        /// 获取appId
        /// </summary>
        /// <param name="context">请求上下文</param>
        /// <returns></returns>
        public static string GetAppId(this HttpContext context)
        {
            string appId = "";
            var appIdByContext = context.Request.Headers["appId"];
            if (appIdByContext.Count > 0)
            {
                appId = appIdByContext.FirstOrDefault();
            }
            return appId;
        }

        /// <summary>
        /// 获取请求头部签名时间
        /// 说明:签名时间格式:yyyyMMddHHmmss格式的14位字符串
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetSignTime(this HttpContext context)
        {
            //时间参数
            string signTime = "";
            var signTimeByContext = context.Request.Headers["signTime"];
            if (signTimeByContext.Count > 0)
            {
                signTime = signTimeByContext.FirstOrDefault();
            }
            return signTime;
        }

        /// <summary>
        /// 获取传递过来的随机数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetRandomNum(this HttpContext context)
        {
            string randomNum = "";
            var randomNumByContext = context.Request.Headers["randomNum"];
            if (randomNumByContext.Count > 0)
            {
                randomNum = randomNumByContext.FirstOrDefault();
            }
            return randomNum;
        }

        /// <summary>
        /// 获取传递过来的校验信息扩展
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetCheckInfo(this HttpContext context)
        {
            string checkInfo = "";
            var checkInfoByContext = context.Request.Headers["checkInfo"];
            if (checkInfoByContext.Count > 0)
            {
                checkInfo = checkInfoByContext.FirstOrDefault();
            }
            return checkInfo;
        }

        /// <summary>
        /// 获取请求的IP
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
    }
}