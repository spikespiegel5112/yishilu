using NLS.AspNetCore.Controllers;
using NLS.Framework.Entites.SY;
using NLS.ServiceCore.SY.Authorize;
using System.Threading.Tasks;

namespace NLS.ServiceCore.SY
{
    public interface IAuthorizeService : INLSServiceApplication
    {
        /// <summary>
        /// 用户登录验证，返回用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseParamters<User>> Authorize(UserAuthorizeInputDto input);
        /// <summary>
        /// 微信授权
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="avarurl"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        Task<WxUser> GetUserInfoByOpenid(string openid, string avarurl, string nickname);
    }
}