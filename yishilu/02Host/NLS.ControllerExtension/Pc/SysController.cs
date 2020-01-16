using Microsoft.AspNetCore.Mvc;
using NLS.ApiControllerCore;
using NLS.Entites.Context;
using NLS.Entites.Entites;
using NLS.ServiceCore.SystemService;
using System;
using System.Threading.Tasks;

namespace NLS.Host.ControllerExtension.Pc
{
    public class SysController : BaseController
    {
        private readonly ISysServer sysServer;

        public SysController(NLSEntitesContext context) : base(context)
        {
            sysServer = Reference<SysServer>();
        }

        #region 系统用户管理

        /// <summary>
        /// 后台用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("pc.user.login")]
        public async Task<IActionResult> PcUserLogin([FromBody]Sys_User model)
        {
            try
            {
                var result = await sysServer.PcUserLoginAsync(model);
                Code = result.Code;
                Data = result.Data;
            }
            catch (Exception e)
            {
                Code = ResultCode.UnknowError;
                Msg = e.Message;
            }
            return this.Result();
        }

        /// <summary>
        /// 伪删除系统用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("pc.delete.sysuser")]
        public async Task<IActionResult> DeleteSysuser([FromBody]Sys_User model)
        {
            try
            {
                var result = await sysServer.DeleteSysuser(model);
                Data = result;
            }
            catch (Exception e)
            {
                Code = ResultCode.UnknowError;
                Msg = e.Message;
            }
            return this.Result();
        }

        /// <summary>
        /// 启用禁用系统用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("pc.isenable.sysuser")]
        public async Task<IActionResult> IsEnableSysuser([FromBody]Sys_User model)
        {
            try
            {
                var result = await sysServer.IsEnableSysuser(model);
                Data = result;
            }
            catch (Exception e)
            {
                Code = ResultCode.UnknowError;
                Msg = e.Message;
            }
            return this.Result();
        }

        /// <summary>
        /// 新增修改系统用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("pc.add.update.user")]
        public async Task<IActionResult> AddorUpdateUser([FromBody]Sys_User model)
        {
            try
            {
                var result = await sysServer.AddorUpdateUser(model);
                Data = result;
            }
            catch (Exception e)
            {
                Code = ResultCode.UnknowError;
                Msg = e.Message;
            }
            return this.Result();
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("pc.update.sysuser.resetpwd")]
        public async Task<IActionResult> ResetPwdSysuser([FromBody]Sys_User model)
        {
            try
            {
                var result = await sysServer.ResetPwdSysuser(model);
                Data = result;
            }
            catch (Exception e)
            {
                Code = ResultCode.UnknowError;
                Msg = e.Message;
            }
            return this.Result();
        }

        [HttpGet("pc.get.user.pagelist")]
        public IActionResult GetSysUserPageList(int eid = 1, string keyword = "", int pagesize = 10, int pageindex = 1)
        {
            try
            {
                var result = sysServer.GetSysUserPageList(eid, keyword, pagesize, pageindex);
                Data = result;
            }
            catch (Exception e)
            {
                Code = ResultCode.UnknowError;
                Msg = e.Message;
            }
            return this.Result();
        }

        #endregion 系统用户管理

        #region 系统角色管理

        /// <summary>
        /// 获取角色分页列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("get.pc.role.pagelist")]
        public async Task<IActionResult> GetRolePagelist(int pageIndex = 1, int pageSize = 10, string keyword = "")
        {
            try
            {
                var result = await sysServer.GetRolePagelist(pageIndex, pageSize, keyword);
                Code = result.Code;
                Data = result.Data;
            }
            catch (Exception e)
            {
                Code = ResultCode.UnknowError;
                Msg = e.Message;
            }
            return this.Result();
        }

        /// <summary>
        /// 修改添加角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("add.update.pc.role")]
        public async Task<IActionResult> AddorUpdateRole([FromBody]Sys_Role role)
        {
            var result = await sysServer.AddorUpdateRole(role);
            Code = result.Code;
            Data = result.Data;
            return this.Result();
        }

        /// <summary>
        /// 伪删除角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("delete.pc.role")]
        public async Task<IActionResult> DeleteSysRole([FromBody]Sys_Role role)
        {
            try
            {
                var result = await sysServer.DeleteSysRole(role);
                Code = result.Code;
                Data = result.Data;
            }
            catch (Exception e)
            {
                Code = ResultCode.UnknowError;
                Msg = e.Message;
            }
            return this.Result();
        }

        /// <summary>
        /// 获取角色菜单
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        [HttpGet("get.pc.role.menulist")]
        public async Task<IActionResult> GetRoleMenulist(int rid = 0)
        {
            try
            {
                var result = await sysServer.GetRoleMenulist(rid);
                Code = result.Code;
                Data = result.Data;
            }
            catch (Exception e)
            {
                Code = ResultCode.UnknowError;
                Msg = e.Message;
            }
            return this.Result();
        }

        /// <summary>
        /// 获取角色下拉
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        [HttpGet("get.pc.role.selectlist")]
        public async Task<IActionResult> GetRoleSelectlist()
        {
            try
            {
                var result = await sysServer.GetRoleSelectlist();
                Code = result.Code;
                Data = result.Data;
            }
            catch (Exception e)
            {
                Code = ResultCode.UnknowError;
                Msg = e.Message;
            }
            return this.Result();
        }

        #endregion 系统角色管理
    }
}