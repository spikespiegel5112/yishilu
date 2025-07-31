using Microsoft.AspNetCore.Mvc;
using NLS.Entites.Context;
using System;

namespace NLS.ApiControllerCore
{
    public class BaseController : Controller
    {
        private NLSEntitesContext Context;

        /// <summary>
        /// 反射引用Service
        /// 当存在 Constructor(NLSEntitesContext) 时自动注入
        /// </summary>
        /// <typeparam name="T">Service类型</typeparam>
        /// <returns>Service实例</returns>
        protected T Reference<T>()
        {
            Type t = typeof(T);
            if (t.GetConstructor(new Type[] { typeof(NLSEntitesContext) }) != null)
            {
                return (T)Activator.CreateInstance(t, Context);
            }
            return (T)Activator.CreateInstance(t);
        }

        public BaseController(NLSEntitesContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// 请求返回状态
        /// 默认为 Success
        /// </summary>
        protected ResultCode Code { get; set; } = ResultCode.Success;

        /// <summary>
        ///  请求结果
        /// </summary>
        protected object Data { get; set; }

        /// <summary>
        ///  请求消息体
        /// </summary>
        protected string Msg { get; set; }

        /// <summary>
        /// 接口返回方法
        /// </summary>
        /// <param name="result">字符类型结果</param>
        /// <returns></returns>
        //[NonAction]
        // protected virtual ContentResult ResultStr(string result) => new ContentResult { Content = result, ContentType = "String", StatusCode = (int)Code };

        /// <summary>
        /// 接口返回方法
        /// </summary>
        [NonAction]
        protected virtual JsonResult Result(object obj) => Json(new
        {
            Code,
            Data = obj,
            Msg
        });

        /// <summary>
        /// 接口返回方法
        /// </summary>
        [NonAction]
        protected virtual JsonResult Result() => Json(new
        {
            Code,
            Data,
            Msg
        });

       
    }
}