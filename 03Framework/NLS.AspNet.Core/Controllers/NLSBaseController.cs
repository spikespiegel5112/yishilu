using Castle.Windsor;
using Microsoft.AspNetCore.Mvc;
using NLS.AspNetCore.Authorize;

namespace NLS.AspNetCore.Controllers
{
    /// <summary>
    ///基础Controller
    /// </summary>
    public abstract class NLSBaseController : Controller
    {
        /// <summary>
        /// IoC容器
        /// </summary>
        public static IWindsorContainer IoCManager;

        /// <summary>
        /// 存放用户信息
        /// 并非真正Session对象
        /// </summary>
        public static INLSNullSession NLSSession { get; set; } = new NLSNullSession();

        /// <summary>
        /// 接口返回方法
        /// </summary>
        /// <returns>ResponseParamters</returns>
        [NonAction]
        public virtual JsonResult Result() => Json(new ResponseParamters<string>());

        /// <summary>
        /// 接口返回方法
        /// </summary>
        /// <param name="responseParamters">返回参数</param>
        /// <returns>ResponseParamters</returns>
        [NonAction]
        public virtual JsonResult Result<T>(ResponseParamters<T> responseParamters) => Json(responseParamters);

        /// <summary>
        /// 接口返回方法
        /// </summary>
        /// <param name="code">请求状态码</param>
        /// <returns>ResponseParamters</returns>
        [NonAction]
        public virtual JsonResult Result(ResultStatusCode code) => Json(new ResponseParamters<string>
        {
            State = code
        });

        /// <summary>
        /// 接口返回方法
        /// </summary>
        /// <param name="code">返回状态码</param>
        /// <param name="msg">返回提示信息</param>
        /// <returns>ResponseParamters</returns>
        [NonAction]
        public virtual JsonResult Result(ResultStatusCode code, string msg) => Json(new ResponseParamters<string>
        {
            State = code,
            Msg = msg
        });

        /// <summary>
        /// 接口返回方法
        /// </summary>
        /// <typeparam name="T">传入参数类型</typeparam>
        /// <param name="result">传入数据</param>
        /// <returns>ResponseParamters</returns>
        [NonAction]
        public virtual JsonResult Result<T>(T result) => Json(new ResponseParamters<T>
        {
            Data = result
        });

        /// <summary>
        /// 接口返回方法
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="code">接口返回状态码</param>
        /// <param name="result">返回数据</param>
        /// <returns>ResponseParamters</returns>
        [NonAction]
        public virtual JsonResult Result<T>(ResultStatusCode code, T result) => Json(new ResponseParamters<T>
        {
            State = code,
            Data = result
        });

        /// <summary>
        /// 接口返回方法
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="code">接口返回状态码</param>
        /// <param name="msg">提示信息</param>
        /// <param name="result">返回数据</param>
        /// <returns>ResponseParamters</returns>
        [NonAction]
        public virtual JsonResult Result<T>(ResultStatusCode code, string msg, T result) => Json(new ResponseParamters<T>
        {
            State = code,
            Msg = msg,
            Data = result
        });
    }
}