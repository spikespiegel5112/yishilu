using System.ComponentModel;

namespace NLS.AspNetCore.Linq
{
    /// <summary>
    /// 分页基类
    /// </summary>
    public abstract class AbsBasePageModel
    {
        /// <summary>
        /// 页码
        /// </summary>
        [DefaultValue(1)]
        public virtual int PageIndex { get; set; }

        /// <summary>
        /// 一页显示条数
        /// </summary>
        [DefaultValue(15)]
        public virtual int PageSize { get; set; }
    }
}