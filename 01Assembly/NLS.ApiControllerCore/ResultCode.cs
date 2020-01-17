using System.ComponentModel;

namespace NLS.ApiControllerCore
{
    public enum ResultCode
    {
        #region 全局返回码区域

        /// <summary>
        /// 请求成功
        /// </summary>
        [Description("请求成功")]
        Success = 100,

        /// <summary>
        /// 请求错误
        /// </summary>
        [Description("请求错误")]
        RequestError = 101,

        /// <summary>
        /// 系统服务未知异常
        /// </summary>
        [Description("系统服务未知异常")]
        UnknowError = 102,

        /// <summary>
        /// 非法请求
        /// </summary>
        [Description("非法请求")]
        IllegalRequest = 103,

        /// <summary>
        /// 登录失败
        /// </summary>
        [Description("登录失败，用户名密码不匹配")]
        LoginFailed = 104,

        /// <summary>
        /// 保存失败
        /// </summary>
        [Description("保存失败")]
        SaveFailed = 105,

        /// <summary>
        /// 上传的文件太大
        /// </summary>
        [Description("上传的文件太大")]
        FileSizeTooLarge = 106,

        /// <summary>
        /// 图片格式不正确
        /// </summary>
        [Description("图片格式不正确")]
        PicSuffixError = 107,

        /// <summary>
        /// 名称重复
        /// </summary>
        [Description("名称或手机号重复")]
        RepeatNameError = 108,

        /// <summary>
        /// 手机号格式有误
        /// </summary>
        [Description("手机号格式有误")]
        PhoneError = 109,

        /// <summary>
        /// 用户已被禁用
        /// </summary>
        [Description("用户已被禁用")]
        UserEnableError = 110,

        [Description("存在相同数据")]
        ValueExists = 111,

        [Description("文件类型错误")]
        FileTypeError = 112,

        #endregion 全局返回码区域
    }
}