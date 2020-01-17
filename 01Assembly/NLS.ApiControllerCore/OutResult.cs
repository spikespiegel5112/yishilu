namespace NLS.ApiControllerCore
{
    public class OutResult
    {
        private static ResultCode DefaultCode = ResultCode.Success;

        /// <summary>
        /// 请求返回码
        /// </summary>
        public ResultCode Code { get; set; } = DefaultCode;

        /// <summary>
        /// 请求返回信息
        /// </summary>
        public string Msg { get; set; } = DefaultCode.GetDescription();

        /// <summary>
        /// 请求返回数据
        /// </summary>
        public object Data { get; set; }
    }
}