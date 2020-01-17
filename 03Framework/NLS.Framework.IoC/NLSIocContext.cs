namespace NLS.Framework.IoC
{
    /// <summary>
    /// IoC上下文管理模块
    /// </summary>
    public sealed class NLSIocContext
    {
        private NLSIocContext()
        {
        }

        /// <summary>
        /// _Context线程安全
        /// </summary>
        private volatile static NLSIocContext _Context;

        private static readonly object _Lock = new object();

        /// <summary>
        /// 获取上下文实例
        /// 双检锁实现单例模式
        /// </summary>
        public static NLSIocContext Context
        {
            get
            {
                if (_Context == null)
                {
                    lock (_Lock)
                    {
                        if (_Context == null)
                        {
                            _Context = new NLSIocContext();
                        }
                    }
                }
                return _Context;
            }
        }

        /// <summary>
        /// 依赖注入对象管理
        /// </summary>
        public NLSDITypeInfoManage NLSDITypeInfoManage
        {
            get;
            set;
        }

        private INLSDITypeAnalyticalProvider _NLSDITypeAnalyticalProvider;

        /// <summary>
        /// 依赖注入类型分析支持方法
        /// </summary>
        public INLSDITypeAnalyticalProvider NLSDITypeAnalyticalProvider
        {
            get
            {
                if (_NLSDITypeAnalyticalProvider == null)
                {
                    _NLSDITypeAnalyticalProvider = new NLSDefaultDITypeAnalyticalProvider();
                }
                return _NLSDITypeAnalyticalProvider;
            }
            set
            {
                _NLSDITypeAnalyticalProvider = value;
            }
        }
    }
}