using System.ComponentModel.DataAnnotations;

namespace NLS.Framework.EF
{
    /// <summary>
    /// 实体公共抽象基类
    /// </summary>
    public abstract class AbsEntity : IFrameworkDependency
    {
        /// <summary>
        /// Id  long  自增
        /// </summary>
        [Key]
        public long Id { get; set; }
    }

    /// <summary>
    /// 实体公共抽象基类
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    public abstract class AbsEntity<T> : IFrameworkDependency where T : struct
    {
        /// <summary>
        /// Id  T  自增
        /// </summary>
        [Key]
        public T Id { get; set; }
    }
}