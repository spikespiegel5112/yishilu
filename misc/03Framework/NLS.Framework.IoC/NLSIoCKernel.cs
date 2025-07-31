using System;

namespace NLS.Framework.IoC
{
    /// <summary>
    /// IoC核心模块
    /// </summary>
    public sealed class NLSIoCKernel : INLSIoCKernel
    {
        public NLSIoCKernel()
        {
            NLSIocContext.Context.NLSDITypeInfoManage = new NLSDITypeInfoManage();
        }

        private Type BaseType;

        /// <summary>
        /// 绑定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public INLSIoCKernel Bind<T>()
        {
            BaseType = typeof(T);
            return this;
        }

        /// <summary>
        /// 获取对象实例
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        public V GetValue<V>() where V : class
        {
            return NLSIocContext.Context.NLSDITypeAnalyticalProvider.CreateDITypeAnalaytical().GetValue<V>();
        }

        /// <summary>
        /// 绑定对象
        /// 与Bind<T> 成对使用
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        public INLSIoCKernel To<U>() where U : class
        {
            Type achieveType = typeof(U);
            if (achieveType.BaseType == this.BaseType || achieveType.GetInterface(this.BaseType.Name) != null)
            {
                NLSIocContext.Context.NLSDITypeInfoManage.AddTypeInfo(this.BaseType, achieveType);
            }
            return this;
        }

        /// <summary>
        /// 类型关联注册
        /// 等同于
        /// INLSIoCKernel.Bind<A>().To<B>();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        public INLSIoCKernel Register<T, U>() where T : class where U : class
        {
            Type _baseType = typeof(T);
            Type type = typeof(U);
            if (type.BaseType == _baseType || type.GetInterface(_baseType.Name) != null)
            {
                NLSIocContext.Context.NLSDITypeInfoManage.AddTypeInfo(_baseType, type);
            }
            return this;
        }
    }
}