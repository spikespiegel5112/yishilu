using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NLS.Framework.ExtentionCore
{
    public sealed class CommonUnits : IFrameworkDependency, ICommonUnits
    {
        public string DESDecrypto(string decryptoContent, string cryptoKey = "")
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(cryptoKey);
            byte[] keyIV = Encoding.UTF8.GetBytes("!7o)P126");
            byte[] inputByteArray = StrToToHexByte(decryptoContent);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider() { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7, Key = keyBytes, IV = keyIV };
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }

        public string DESEncrypto(string encryptoContent, string cryptoKey = "")
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(cryptoKey);
            byte[] keyIV = Encoding.UTF8.GetBytes("!7o)P126");
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptoContent);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider() { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7, Key = keyBytes, IV = keyIV };
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return StringToHexString(mStream.ToArray());
        }


        ///<summary>
        ///生成随机字符串
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，默认为包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        public string GetRndString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe)
        {
            byte[] b = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = null;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }


        public string GetMD5(string source, bool output16 = false, bool toUpper = false)
        {
            var t_toUpper = toUpper ? "X2" : "x2";
            if (string.IsNullOrWhiteSpace(source)) { return string.Empty; }
            string t_md5_code = string.Empty;
            try
            {
                MD5 t_md5 = MD5.Create();
                byte[] _t = t_md5.ComputeHash(Encoding.UTF8.GetBytes(source));
                for (int i = 0; i < _t.Length; i++)
                {
                    t_md5_code += _t[i].ToString(t_toUpper);
                }
                if (output16)
                {
                    t_md5_code = t_md5_code.Substring(8, 16);
                }
            }
            catch { }
            return t_md5_code;
        }

        private static string StringToHexString(byte[] s)
        {
            string result = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                result += Convert.ToString(s[i], 16);
            }
            return result;
        }

        private static byte[] StrToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
            {
                hexString = hexString.Insert(hexString.Length - 1, 0.ToString());
            }
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }
    }
}