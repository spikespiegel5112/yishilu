namespace NLS.Framework.IoC
{
    /// <summary>
    /// Ioc容器核心
    /// </summary>
    public interface INLSIoCKernel
    {
        /// <summary>
        /// 绑定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        INLSIoCKernel Bind<T>();

        /// <summary>
        /// 绑定对象
        /// 与Bind<T> 成对使用
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        INLSIoCKernel To<U>() where U : class;

        /// <summary>
        /// 获取对象实例
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        V GetValue<V>() where V : class;

        /// <summary>
        /// 类型关联注册
        /// 等同于
        /// this.Bind<A>().To<B>();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        INLSIoCKernel Register<T, U>() where T : class where U : class;
    }
}