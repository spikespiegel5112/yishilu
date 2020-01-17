using System;

namespace NLS.Framework.IoC
{
    /// <summary>
    /// 属性注入属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class NLSDITypeAttribute : Attribute
    {
        public NLSDITypeAttribute()
        {
        }
    }
}