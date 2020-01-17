namespace NLS.Framework.ExtentionCore
{
    /// <summary>
    /// 框架通用方法集合
    /// </summary>
    public interface ICommonUnits : IFrameworkDependency
    {
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptoContent">待加密内容</param>
        /// <param name="cryptoKey">加密Key</param>
        /// <returns>加密后字符串</returns>
        string DESEncrypto(string encryptoContent, string cryptoKey = "");

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptoContent">待解密内容字符串</param>
        /// <param name="cryptoKey">加密Key</param>
        /// <returns>解密后内容</returns>
        string DESDecrypto(string decryptoContent, string cryptoKey = "");

        /// <summary>
        /// 获取传入字符串MD5
        /// </summary>
        /// <param name="source">原字符串</param>
        /// <param name="output16">是否输出16位</param>
        /// <param name="toUpper">是否输出大写字符</param>
        /// <returns>输入字符串的MD5码</returns>
        string GetMD5(string source, bool output16 = false, bool toUpper = false);

        ///<summary>
        ///生成随机字符串
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，默认为包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        string GetRndString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe);
    }
}