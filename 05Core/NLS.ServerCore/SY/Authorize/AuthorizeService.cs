using Microsoft.EntityFrameworkCore;
using NLS.AspNetCore.Controllers;
using NLS.Framework;
using NLS.Framework.EF.Repository;
using NLS.Framework.Entites.SY;
using NLS.Framework.ExtentionCore;
using NLS.ServiceCore.SY.Authorize;
using System;
using System.Threading.Tasks;

namespace NLS.ServiceCore.SY
{
    /// <summary>
    /// 账户验证
    /// </summary>
    public sealed class AuthorizeService:BaseService<NLSEntitesContext> ,INLSServiceApplication, IAuthorizeService
    {
        private readonly IRepository<WxUser> sysUserRepository;

        private readonly ICommonUnits commonUnits;

        public AuthorizeService(IRepository<WxUser> sysUserRepository, ICommonUnits commonUnits)
        {
            this.sysUserRepository = sysUserRepository;
            this.commonUnits = commonUnits;
        }

        /// <summary>
        /// 验证登录信息
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseParamters<User>> Authorize(UserAuthorizeInputDto input)
        {
            ResponseParamters<User> result = new ResponseParamters<User>();
            if (input == null || string.IsNullOrWhiteSpace(input.Openid))
            {
                result.State = ResultStatusCode.LoginFailed;
                result.Msg = "传入参数有误";
                return result;
            }
            var userInfo = await sysUserRepository.GetAllAsNoTracking(p => p.Openid == input.Openid).FirstOrDefaultAsync();
            if (userInfo == null)
            {
                result.State = ResultStatusCode.LoginFailed;
                result.Msg = "未查询到用户信息";
                return result;
            }
            return result;
        }

        #region 微信用户授权登录
        public async Task<WxUser> GetUserInfoByOpenid(string openid, string avarurl, string nickname)
        {
            var customerinfo = DBRepository.SearchFirstOrDefault<WxUser>(w => w.Openid == openid);
            if (customerinfo != null)
            {
                return customerinfo;
            }
            else
            {
                WxUser addmodel = new WxUser();
                addmodel.Openid = openid;
                addmodel.NickName = nickname;
                addmodel.CreateTime = DateTime.Now;
                addmodel.Avatar = avarurl;
                await DBRepository.InsertAsync<WxUser>(addmodel);
                return addmodel;
            }
        }
        #endregion
    }
}