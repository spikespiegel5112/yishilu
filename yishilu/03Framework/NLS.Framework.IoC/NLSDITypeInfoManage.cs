using System;
using System.Collections.Generic;

namespace NLS.Framework.IoC
{
    /// <summary>
    /// 依赖注入属性维护
    /// </summary>
    public sealed class NLSDITypeInfoManage
    {
        /// <summary>
        /// 类型对应字典
        /// </summary>
        private Dictionary<Type, Type> _DITypeInfo;

        public NLSDITypeInfoManage()
        {
            _DITypeInfo = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// 添加类型对应关系
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddTypeInfo(Type key, Type value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (_DITypeInfo.ContainsKey(key))
            {
                return;
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            _DITypeInfo.Add(key, value);
        }

        /// <summary>
        /// 从字典中获取Key为传入Type对应的Value Type
        /// </summary>
        /// <param name="key">字典Key</param>
        /// <returns>获取到的Value</returns>
        public Type GetTypeInfo(Type key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (_DITypeInfo.ContainsKey(key))
            {
                return _DITypeInfo[key];
            }
            return null;
        }

        /// <summary>
        /// 检查传入Key是否存在于字典之中
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True 或 False</returns>
        public bool ContainsKey(Type key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            return _DITypeInfo.ContainsKey(key);
        }
    }
}