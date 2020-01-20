using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLS.AspNetCore.Controllers;
using NLS.Framework.Entites.SY;
using NLS.ServiceCore.SY.Task;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NLS.Host.ControllerExtension.SYS
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskController : NLSBaseController
    {
        private readonly ITaskService taskService;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="itaskService"></param>
        public TaskController(ITaskService itaskService)
        {
            this.taskService = itaskService;
        }

        /// <summary>
        /// 用户领取任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("h5.user.receive.task")]
        public async Task<JsonResult> ReceiveTask([FromBody]Wx_User_Task input)
        {
            var result = await taskService.ReceiveTask(input);
            if (result.State == ResultStatusCode.Success)
            {
                return Result(result);
            }
            return Result(result.State, result.Msg);
        }

        /// <summary>
        /// 用户点亮任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("h5.user.light.task")]
        public async Task<JsonResult> UserLigntTask([FromBody]Wx_User_Task input)
        {
            var result = await taskService.UserLigntTask(input);
            if (result.State == ResultStatusCode.Success)
            {
                return Result(result);
            }
            return Result(result.State, result.Msg);
        }

        /// <summary>
        /// 获取用户任务是否点亮
        /// </summary>
        /// <param name="u_id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("h5.get.wxuser.lighten")]
        public async Task<JsonResult> GetLighten(int u_id)
        {
            return Result(await taskService.GetLighten(u_id));
        }

        /// <summary>
        /// 用户抽奖
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("h5.user.luck.draw")]
        public async Task<JsonResult> UserLuckDraw([FromBody]Draw input)
        {
            var result = await taskService.UserLuckDraw(input.u_id);
            if (result.State == ResultStatusCode.Success)
            {
                return Result(result);
            }
            return Result(result.State, result.Msg);
        }

        /// <summary>
        /// 获取抽奖列表
        /// </summary>
        /// <param name="u_id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("h5.get.wxuser.drawlist")]
        public async Task<JsonResult> GetPrizeList(int u_id)
        {
            return Result(await taskService.GetPrizeList(u_id));
        }

        /// <summary>
        /// 
        /// </summary>
        public class Draw {
            public int u_id { get; set; }
        }
    }
}
