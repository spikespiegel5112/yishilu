using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NLS.AspNetCore.Authorize;
using NLS.AspNetCore.Controllers;
using NLS.Framework.Entites.SY;
using NLS.Framework.ExtentionCore;
using NLS.ServiceCore.SY;
using NLS.ServiceCore.SY.Authorize;
using NLS.ServiceCore.SY.SysUser;
using NLS.ServiceCore.SY.SysUser.Dto;
using NLS.WeChatConfiguration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static NLS.WeChatConfiguration.WeiXinHelper;

namespace NLS.Host.ControllerExtension.SYS
{
    /// <summary>
    /// 用户验证
    /// </summary>
    [NLSAuthorize]
    public sealed class AuthorizeController : NLSBaseController
    {
        private readonly IAuthorizeService authorizeService;
        private readonly ISysUserService sysUserService;

        private readonly ICommonUnits commonUnits;

        /// <summary>
        /// 构造
        /// </summary>
        public AuthorizeController(IAuthorizeService authorizeService, ISysUserService sysUserService, ICommonUnits commonUnits)
        {
            this.authorizeService = authorizeService;
            this.sysUserService = sysUserService;
            this.commonUnits = commonUnits;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("get/verificationcode")]
        public async Task<JsonResult> SMSValidate(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return Result(ResultStatusCode.PhoneError, "请输入有效的手机号码");
            }

            //获取6位随机串
            var _rnd_num = await Task.FromResult(commonUnits.GetRndString(6, true, false, false, false));
            //这里开始发送短信

            return Result(_rnd_num);
        }

        /// <summary>
        /// 系统用户登录接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns>系统用户信息</returns>
        [AllowAnonymous]
        [HttpPost("sys.user.login")]
        public async Task<JsonResult> SysUserLogin([FromBody]User input)
        {
            var result = await sysUserService.PcUserLoginAsync(input);
            if (result.State == ResultStatusCode.Success)
            {
                return Result(result);
            }
            return Result(result.State, result.Msg);
        }
        #region 微信用户管理
        /// <summary>
        /// 获取微信用户的分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("get.wxuser.pagelist")]
        public async Task<JsonResult> GetTeamPageList(UserSerchModel input)
        {
            return Result(await sysUserService.GetWxUserPageList(input));
        }
        /// <summary>
        /// 编辑添加用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("add.update.wxuser")]
        public async Task<JsonResult> AddOrUpdateWxUser([FromBody]WxUser input)
        {
            var result = await sysUserService.AddOrUpdateWxUser(input);
            if (result.State == ResultStatusCode.Success)
            {
                return Result(result);
            }
            return Result(result.State, result.Msg);
        }
        /// <summary>
        /// 获取中奖或未中奖用户
        /// </summary>
        /// <param name="f_prize"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("get.prize.wxuser.list")]
        public async Task<JsonResult> GetPrizeUserList(int f_prize)
        {
            return Result(await sysUserService.GetPrizeUserList(f_prize));
        }
        /// <summary>
        /// 抽奖
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="prizetype"></param> 
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("update.wxuser.prizestate")]
        public JsonResult UpdateUserPrizeState(string uids, int prizetype)
        {
            var result =  sysUserService.UpdateUserPrizeState(uids, prizetype);
            if (result.State == ResultStatusCode.Success)
            {
                return Result(result);
            }
            return Result(result.State, result.Msg);
        }

        /// <summary>
        /// 清空抽奖
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("clear.wxuser.prizestate")]
        public JsonResult ClearUserPrizeState()
        {
            var result = sysUserService.ClearUserPrizeState();
            if (result.State == ResultStatusCode.Success)
            {
                return Result(result);
            }
            return Result(result.State, result.Msg);
        }
        #endregion


        /// <summary>
        /// 用户验证登录接口
        /// </summary>
        /// <returns>用户Token</returns>
        [AllowAnonymous]
        [HttpPut("authorize")]
        public async Task<JsonResult> Authorize([FromBody]UserAuthorizeInputDto input)
        {
            var result = await authorizeService.Authorize(input);
            if (result.State == ResultStatusCode.Success)
            {
                var _token = AuthorizeGetToken(result.Data);
                return Result(result.State, result.Msg, _token);
            }
            return Result(result.State, result.Msg);
        }

        private string AuthorizeGetToken(User sys_User)
        {
            var claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("UserId",sys_User.Id.ToString(),ClaimValueTypes.Integer32),
                new Claim("UserName",sys_User.UserName,ClaimValueTypes.String),
                new Claim("ExpireTime","7200000")
            };
            var token = new JwtSecurityToken(
                 issuer: "NLS_XINYI",
                 audience: "NLS_XINYI",
                 claims: claims,
                 notBefore: DateTime.Now,
                 expires: DateTime.Now.AddDays(1),
                 signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("G0ae*$3c5nmahi4005pa2ea-0fea210c8tae")), SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #region 微信授权
        /// <summary>
        /// 微信授权
        /// </summary>
        /// <param name="input"></param>
        /// <returns>系统用户信息</returns>
        [AllowAnonymous]
        [HttpPost("wx.login.openid")]
        public async Task<JsonResult> GetWXOpenId([FromBody]WXResultModel input)
        {
            var token = await WeiXinHelper.GetAccessToken(input.code);
            if (string.IsNullOrEmpty(token.openid))
            {
                return Result(ResultStatusCode.RequestError, "code已经过期");
            }
            WXUserInfo userInfo = await WeiXinHelper.GetWxUserInfo(token.access_token, token.openid);
                var data = await authorizeService.GetUserInfoByOpenid(userInfo.openid, userInfo.headimgurl, userInfo.nickname);
                return Result(data);
        }
        /// <summary>
        /// 通过openid获取用户信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("wx.login.user.byopenid")]
        public async Task<JsonResult> GetUserByWXOpenId(string openid)
        {
            var data =await  authorizeService.GetUserInfoByOpenid(openid, "", "");
            return Result(data);
        }

        /// <summary>
        /// 
        /// </summary>
        public class WXResultModel
        {
            /// <summary>
            /// session_key
            /// </summary>
            public string session_key { get; set; }
            /// <summary>
            /// 授权码
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// openid
            /// </summary>
            public string openid { get; set; }
            /// <summary>
            /// 头像
            /// </summary>
            public string avatarUrl { get; set; }
        }
        #endregion

        #region 系统配置
        /// <summary>
        /// 编辑系统设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("add.update.setting")]
        public async Task<JsonResult> AddOrUpdateTeam([FromBody]Sys_Setting input)
        {
            var result = await sysUserService.AddOrUpdateSetting(input);
            if (result.State == ResultStatusCode.Success)
            {
                return Result(result);
            }
            return Result(result.State, result.Msg);
        }
        #endregion
    }
}