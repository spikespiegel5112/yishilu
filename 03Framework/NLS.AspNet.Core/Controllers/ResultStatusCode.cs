using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NLS.AspNetCore.Controllers
{
    public static class EnumExtend
    {
        /// <summary>
        /// 获取枚举描述信息
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum en)
        {
            //返回信息
            string strDesc = en.ToString();
            //获取信息Type
            Type type = en.GetType();
            //获取成员类型信息集合
            MemberInfo[] memberInfos = type.GetMember(strDesc);
            if (memberInfos != null && memberInfos.Length > 0)
            {
                //获取自定义属性集合
                IEnumerable<Attribute> attrs = (IEnumerable<Attribute>)memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Any())
                {
                    strDesc = ((DescriptionAttribute)attrs.FirstOrDefault()).Description;
                }
            }
            return strDesc;
        }
    }

    public enum ResultStatusCode
    {
        #region 全局返回码区域

        /// <summary>
        /// 请求成功
        /// </summary>
        [Description("请求成功")]
        Success = 200,

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