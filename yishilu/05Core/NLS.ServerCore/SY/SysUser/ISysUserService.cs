using NLS.AspNetCore.Controllers;
using NLS.AspNetCore.Linq;
using NLS.Framework.Entites.SY;
using NLS.ServiceCore.SY.SysUser.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NLS.ServiceCore.SY.SysUser
{
    public interface  ISysUserService : INLSServiceApplication
    {
        /// <summary>
        /// 后台登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseParamters<User>> PcUserLoginAsync(User model);
        /// <summary>
        /// 获取微信用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageResult<WxUser>> GetWxUserPageList(UserSerchModel input);
        /// <summary>
        /// 更新添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseParamters<string>> AddOrUpdateWxUser(WxUser model);
        /// <summary>
        /// 更新系统配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseParamters<string>> AddOrUpdateSetting(Sys_Setting model);
        /// <summary>
        /// 获取中奖或未中奖用户
        /// </summary>
        /// <param name="F_Prize"></param>
        /// <returns></returns>
        Task<List<WxUser>> GetPrizeUserList(int F_Prize);
        /// <summary>
        /// 抽奖
        /// </summary>
        /// <param name="uids"></param>
        /// <returns></returns>
        ResponseParamters<string> UpdateUserPrizeState(string uids, int prizetype);
        /// <summary>
        /// 清空中奖记录
        /// </summary>
        /// <returns></returns>
        ResponseParamters<string> ClearUserPrizeState();
    }
}
