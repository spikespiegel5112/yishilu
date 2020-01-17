using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace NLS.ExtentionsCore.CommonUnits
{
    public sealed class CommonUnits
    {
        #region DES

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

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptoContent">待加密字符串</param>
        public static string Encryp_DES(string encryptoContent, string cryptoKey = "shnl_hud")
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

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptoContent">待解密字符串</param>
        /// <returns></returns>
        public static string Decryp_DES(string decryptoContent, string cryptoKey = "shnl_hud")
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

        #endregion DES

        /// <summary>
        ///获取指定字符串的MD5码
        ///0.1
        /// </summary>
        /// <param name="source">源字符</param>
        /// <param name="need16">是否输出16位MD5,默认false</param>
        /// <param name="toUpper">是否输出大写,默认false</param>
        public static string GetMD5(string source, bool need16 = false, bool toUpper = false)
        {
            var t_toUpper = toUpper ? "X2" : "x2";
            if (string.IsNullOrWhiteSpace(source))
            {
                return string.Empty;
            }
            string t_md5_code = string.Empty;
            try
            {
                MD5 t_md5 = MD5.Create();
                byte[] _t = t_md5.ComputeHash(Encoding.UTF8.GetBytes(source));
                for (int i = 0; i < _t.Length; i++)
                {
                    t_md5_code += _t[i].ToString(t_toUpper);
                }
                if (need16)
                {
                    t_md5_code = t_md5_code.Substring(8, 16);
                }
            }
            catch { }
            return t_md5_code;
        }

        /// <summary>
        /// 获取Base64随机数
        /// </summary>
        public static string GetNonCryptoRandomDataAsBase64(int binaryLength)
        {
            byte[] buffer = new byte[binaryLength];
            new Random().NextBytes(buffer);
            return Convert.ToBase64String(buffer);
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
        public static string GetRndString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe)
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

        /// <summary>
        /// 获取输入数据的哈希值
        /// 0.1
        /// </summary>
        public static string SHA1(string str)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(str);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }

        #region 身份证验证

        /// <summary>
        /// 验证身份证信息是否准确
        /// </summary>
        public static bool CheckIdCard(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id)) { return false; }
            if (Id.Length == 15)
            {
                return CheckIDCard15(Id);
            }
            if (Id.Length == 18)
            {
                return CheckIDCard18(Id);
            }
            return false;
        }

        private static bool CheckIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }

        private static bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }

        #endregion 身份证验证

        #region 获取订单号

        ///<summary>
        ///获取订单号
        ///</summary>
        public static string GetOrderNo
        {
            get
            {
                var radstring = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                radstring += GetRndString(19 - radstring.Length, true, false, false, false);
                return radstring;
            }
        }

        #endregion 获取订单号

        #region 序列化

        /// <summary>
        /// 序列化
        /// </summary>
        private static byte[] Serialize(object obj)
        {
            try
            {
                if (obj == null)
                    return null;

                var binaryFormatter = new BinaryFormatter();
                using (var memoryStream = new MemoryStream())
                {
                    binaryFormatter.Serialize(memoryStream, obj);
                    var data = memoryStream.ToArray();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        private static T Deserialize<T>(byte[] data)
        {
            if (data == null)
                return default(T);

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(data))
            {
                var result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }

        #endregion 序列化

        /// <summary>
        /// 获取32位GUID
        /// 不包含 ‘-’
        /// 0.1
        /// </summary>
        public static string Guid32
            => Guid.NewGuid().ToString("N");

        /// <summary>
        /// 获取自 1970/01/01 00:00:00 以来的秒数
        /// 0.1
        /// </summary>
        public static string TimestampSince1970
            => Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
    }
}