namespace NLS.AspNetCore.Controllers
{
    /// <summary>
    /// 接口数据返回结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseParamters<T>
    {
        private static ResultStatusCode DefaultCode = ResultStatusCode.Success;

        /// <summary>
        /// 请求返回码
        /// </summary>
        public ResultStatusCode State { get; set; } = DefaultCode;

        /// <summary>
        /// 请求返回信息
        /// </summary>
        public string Msg { get; set; } = DefaultCode.GetDescription();

        /// <summary>
        /// 请求返回数据
        /// </summary>
        public T Data { get; set; }
    }
}